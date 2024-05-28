using MassTransit;
using Microsoft.EntityFrameworkCore;
using MindfulTime.Auth.Domain.Repository.Entities;
using MindfulTime.Auth.Domain.Repository.Interfaces;
using MindfulTime.Auth.Application.Models;
using MindfulTime.Auth.Application.Interfaces;
using OpenClasses.Auth;
using OpenClasses.Calendar;
using OpenClasses.Notification;
using MindfulTime.Auth.Application.Services;

namespace MindfulTime.Auth.Services
{
    public class UserService(IBaseRepository<User> userRepository, IPublishEndpoint publishEndpoint) : IUserService
    {
        private readonly IBaseRepository<User> _userRepository = userRepository;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task<BaseResponse<UserDto>> CreateUser(UserDto user)
        {
            var cryptPassword = CryptoService.HashPassword(user.Password);

            var userFromDb = await _userRepository.ReadAsync().SingleOrDefaultAsync(x=> x.Email == user.Email);
            if (userFromDb != null) return new BaseResponse<UserDto> { ErrorMessage = $"Пользователь с {user.Email} уже есть в базе данных. Создание пользователя не возможно." };

            var userToDb = new User()
            {
                Email = user.Email,
                Id = Guid.NewGuid(),
                Name = user.Name,
                Password = cryptPassword,
                Role = user.Role,
                TelegramId = user.TelegramId,
                IsSendMessage = user.IsSendMessage
            };

            var result = await _userRepository.CreateAsync(userToDb);
            if (result.IsError) return new BaseResponse<UserDto> { ErrorMessage = result.ErrorMessage };

            UserMT publishUserToCalendarDb = new()
            {
                Email = userToDb.Email,
                Id = userToDb.Id,
                Name = userToDb.Name,
                Password = userToDb.Password,
                Role = userToDb.Role,
                IsSendMessage= user.IsSendMessage,
                TelegramId= userToDb.TelegramId
            };
            NUserMT PublishUserToNotificationDb = new()
            {
                Email = userToDb.Email,
                Id = userToDb.Id,
                Name = userToDb.Name,
                Password = userToDb.Password,
                Role = userToDb.Role,
                IsSendMessage= user.IsSendMessage,
                TelegramId = user.TelegramId
            };

            // Отправка нового пользователя в БД Календарь и БД Уведомлений
            await _publishEndpoint.Publish(publishUserToCalendarDb);
            await _publishEndpoint.Publish(PublishUserToNotificationDb);

            return new BaseResponse<UserDto>
            {
                Data = new UserDto
                {
                    Role = result.Data.Role,
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                    IsSendMessage = result.Data.IsSendMessage,
                    Password = result.Data.Password,
                    TelegramId = result.Data.TelegramId
                }
            };

        }

        public async Task<BaseResponse<UserDto>> DeleteUser(UserDto user)
        {
            var userFromDb = await _userRepository.ReadAsync().SingleOrDefaultAsync(x => x.Email == user.Email);
            if (userFromDb == null || userFromDb.Email =="admin@gmail.ru") return new BaseResponse<UserDto> { ErrorMessage = $"Пользователь с {user.Email} не найден в базе данных." };
            
            var result = await _userRepository.DeleteAsync(userFromDb);
            if (result.IsError) return new BaseResponse<UserDto> { ErrorMessage = result.ErrorMessage };

            User_del_MT PublishUserToCalendarDb = new()
            {
                Email = userFromDb.Email,
                Id = userFromDb.Id,
                Name = userFromDb.Name,
                Password = userFromDb.Password,
                Role = userFromDb.Role,
                TelegramId=userFromDb.TelegramId,
                IsSendMessage=userFromDb.IsSendMessage
            };
            NUser_del_MT PublishUserToNotificationDB = new()
            {
                Email = userFromDb.Email,
                Id = userFromDb.Id,
                Name = userFromDb.Name,
                Password = userFromDb.Password,
                Role = userFromDb.Role,
                TelegramId = userFromDb.TelegramId,
                IsSendMessage=userFromDb.IsSendMessage
            };

            //Удаление пользователя из БД Календарь и БД Уведомлений
            await _publishEndpoint.Publish(PublishUserToCalendarDb);
            await _publishEndpoint.Publish(PublishUserToNotificationDB);

            return new BaseResponse<UserDto>
            {
                Data = new UserDto
                {
                    Role = result.Data.Role,
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                    Password = result.Data.Password,
                    IsSendMessage = result.Data.IsSendMessage,
                    TelegramId = result.Data.TelegramId
                }
            };
        }

        public async Task<BaseResponse<UserDto>> ReadUser(UserDto user)
        {

            var userFromDb = await _userRepository.ReadAsync().SingleOrDefaultAsync(x => x.Email == user.Email);
            
            if (userFromDb!= null)
            {
                var passIsVerify = CryptoService.VerifyPassword(user.Password, userFromDb.Password);
                if (passIsVerify)
                {
                    UserDto userDto = new()
                    {
                        Id = userFromDb.Id,
                        Role = userFromDb.Role,
                        Name = userFromDb.Name,
                        Email = userFromDb.Email,
                        TelegramId=userFromDb.TelegramId,
                        IsSendMessage=userFromDb.IsSendMessage,
                        Password = userFromDb.Password
                    };
                    return new BaseResponse<UserDto>() { Data = userDto };
                }
            }
            return new BaseResponse<UserDto>() { ErrorMessage = "Пользователь не найден." };
        }

        public async Task<BaseResponse<List<UserDto>>> ReadAllUsers(UserDto user)
        {
            var usersFromDb = await _userRepository.ReadAsync().ToListAsync();
            var mainUser = usersFromDb.SingleOrDefault(x => x.Id == user.Id && x.Role == user.Role);
            if (mainUser!= null)
            {
                var passIsVerify = CryptoService.VerifyPassword(user.Password, mainUser.Password);
                if (passIsVerify)
                {
                    List<UserDto> users = [];
                    foreach (var _user in usersFromDb)
                    {
                        UserDto userDto = new()
                        {
                            Id = _user.Id,
                            Role = _user.Role,
                            Name = _user.Name,
                            Email = _user.Email,
                            IsSendMessage = _user.IsSendMessage,
                            TelegramId = _user.TelegramId,
                            Password = _user.Password
                        };
                        users.Add(userDto);
                    }

                    return new BaseResponse<List<UserDto>>() { Data = users };
                }
            }
            return new BaseResponse<List<UserDto>>() { ErrorMessage = "Не авторизованный запрос." };
        }

        public async Task<BaseResponse<UserDto>> UpdateUser(UserDto user)
        {
            var userFromDb = await _userRepository.ReadAsync().SingleOrDefaultAsync(x => x.Email == user.Email);
            if (userFromDb == null) return new BaseResponse<UserDto> { ErrorMessage = $"Пользователь с {user.Email} не найден в базе данных." };

            var userToDb = new User()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Role = user.Role,
                TelegramId= user.TelegramId,
                IsSendMessage= user.IsSendMessage,
            };

            var result = await _userRepository.UpdateAsync(userToDb);
            if (result.IsError) return new BaseResponse<UserDto> { ErrorMessage = result.ErrorMessage };

            User_upd_MT PublishUserToCalendarDb = new()
            {
                Email = userToDb.Email,
                Id = userToDb.Id,
                Name = userToDb.Name,
                Password = userToDb.Password,
                Role = userToDb.Role,
                IsSendMessage = userToDb.IsSendMessage,
                TelegramId = userToDb.TelegramId
            };
            NUser_upd_MT PublishUserToNotificationDb = new()
            {
                Email = userToDb.Email,
                Id = userToDb.Id,
                Name = userToDb.Name,
                Password = userToDb.Password,
                Role = userToDb.Role,
                IsSendMessage = userToDb.IsSendMessage,
                TelegramId= userToDb.TelegramId
            };

            // Обновление пользователя в БД Календарь и БД Уведомлений
            await _publishEndpoint.Publish(PublishUserToCalendarDb);
            await _publishEndpoint.Publish(PublishUserToNotificationDb);

            return new BaseResponse<UserDto>
            {
                Data = new UserDto
                {
                    Role = result.Data.Role,
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                    TelegramId = result.Data.TelegramId,
                    IsSendMessage = result.Data.IsSendMessage,
                    Password = result.Data.Password
                }
            };
        }
    }
}

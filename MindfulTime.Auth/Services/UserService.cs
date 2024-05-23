using MassTransit;
using Microsoft.EntityFrameworkCore;
using MindfulTime.Auth.Domain.Repository.Entities;
using MindfulTime.Auth.Domain.Repository.Interfaces;
using MindfulTime.Auth.DTO;
using MindfulTime.Auth.Interfaces;
using OpenClasses;

namespace MindfulTime.Auth.Services
{
    public class UserService(IBaseRepository<User> repository, IPublishEndpoint publishEndpoint) : IUserService
    {
        private readonly IBaseRepository<User> _repository = repository;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public async Task<BaseResponse<UserDto>> CreateUser(UserDto user)
        {
            var userFromDb = await _repository.ReadAsync().SingleOrDefaultAsync(x=> x.Email == user.Email);
            if (userFromDb != null) return new BaseResponse<UserDto> { ErrorMessage = $"Пользователь с {user.Email} уже есть в базе данных. Создание пользователя не возможно." };
            var userToDb = new User()
            {
                Email = user.Email,
                Id = Guid.NewGuid(),
                Name = user.Name,
                Password = user.Password,
                Role = user.Role
            };
            var result = await _repository.CreateAsync(userToDb);
            if (result.IsError) return new BaseResponse<UserDto> { ErrorMessage = result.ErrorMessage };

            UserMT publishUser = new()
            {
                Email = userToDb.Email,
                Id = userToDb.Id,
                Name = userToDb.Name,
                Password = userToDb.Password,
                Role = userToDb.Role
            };

            await _publishEndpoint.Publish(publishUser);

            return new BaseResponse<UserDto>
            {
                Data = new UserDto
                {
                    Role = result.Data.Role,
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                }
            };

        }

        public async Task<BaseResponse<UserDto>> DeleteUser(UserDto user)
        {
            var userFromDb = await _repository.ReadAsync().SingleOrDefaultAsync(x => x.Email == user.Email);
            if (userFromDb == null || userFromDb.Email =="admin@gmail.ru") return new BaseResponse<UserDto> { ErrorMessage = $"Пользователь с {user.Email} не найден в базе данных." };
            
            var result = await _repository.DeleteAsync(userFromDb);
            if (result.IsError) return new BaseResponse<UserDto> { ErrorMessage = result.ErrorMessage };
            User_del_MT publishUser = new()
            {
                Email = userFromDb.Email,
                Id = userFromDb.Id,
                Name = userFromDb.Name,
                Password = userFromDb.Password,
                Role = userFromDb.Role
            };
            await _publishEndpoint.Publish(publishUser);
            return new BaseResponse<UserDto>
            {
                Data = new UserDto
                {
                    Role = result.Data.Role,
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                }
            };
        }

        public async Task<BaseResponse<UserDto>> ReadUser(UserDto user)
        {
            var userFromDb = await _repository.ReadAsync().SingleOrDefaultAsync(x => x.Email == user.Email && x.Password == user.Password);
            if (userFromDb!= null)
            {
                UserDto userDto = new()
                {
                    Id = userFromDb.Id,
                    Role = userFromDb.Role,
                    Name = userFromDb.Name,
                    Email = userFromDb.Email
                };
                return new BaseResponse<UserDto>() { Data = userDto };

            }
            return new BaseResponse<UserDto>() { ErrorMessage = "Пользователь не найден." };
        }

        public async Task<BaseResponse<List<UserDto>>> ReadAllUsers(UserDto user)
        {
            var usersFromDb = await _repository.ReadAsync().ToListAsync();
            var mainUser = usersFromDb.SingleOrDefault(x => x.Id == user.Id && x.Role == user.Role && x.Password == user.Password);
            if (mainUser!= null)
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
                    };
                    users.Add(userDto);
                }
                
                return new BaseResponse<List<UserDto>>() { Data = users };

            }
            return new BaseResponse<List<UserDto>>() { ErrorMessage = "Не авторизованный запрос." };
        }

        public async Task<BaseResponse<UserDto>> UpdateUser(UserDto user)
        {
            var userFromDb = await _repository.ReadAsync().SingleOrDefaultAsync(x => x.Email == user.Email);
            if (userFromDb == null) return new BaseResponse<UserDto> { ErrorMessage = $"Пользователь с {user.Email} не найден в базе данных." };
            var userToDb = new User()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Role = user.Role
            };
            var result = await _repository.UpdateAsync(userToDb);
            if (result.IsError) return new BaseResponse<UserDto> { ErrorMessage = result.ErrorMessage };
            User_upd_MT publishUser = new()
            {
                Email = userToDb.Email,
                Id = userToDb.Id,
                Name = userToDb.Name,
                Password = userToDb.Password,
                Role = userToDb.Role
            };
            await _publishEndpoint.Publish(publishUser);
            return new BaseResponse<UserDto>
            {
                Data = new UserDto
                {
                    Role = result.Data.Role,
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                }
            };
        }
    }
}

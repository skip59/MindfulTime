using MindfulTime.Notification.Domain.Repository.Entities;
using OpenClasses.Notification;

namespace MindfulTime.Notification.Domain.Repository.Services
{
    public class UserNotificationRepositoryService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<BaseResponse<User>> CreateAsync(NUserMT entity)
        {
            var mapResult = Mapper(entity);
            if (mapResult == null) return new BaseResponse<User> { ErrorMessage = "Неверные входные днные при конвертации объекта" };
            if (!_context.Users.Any(x => x.Email == mapResult.Email))
            {
                await _context.Users.AddAsync(mapResult);
                await _context.SaveChangesAsync();
                return new BaseResponse<User> { Data =  mapResult };
            }
            return new BaseResponse<User> { ErrorMessage = $"Пользователь {entity.Email} уже существует в базе данных" };
        }

        public async Task<BaseResponse<User>> DeleteAsync(NUser_del_MT entity)
        {
            var mapResult = Mapper(entity);
            if (mapResult == null) return new BaseResponse<User> { ErrorMessage = "Неверные входные днные при конвертации объекта" };
            if (_context.Users.Any(x => x.Email == mapResult.Email))
            {
                _context.Users.Remove(mapResult);
                await _context.SaveChangesAsync();
                return new BaseResponse<User> { Data =  mapResult };
            }
            return new BaseResponse<User> { ErrorMessage = $"Пользователь {entity.Email} не найден" };
        }

        public IQueryable<User> ReadAsync()
        {
            return _context.Users;
        }

        public async Task<bool> UpdateAsync(NUser_upd_MT entity)
        {
            var mapResult = Mapper(entity);
            if (mapResult == null) return false;
            if (_context.Users.Any(x => x.Email == mapResult.Email))
            {
                _context.Users.Update(mapResult);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private static User Mapper<T>(T user) => user switch
        {
            NUserMT userMt => new User
            {
                Email = userMt.Email,
                Id = userMt.Id,
                Name = userMt.Name,
                Password = userMt.Password,
                Role = userMt.Role
            },
            NUser_del_MT userDelMt => new User
            {
                Email = userDelMt.Email,
                Id = userDelMt.Id,
                Name = userDelMt.Name,
                Password = userDelMt.Password,
                Role = userDelMt.Role
            },
            NUser_upd_MT userUpdMt => new User
            {
                Email = userUpdMt.Email,
                Id = userUpdMt.Id,
                Name = userUpdMt.Name,
                Password = userUpdMt.Password,
                Role = userUpdMt.Role
            },
            _ => null,
        };
    }
}

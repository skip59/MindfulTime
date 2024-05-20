using MindfulTime.Calendar.Domain.Repository.Entities;
using OpenClasses;

namespace MindfulTime.Calendar.Domain.Repository.Services
{
    public class UserCalendarRepositoryService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<BaseResponse<User>> CreateAsync(UserMT entity)
        {
            var mapResult = Mapper(entity);
            if (mapResult == null) return new BaseResponse<User> { ErrorMessage = "Неверные входные днные при конвертации объекта" };
            var users = await Task.Run(ReadAsync);
            if (users.Any(x => x.Email == mapResult.Email))
            {
                if(await UpdateAsync(entity)) return new BaseResponse<User> { Data =  mapResult };
            }
            await _context.Users.AddAsync(mapResult);
            await _context.SaveChangesAsync();
            return new BaseResponse<User> { Data =  mapResult };
        }

        public Task<BaseResponse<User>> DeleteAsync(UserMT entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> ReadAsync()
        {
            return _context.Users;
        }

        public async Task<bool> UpdateAsync(UserMT entity)
        {
            if (entity == null) return false;
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

        private static User Mapper(UserMT user)
        {
            if (user == null)
            {
                return null;
            }
            return new User
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Role = user.Role
            };
        }
    }
}

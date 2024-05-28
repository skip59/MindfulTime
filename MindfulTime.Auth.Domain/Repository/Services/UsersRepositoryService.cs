using MindfulTime.Auth.Domain.Repository.Entities;
using MindfulTime.Auth.Domain.Repository.Interfaces;

namespace MindfulTime.Auth.Domain.Repository.Services
{
    public class UsersRepositoryService(ApplicationDbContext context) : IBaseRepository<User>
    {
        readonly ApplicationDbContext _context = context;
        public async Task<BaseResponse<User>> CreateAsync(User entity)
        {
            try
            {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new BaseResponse<User> { Data = _context.Users.Single(x => x.Id == entity.Id) };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User> { ErrorMessage = ex.Message };
            }

        }

        public async Task<BaseResponse<User>> DeleteAsync(User entity)
        {
            try
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
                return new BaseResponse<User> { Data = entity };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User> { ErrorMessage = ex.Message };
            }

        }

        public IQueryable<User> ReadAsync()
        {
            try
            {
                return _context.Users;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<BaseResponse<User>> UpdateAsync(User entity)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(entity.Id);
                entity.Password = existingUser!.Password;
                _context.Users.Entry(existingUser!).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return new BaseResponse<User> { Data = entity };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User> { ErrorMessage = ex.Message };
            }

        }
    }
}

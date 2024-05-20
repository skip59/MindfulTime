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
                _context.Users.Add(entity);
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
                return _context.Users.Select(x => x);
            }
            catch (Exception ex)
            {
                return null!;
            }

        }

        public async Task<BaseResponse<User>> UpdateAsync(User entity)
        {
            try
            {
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
                return new BaseResponse<User> { Data = _context.Users.Single(x => x.Id == entity.Id) };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User> { ErrorMessage = ex.Message };
            }

        }
    }
}

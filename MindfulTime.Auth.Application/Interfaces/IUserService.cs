using MindfulTime.Auth.Application.Models;
using OpenClasses.Auth;

namespace MindfulTime.Auth.Application.Interfaces
{
    public interface IUserService
    {
        public Task<BaseResponse<UserDto>> CreateUser(UserDto user);
        public Task<BaseResponse<UserDto>> DeleteUser(UserDto user);
        public Task<BaseResponse<UserDto>> UpdateUser(UserDto user);
        public Task<BaseResponse<List<UserDto>>> ReadAllUsers(UserDto user);
        public Task<BaseResponse<UserDto>> ReadUser(UserDto user);
    }
}

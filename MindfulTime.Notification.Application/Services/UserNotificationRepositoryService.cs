using UserEntity = MindfulTime.Notification.Infrastructure.Entities.UserEntity;

namespace MindfulTime.Notification.Domain.Services;

public class UserNotificationRepositoryService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;
    public async Task<BaseResponse<UserEntity>> CreateAsync(NUserMT entity)
    {
        var mapResult = Mapper(entity);
        if (mapResult == null) return new BaseResponse<UserEntity> { ErrorMessage = "Неверные входные днные при конвертации объекта" };
        if (!_context.Users.Any(x => x.Email == mapResult.Email))
        {
            await _context.Users.AddAsync(mapResult);
            await _context.SaveChangesAsync();
            return new BaseResponse<UserEntity> { Data = mapResult };
        }
        return new BaseResponse<UserEntity> { ErrorMessage = $"Пользователь {entity.Email} уже существует в базе данных" };
    }

    public async Task<BaseResponse<UserEntity>> DeleteAsync(NUser_del_MT entity)
    {
        var mapResult = Mapper(entity);
        if (mapResult == null) return new BaseResponse<UserEntity> { ErrorMessage = "Неверные входные днные при конвертации объекта" };
        if (_context.Users.Any(x => x.Email == mapResult.Email))
        {
            _context.Users.Remove(mapResult);
            await _context.SaveChangesAsync();
            return new BaseResponse<UserEntity> { Data = mapResult };
        }
        return new BaseResponse<UserEntity> { ErrorMessage = $"Пользователь {entity.Email} не найден" };
    }

    public IQueryable<UserEntity> ReadAsync()
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

    private static UserEntity Mapper<T>(T user) => user switch
    {
        NUserMT userMt => new UserEntity
        {
            Email = userMt.Email,
            Id = userMt.Id,
            Name = userMt.Name,
            Password = userMt.Password,
            Role = userMt.Role,
            IsSendMessage = userMt.IsSendMessage,
            TelegramId = userMt.TelegramId
        },
        NUser_del_MT userDelMt => new UserEntity
        {
            Email = userDelMt.Email,
            Id = userDelMt.Id,
            Name = userDelMt.Name,
            Password = userDelMt.Password,
            Role = userDelMt.Role,
            IsSendMessage = userDelMt.IsSendMessage,
            TelegramId = userDelMt.TelegramId
        },
        NUser_upd_MT userUpdMt => new UserEntity
        {
            Email = userUpdMt.Email,
            Id = userUpdMt.Id,
            Name = userUpdMt.Name,
            Password = userUpdMt.Password,
            Role = userUpdMt.Role,
            IsSendMessage = userUpdMt.IsSendMessage,
            TelegramId = userUpdMt.TelegramId
        },
        _ => null,
    };
}

namespace MindfulTime.Auth.Controllers;

[Route("api/[action]")]
[ApiController]
public class AuthController(IUserService user, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IUserService _userService = user;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> CheckUser(UserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new BadHttpRequestException("Данные не валидны");
            }

            var user = await _userService.ReadUser(userDto);
            if (user.isError) throw new BadHttpRequestException(user.ErrorMessage);
            _logger.LogInformation($"Пользователь с ID={user.Data.Id} авторизован");

            return Ok(user.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в работе сервиса авторизации.", ex);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new BadHttpRequestException("Данные не валидны");
            }

            var user = await _userService.CreateUser(userDto);
            if (user.isError) throw new BadHttpRequestException(user.ErrorMessage);
            _logger.LogInformation($"Пользователь с ID={user.Data.Id} создан");

            return Ok(user.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в работе сервиса авторизации.", ex);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> GetUsers(UserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new BadHttpRequestException("Данные не валидны");
            }

            var users = await _userService.ReadAllUsers(userDto);
            if (users.isError) throw new BadHttpRequestException(users.ErrorMessage);
            _logger.LogInformation($"Пользователи получены");

            return Ok(users.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в работе сервиса авторизации.", ex);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser(UserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new BadHttpRequestException("Данные не валидны");
            }

            var users = await _userService.UpdateUser(userDto);
            if (users.isError) throw new BadHttpRequestException(users.ErrorMessage);
            _logger.LogInformation($"Пользователь с обновлен");

            return Ok(users.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в работе сервиса авторизации.", ex);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(UserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new BadHttpRequestException("Данные не валидны");
            }

            var users = await _userService.DeleteUser(userDto);
            if (users.isError) throw new BadHttpRequestException(users.ErrorMessage);
            _logger.LogInformation($"Пользователь удален");

            return Ok(users.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в работе сервиса авторизации.", ex);
        }

        return Ok();
    }

}

namespace MindfulTime.Auth.Infrastructure.Crypto;

public static class CryptoService
{
    public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public static bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}

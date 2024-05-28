namespace MindfulTime.Auth.Application.Services
{
    internal static class CryptoService
    {
        internal static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        internal static bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

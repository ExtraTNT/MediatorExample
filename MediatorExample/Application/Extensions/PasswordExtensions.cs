namespace MediatorExample.Application.Extensions;

public static class PasswordExtensions
{
    /// <summary>
    /// Basic hash extension
    /// </summary>
    /// <param name="password">Input string</param>
    /// <returns>Hashed input string</returns>
    public static string Hash(this string password) => BCrypt.Net.BCrypt.HashPassword(password, Constants.Salt);

    /// <summary>
    /// Extension to check strings against a hased string
    /// </summary>
    /// <param name="password">Input string</param>
    /// <param name="hash">Test string</param>
    /// <returns>IsEquals</returns>
    public static bool Check(this string password, string hash) => password.Hash().Equals(hash);
}
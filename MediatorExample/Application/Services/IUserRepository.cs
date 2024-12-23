using MediatorExample.Application.Domain;

namespace MediatorExample.Application.Services;

public interface IUserRepository
{
    /// <summary>
    /// Add a new user to the User DB using email and password
    /// </summary>
    /// <param name="email">Email of user</param>
    /// <param name="password">Hashed passwd</param>
    /// <returns>Success</returns>
    bool AddUser(string email, string password);
    /// <summary>
    /// User by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns>User with matching email</returns>
    User? GetUserByEmail(string email);
    // Add other necessary methods
    /// <summary>
    /// Changes the password of a user
    /// </summary>
    /// <param name="email">Email of user</param>
    /// <param name="password">Hashed passwd</param>
    /// <returns>Success</returns>
    bool ChangeUserPasswd(string email, string password);
}


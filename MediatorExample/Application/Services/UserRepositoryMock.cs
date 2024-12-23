using MediatorExample.Application.Domain;
using MediatorExample.Application.Extensions;

namespace MediatorExample.Application.Services;

/// <summary>
/// Mock for  a user db, sets up a default user (admin@localhost) with the password 1234 and id 1, memory only
/// </summary>
public class UserRepositoryMock : IUserRepository
{
    // initial mock db
    private readonly List<User> _dbMock = [
        new() {
            Email = "admin@localhost", Id = 1, Password = "1234".Hash(), PasswordVersion = 0
        }
    ];
    /// <inheritdoc />
    public bool AddUser(string email, string password) {
        if (_dbMock.Exists(u => u.Email == email))
        {
            return false;
        }
        _dbMock.Add( new User() {Id = _dbMock.Max(x => x.Id) + 1, Email = email, Password = password });
        return true;
    }

    /// <inheritdoc />
    public User? GetUserByEmail(string email) {
        return _dbMock.FirstOrDefault(u => u.Email.Equals(email));
    }

    /// <inheritdoc />
    public bool ChangeUserPasswd(string email, string password) {
        var user = GetUserByEmail(email);
        if (user == null) return false;
        user.Password = password;
        return true;
    }
}

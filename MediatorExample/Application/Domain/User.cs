using System.Text.Json.Serialization;

namespace MediatorExample.Application.Domain;

public class User
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    [JsonIgnore]
    public int PasswordVersion { get; set; } = 1;
    
    [JsonIgnore]
    public int Id { get; set; }
}
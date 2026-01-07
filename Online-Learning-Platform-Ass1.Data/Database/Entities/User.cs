namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreateAt { get; set; }

    public Guid? RoleId { get; set; }
    public Role? Role { get; set; }

    public Profile? Profile { get; set; }
}

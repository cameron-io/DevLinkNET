namespace API.Dtos.Account;

public class UserDto
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string Avatar { get; set; }
}
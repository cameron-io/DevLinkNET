using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Account;

public class RegisterDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [RegularExpression(
        "(?=^.{6,20}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
        ErrorMessage =
            "Password must have at least: " +
            "1 Uppercase, 1 Lowercase, 1 Number, 1 Special Character " +
            "and 6 characters")]
    public required string Password { get; set; }
}
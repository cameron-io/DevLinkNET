using System.ComponentModel.DataAnnotations;
using API.Dtos.Account;

namespace API.Dtos.Profile;

public class ProfileDto
{
    public int Id { get; set; }
    [Required]
    public required string Status { get; set; }
    [Required]
    public List<string> Skills { get; set; } = [];

    public string Company { get; set; }
    public string Website { get; set; }
    public string Location { get; set; }
    public string Bio { get; set; }
    public string GitHubUsername { get; set; }
    public List<ExperienceDto> Experience { get; set; } = [];
    public List<EducationDto> Education { get; set; } = [];
    public SocialDto Social { get; set; }
    public UserDto User { get; set; }
}

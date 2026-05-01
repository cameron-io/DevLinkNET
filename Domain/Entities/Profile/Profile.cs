namespace Domain.Entities;

public class Profile : BaseEntity
{
    public string Status { get; set; }
    public List<string> Skills { get; set; } = [];

    public string Company { get; set; }
    public string Website { get; set; }
    public string Location { get; set; }
    public string Bio { get; set; }
    public string GitHubUsername { get; set; }
    public List<Experience> Experience { get; set; } = [];
    public List<Education> Education { get; set; } = [];
    public List<Social> Social { get; set; } = [];

    public int AppUserId { get; set; } // Required foreign key property
    public AppUser AppUser { get; set; } = null!; // Required reference navigation to principal
}

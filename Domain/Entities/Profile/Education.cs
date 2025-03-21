namespace Domain.Entities;

public class Education: BaseEntity
{
    public string School { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public string From { get; set; }
    public string To { get; set; } = null;
    public bool Current { get; set; } = false;
    public string Description { get; set; }
    
    public int ProfileId { get; set; } // Required foreign key property
    public Profile Profile { get; set; } = null!; // Required reference navigation to principal
}

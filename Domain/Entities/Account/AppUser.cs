using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public Profile Profile { get; set; }
    public List<Post> Post { get; set; }
    public List<Comment> Comments { get; set; }
    public List<UserLike> UserLikes { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
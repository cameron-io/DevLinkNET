﻿using Microsoft.AspNetCore.Identity;

namespace Core.Data;

public class AppUser : IdentityUser<int>
{
    public string DisplayName { get; set; }
    public Profile Profile { get; set; }
    public List<Post> Post { get; set; }
    public List<Comment> Comments { get; set; }
    public List<UserLike> UserLikes { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
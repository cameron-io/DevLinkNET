using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class UserManagerExtensions
{
    public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> userManager, 
        ClaimsPrincipal user)
    {
        return await userManager.Users
            .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
    }
}
using System.Net.NetworkInformation;
using System.Text.Json;
using Domain.Entities;
using Domain.Services;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seed;

public class Seed
{
    private static readonly JsonSerializerOptions options = JsonSerializerOptionsProvider.GetOptions();

    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IProfileService profileService)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("../Infrastructure/Data/Seed/JsonData/UserSeed.json");

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;

        var profileData = await File.ReadAllTextAsync("../Infrastructure/Data/Seed/JsonData/ProfileSeed.json");
        var profile = JsonSerializer.Deserialize<Profile>(profileData, options);

        if (profile == null) return;

        var roles = new List<AppRole>
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName!.ToLower();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");

            profile.AppUser = user;
            profileService.UpsertAsync(profile);
        }

        var admin = new AppUser
        {
            UserName = "admin",
            Name = "Admin"
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
    }
}
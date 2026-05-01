using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services;
using API.Dtos.Profile;
using API.Dtos;
using API.Extensions;

namespace API.Controllers;

[ApiController]
[Route("api/profiles")]
public class ProfileController(
    UserManager<AppUser> userManager,
    IProfileService profileService
) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProfileDto>>> GetAllProfiles()
    {
        var profiles = await profileService.ListAllProfilesAsync();

        if (profiles == null) return NotFound();

        return Ok(
           profiles.Select(DtoMapper.ToProfileDTOMap).ToList()
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDto>> GetProfileById(int id)
    {
        var profile = await profileService.GetProfileIdAsync(id);

        if (profile == null) return NotFound();

        return DtoMapper.ToProfileDTOMap(profile);
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<ProfileDto>> GetByUserId(int id)
    {
        var profile = await profileService.GetProfileByUserIdAsync(id);

        if (profile == null) return NotFound();

        return DtoMapper.ToProfileDTOMap(profile);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<ProfileDto>> GetMe()
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var profile = await profileService.GetProfileByUserIdAsync(user.Id);

        if (profile == null) return NotFound();

        return DtoMapper.ToProfileDTOMap(profile);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProfileDto>> UpsertProfile(ProfileDto profileDto)
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

        var profile = DtoMapper.ToProfileMap(profileDto);

        profile.AppUser = user;

        if (await profileService.UpsertAsync(profile)) return DtoMapper.ToProfileDTOMap(profile);

        return BadRequest("Failed to update user profile");
    }

    [Authorize]
    [HttpPut("experience")]
    public async Task<ActionResult<ProfileDto>> AddExperience(ExperienceDto experienceDto)
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var profile = await profileService.GetProfileByUserIdAsync(user.Id);
        var experience = DtoMapper.ToExperienceMap(experienceDto);

        experience.Profile = profile;

        if (await profileService.UpsertExperienceAsync(experience))
        {
            var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
            return DtoMapper.ToProfileDTOMap(newProfile);
        }
        return BadRequest("Failed to update user profile");
    }

    [Authorize]
    [HttpPut("education")]
    public async Task<ActionResult<ProfileDto>> AddEducation(EducationDto educationDto)
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var profile = await profileService.GetProfileByUserIdAsync(user.Id);
        var education = DtoMapper.ToEducationMap(educationDto);

        education.Profile = profile;

        if (await profileService.UpsertEducationAsync(education))
        {
            var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
            return DtoMapper.ToProfileDTOMap(newProfile);
        }
        return BadRequest("Failed to update user profile");
    }

    [Authorize]
    [HttpDelete("experience/{id}")]
    public async Task<ActionResult<ProfileDto>> DeleteExperience(int id)
    {
        if (await profileService.DeleteExperienceAsync(id))
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
            var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
            return DtoMapper.ToProfileDTOMap(newProfile);
        }
        return BadRequest("Failed to update user profile");
    }

    [Authorize]
    [HttpDelete("education/{id}")]
    public async Task<ActionResult<ProfileDto>> DeleteEducation(int id)
    {
        if (await profileService.DeleteEducationAsync(id))
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
            var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
            return DtoMapper.ToProfileDTOMap(newProfile);
        }
        return BadRequest("Failed to update user profile");
    }
}
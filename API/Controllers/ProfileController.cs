using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services;
using API.Dtos.Profile;
using API.Dtos;
using API.Extensions;
using System.Net.Http.Headers;
using System.Text.Json;

namespace API.Controllers;

[ApiController]
[Route("api/profiles")]
public class ProfileController(
    UserManager<AppUser> userManager,
    IProfileService profileService,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<ProfileController> logger
) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<ProfileController> _logger = logger;

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

    [HttpGet("github/{username}")]
    public async Task<ActionResult> GetGithubRepos(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest(new { msg = "Username is required" });
        }

        var githubClientId = _configuration["GITHUB_CLIENT_ID"];
        var githubClientSecret = _configuration["GITHUB_CLIENT_SECRET"];
        var githubToken = _configuration["GITHUB_TOKEN"];

        if (string.IsNullOrWhiteSpace(githubClientId) || string.IsNullOrWhiteSpace(githubClientSecret))
        {
            _logger.LogWarning("⚠️ GitHub OAuth credentials missing, using unauthenticated rate limits");
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("DevLinkNET");
        client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

        if (!string.IsNullOrWhiteSpace(githubToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", githubToken);
        }

        var queryParams = new List<string>
        {
            "per_page=5",
            "sort=created",
            "direction=asc"
        };

        if (!string.IsNullOrWhiteSpace(githubClientId))
        {
            queryParams.Add($"client_id={Uri.EscapeDataString(githubClientId)}");
        }

        if (!string.IsNullOrWhiteSpace(githubClientSecret))
        {
            queryParams.Add($"client_secret={Uri.EscapeDataString(githubClientSecret)}");
        }

        var url = $"https://api.github.com/users/{Uri.EscapeDataString(username)}/repos?{string.Join('&', queryParams)}";

        try
        {
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<object>(body);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, json);
            }

            return Ok(json);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "GitHub API Error: {Message}", ex.Message);
            return StatusCode(500, new { msg = "Server error" });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProfileDto>> UpsertProfile(ProfileDto profileDto)
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var profile = DtoMapper.ToProfileMap(profileDto);

        profile.AppUser = user;
        profileService.UpsertAsync(profile);

        return DtoMapper.ToProfileDTOMap(profile);
    }

    [Authorize]
    [HttpPut("experience")]
    public async Task<ActionResult<ProfileDto>> AddExperience(ExperienceDto experienceDto)
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var profile = await profileService.GetProfileByUserIdAsync(user.Id);
        var experience = DtoMapper.ToExperienceMap(experienceDto);

        experience.Profile = profile;

        profileService.UpsertExperienceAsync(experience);
        var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
        return DtoMapper.ToProfileDTOMap(newProfile);
    }

    [Authorize]
    [HttpPut("education")]
    public async Task<ActionResult<ProfileDto>> AddEducation(EducationDto educationDto)
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var profile = await profileService.GetProfileByUserIdAsync(user.Id);
        var education = DtoMapper.ToEducationMap(educationDto);

        education.Profile = profile;
        profileService.UpsertEducationAsync(education);
        var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
        return DtoMapper.ToProfileDTOMap(newProfile);
    }

    [Authorize]
    [HttpDelete("experience/{id}")]
    public async Task<ActionResult<ProfileDto>> DeleteExperience(int id)
    {
        profileService.DeleteExperienceAsync(id);
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
        return DtoMapper.ToProfileDTOMap(newProfile);
    }

    [Authorize]
    [HttpDelete("education/{id}")]
    public async Task<ActionResult<ProfileDto>> DeleteEducationAsync(int id)
    {
        profileService.DeleteEducationAsync(id);
        var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
        var newProfile = await profileService.GetProfileByUserIdAsync(user.Id);
        return DtoMapper.ToProfileDTOMap(newProfile);
    }
}
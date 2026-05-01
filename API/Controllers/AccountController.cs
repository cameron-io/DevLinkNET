using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Infrastructure.Services;
using API.Dtos.Account;
using API.Errors;
using API.Extensions;

namespace API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITokenService<AppUser> tokenService) : ControllerBase
{
    [Authorize]
    [HttpGet("info")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await userManager.FindByEmailFromClaimsPrincipal(User);

        if (user == null) return Unauthorized(new ApiResponse(401));

        return new UserDto
        {
            Email = user.Email,
            Token = tokenService.CreateToken(user),
            Name = user.Name
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized(new ApiResponse(401));

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        Response.Cookies.Append("token", tokenService.CreateToken(user), new CookieOptions
        {
            Expires = DateTime.Now.AddHours(3),
            HttpOnly = true
        });

        return Ok();
    }

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("token");

        return Ok();
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
        {
            return new BadRequestObjectResult(new ApiValidationErrorResponse
            { Errors = ["Email address is in use"] });
        }

        var user = new AppUser
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(new ApiResponse(400));

        return await Login(new LoginDto
        {
            Email = registerDto.Email,
            Password = registerDto.Password
        });
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await userManager.FindByEmailAsync(email) != null;
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteAccount()
    {
        var user = await userManager.FindByEmailFromClaimsPrincipal(User);
        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded) return BadRequest(new ApiResponse(400));

        return NoContent();
    }
}
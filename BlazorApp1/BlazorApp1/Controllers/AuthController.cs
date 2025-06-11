using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlazorApp1.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Google.Apis.Auth.AspNetCore3;

namespace BlazorApp1.Controllers;

[ApiController]
[Route ("[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController (
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AuthController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet ("google-callback")]
    public async Task<IActionResult> GoogleCallback ()
    {
        try
        {
            var result = await HttpContext.AuthenticateAsync (GoogleOpenIdConnectDefaults.AuthenticationScheme);
            _logger.LogError ("Google callback call");
            if (!result.Succeeded)
            {
                _logger.LogError ("Google authentication failed");
                return Redirect ("/Account/Login?error=google_auth_failed");
            }

            var email = result.Principal.FindFirstValue (ClaimTypes.Email);
            var name = result.Principal.FindFirstValue (ClaimTypes.Name);
            var googleId = result.Principal.FindFirstValue (ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty (email))
            {
                _logger.LogError ("Email claim not found in Google authentication");
                return Redirect ("/Account/Login?error=email_not_found");
            }

            // Check if user exists
            var user = await _userManager.FindByEmailAsync (email);

            if (user == null)
            {
                // Create new user
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true // Google emails are verified
                };

                var createResult = await _userManager.CreateAsync (user);
                if (!createResult.Succeeded)
                {
                    _logger.LogError ("Failed to create user: {Errors}",
                        string.Join (", ", createResult.Errors.Select (e => e.Description)));
                    return Redirect ("/Account/Login?error=user_creation_failed");
                }
            }

            // Add Google login if not already added
            var logins = await _userManager.GetLoginsAsync (user);
            if (!logins.Any (l => l.LoginProvider == GoogleOpenIdConnectDefaults.AuthenticationScheme))
            {
                await _userManager.AddLoginAsync (user, new UserLoginInfo (
                    GoogleOpenIdConnectDefaults.AuthenticationScheme,
                    googleId,
                    "Google"));
            }

            // Sign in the user
            await _signInManager.SignInAsync (user, isPersistent: true);

            // Store tokens in session if needed
            var tokens = result.Properties?.GetTokens ();
            if (tokens != null)
            {
                HttpContext.Session.SetString ("GoogleTokens", JsonSerializer.Serialize (tokens));
            }

            return Redirect ("/");
        }
        catch (Exception ex)
        {
            _logger.LogError (ex, "Error during Google callback");
            return Redirect ("/Account/Login?error=unexpected_error");
        }
    }

    [HttpGet ("google-login")]
    public IActionResult GoogleLogin (string returnUrl = "/")
    {
        _logger.LogInformation ("Executing google-login");
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action ("GoogleCallback"),
            Items =
            {
                { "returnUrl", returnUrl }
            }
        };

        return Challenge (properties, GoogleOpenIdConnectDefaults.AuthenticationScheme);
    }
}
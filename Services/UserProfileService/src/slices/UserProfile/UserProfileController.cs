using Microsoft.AspNetCore.Mvc;

namespace UserProfileService.slices.UserProfile;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly ILogger<UserProfileController> _logger;
    private readonly UserProfileHandler _userProfileHandler;
    public UserProfileController(ILogger<UserProfileController> logger, UserProfileHandler userProfileHandler)
    {
        _logger = logger;
        _userProfileHandler = userProfileHandler;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserProfile(int userId)
    {
        _logger.LogInformation("GetUserProfile called.");

        var profile = await _userProfileHandler.HandleUserProfile(userId);
        if (profile == null)
        {
            _logger.LogInformation($"UserProfile not found for ID: {userId}");
            return NotFound();
        }
        return Ok(profile);
    }
}
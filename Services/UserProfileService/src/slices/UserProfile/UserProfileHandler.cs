using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.UserModels;

namespace UserProfileService.slices.UserProfile;
public class UserProfileHandler
{
    private readonly SharedDbContext _dbContext;
    private readonly ILogger<UserProfileHandler> _logger;

    public UserProfileHandler(ILogger<UserProfileHandler> logger, SharedDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets the user profile by user ID.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserProfileModel?> HandleUserProfile(int userId)
    {
        var profile = _dbContext.UserProfiles.FirstOrDefault(p => p.UserId == userId);
        if (profile != null)
            _logger.LogInformation($"Fetched user profile with ID: {profile?.UserId}, Name: {profile?.Name}");
        else
            _logger.LogInformation($"UserProfile not found for ID: {userId}");
        return await Task.FromResult(profile);
    }
}

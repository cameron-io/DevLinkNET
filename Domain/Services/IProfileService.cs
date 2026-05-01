using Domain.Entities;

namespace Domain.Services;

public interface IProfileService
{
    Task<Profile> GetProfileIdAsync(int id);
    Task<Profile> GetProfileByUserIdAsync(int appUserId);
    Task<IReadOnlyList<Profile>> ListAllProfilesAsync();
    Task<Experience> GetProfileExperienceByIdAsync(int Id);
    Task<Education> GetProfileEducationByIdAsync(int Id);
    void UpsertAsync(Profile profile);
    void UpsertExperienceAsync(Experience experience);
    void UpsertEducationAsync(Education education);
    void DeleteExperienceAsync(int Id);
    void DeleteEducationAsync(int Id);
}
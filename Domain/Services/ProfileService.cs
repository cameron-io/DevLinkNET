using Domain.Entities;
using Domain.Repositories;
using Domain.Specifications;

namespace Domain.Services;

public class ProfileService(
    IGenericRepository<Profile> profileRepository,
    IGenericRepository<Experience> experienceRepository,
    IGenericRepository<Education> educationRepository
    ) : IProfileService
{

    public async Task<IReadOnlyList<Profile>> ListAllProfilesAsync()
    {
        var spec = new ProfileSpecification();
        return await profileRepository.ListAsync(spec);
    }

    public async Task<Profile> GetProfileIdAsync(int Id)
    {
        var spec = new ProfileSpecification(Id);
        return await profileRepository.GetEntityWithSpecAsync(spec);
    }

    public async Task<Profile> GetProfileByUserIdAsync(int appUserId)
    {
        var spec = new ProfileByUserIdSpecification(appUserId);
        return await profileRepository.GetEntityWithSpecAsync(spec);
    }

    public async Task<Experience> GetProfileExperienceByIdAsync(int Id)
    {
        return await experienceRepository.GetByIdAsync(Id);
    }

    public async Task<Education> GetProfileEducationByIdAsync(int Id)
    {
        return await educationRepository.GetByIdAsync(Id);
    }

    public async void UpsertAsync(Profile profile)
    {
        await profileRepository.UpsertAsync(profile);
    }

    public async void DeleteExperienceAsync(int Id)
    {
        var experience = await GetProfileExperienceByIdAsync(Id);
        await experienceRepository.DeleteAsync(experience);
    }

    public async void DeleteEducationAsync(int Id)
    {
        var Education = await GetProfileEducationByIdAsync(Id);
        await educationRepository.DeleteAsync(Education);
    }

    public async void UpsertExperienceAsync(Experience experience)
    {
        await experienceRepository.UpsertAsync(experience);
    }

    public async void UpsertEducationAsync(Education education)
    {
        await educationRepository.UpsertAsync(education);
    }
}
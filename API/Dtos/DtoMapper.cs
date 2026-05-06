using API.Dtos.Account;
using API.Dtos.Profile;
using Domain.Entities;

namespace API.Dtos;

public class DtoMapper
{
    public static UserDto ToUserDTOMap(AppUser user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Avatar = user.AvatarUrl
        };
    }

    public static ProfileDto ToProfileDTOMap(Domain.Entities.Profile profile)
    {
        return new ProfileDto()
        {
            Id = profile.Id,
            Status = profile.Status,
            Skills = profile.Skills,
            Company = profile.Company,
            Website = profile.Website,
            Location = profile.Location,
            Bio = profile.Bio,
            GitHubUsername = profile.GitHubUsername,
            Experience = [.. profile.Experience.Select(e => new ExperienceDto()
            {
                Id = e.Id,
                Title = e.Title,
                Company = e.Company,
                Location = e.Location,
                From = e.From,
                To = e.To,
                Current = e.Current,
                Description = e.Description
            })],
            Education = [.. profile.Education.Select(ed => new EducationDto()
            {
                Id = ed.Id,
                School = ed.School,
                Degree = ed.Degree,
                FieldOfStudy = ed.FieldOfStudy,
                From = ed.From,
                To = ed.To,
                Current = ed.Current,
                Description = ed.Description
            })],
            Social = profile.Social == null ? null : ToSocialDTOMap(profile.Social),
            User = ToUserDTOMap(profile.AppUser)
        };
    }

    public static Domain.Entities.Profile ToProfileMap(ProfileDto profileDto)
    {
        return new Domain.Entities.Profile()
        {
            Id = profileDto.Id,
            Status = profileDto.Status,
            Skills = profileDto.Skills,
            Company = profileDto.Company,
            Website = profileDto.Website,
            Location = profileDto.Location,
            Bio = profileDto.Bio,
            GitHubUsername = profileDto.GitHubUsername,
            Experience = [.. profileDto.Experience.Select(ToExperienceMap)],
            Education = [.. profileDto.Education.Select(ToEducationMap)],
            Social = ToSocialMap(profileDto.Social)
        };
    }

    public static EducationDto ToEducationDTOMap(Education education)
    {
        return new EducationDto()
        {
            Id = education.Id,
            School = education.School,
            Degree = education.Degree,
            FieldOfStudy = education.FieldOfStudy,
            From = education.From,
            To = education.To == "" ? null : education.To,
            Current = education.Current,
            Description = education.Description
        };
    }

    public static Education ToEducationMap(EducationDto educationDto)
    {
        return new Education()
        {
            Id = educationDto.Id,
            School = educationDto.School,
            Degree = educationDto.Degree,
            FieldOfStudy = educationDto.FieldOfStudy,
            From = educationDto.From,
            To = educationDto.To == null ? "" : educationDto.To,
            Current = educationDto.Current,
            Description = educationDto.Description
        };
    }

    public static ExperienceDto ToExperienceDTOMap(Experience experience)
    {
        return new ExperienceDto()
        {
            Id = experience.Id,
            Title = experience.Title,
            Company = experience.Company,
            Location = experience.Location,
            From = experience.From,
            To = experience.To == "" ? null : experience.To,
            Current = experience.Current,
            Description = experience.Description
        };
    }

    public static Experience ToExperienceMap(ExperienceDto experienceDto)
    {
        return new Experience()
        {
            Id = experienceDto.Id,
            Title = experienceDto.Title,
            Company = experienceDto.Company,
            Location = experienceDto.Location,
            From = experienceDto.From,
            To = experienceDto.To == null ? "" : experienceDto.To,
            Current = experienceDto.Current,
            Description = experienceDto.Description
        };
    }

    public static SocialDto ToSocialDTOMap(Social social)
    {
        return new SocialDto()
        {
            YouTube = social.YouTube,
            Twitter = social.Twitter,
            Facebook = social.Facebook,
            LinkedIn = social.LinkedIn,
            Instagram = social.Instagram
        };
    }

    public static Social ToSocialMap(SocialDto social)
    {
        return new Social()
        {
            YouTube = social.YouTube,
            Twitter = social.Twitter,
            Facebook = social.Facebook,
            LinkedIn = social.LinkedIn,
            Instagram = social.Instagram
        };
    }
}
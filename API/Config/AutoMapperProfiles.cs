using Domain.Entities;
using API.Dtos.Account;
using API.Dtos.Profile;

namespace API.Config;

public class AutoMapperProfiles : AutoMapper.Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<Profile, ProfileDto>()
            .ForMember(x => x.User, o => o.MapFrom(s => s.AppUser));
        CreateMap<ProfileDto, Profile>();
        CreateMap<Experience, ExperienceDto>();
        CreateMap<ExperienceDto, Experience>()
            .ForMember(x => x.To, o => o.MapFrom(s => s.To == "" ? null : s.To));
        CreateMap<Education, EducationDto>();
        CreateMap<EducationDto, Education>()
            .ForMember(x => x.To, o => o.MapFrom(s => s.To == "" ? null : s.To));
        CreateMap<Social, SocialDto>();
    }
}
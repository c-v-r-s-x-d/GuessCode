using AutoMapper;
using GuessCode.API.Models.V1.Auth;
using GuessCode.API.Models.V1.Kata;
using GuessCode.API.Models.V1.Leaderboard;
using GuessCode.API.Models.V1.User;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Auth.Models;
using GuessCode.Domain.Models;
using Newtonsoft.Json;

namespace GuessCode.API.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<TokenDto, Token>()
            .ReverseMap();
        CreateMap<UserDto, User>()
            .ReverseMap();

        CreateMap<Kata, KataDto>()
            .ForMember(dest => dest.KataJsonContent, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.KataRawJsonContent)
                    ? new KataJsonContent()
                    : JsonConvert.DeserializeObject<KataJsonContent>(src.KataRawJsonContent) ?? new KataJsonContent()))
            .AfterMap((src, dest) =>
            {
                dest.KataJsonContent.AnswerOptions =
                    dest.KataJsonContent.GetPublicAnswerOptions();
            });

        CreateMap<KataDto, Kata>()
            .ForMember(dest => dest.KataRawJsonContent, opt => opt.MapFrom(src =>
                JsonConvert.SerializeObject(src.KataJsonContent)));

        
        CreateMap<KataCodeReadingAnswerDto, KataCodeReadingAnswer>()
            .ReverseMap();
        CreateMap<KataCodeReadingSolveResultDto, KataCodeReadingSolveResult>()
            .ReverseMap();
        CreateMap<KataBugFindingAnswerDto, KataBugFindingAnswer>()
            .ReverseMap();
        CreateMap<KataBugFindingSolveResultDto, KataBugFindingSolveResult>()
            .ReverseMap();
        CreateMap<ProfileInfoDto, ProfileInfo>()
            .ReverseMap();
        CreateMap<LeaderboardPositionDto, LeaderboardPosition>()
            .ReverseMap();
    }
}
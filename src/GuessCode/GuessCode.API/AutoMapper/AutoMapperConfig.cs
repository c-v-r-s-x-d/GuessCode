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
        CreateMap<KataDto, Kata>()
            .ForMember(kata => kata.KataRawJsonContent,
                expression => expression.MapFrom(dto => JsonConvert.SerializeObject(dto.KataJsonContent)));
        CreateMap<KataJsonContent, KataJsonContent>() // Maps nested properties
            .ForMember(dest => dest.AnswerOptions, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.AnswerOptionsRawJson)
                    ? new List<AnswerOption>()
                    : JsonConvert.DeserializeObject<List<AnswerOption>>(src.AnswerOptionsRawJson)!));
        CreateMap<Kata, KataDto>();
        CreateMap<KataAnswerDto, KataAnswer>()
            .ReverseMap();
        CreateMap<KataSolveResultDto, KataSolveResult>()
            .ReverseMap();
        CreateMap<ProfileInfoDto, ProfileInfo>()
            .ReverseMap();
        CreateMap<LeaderboardPositionDto, LeaderboardPosition>()
            .ReverseMap();
    }
}
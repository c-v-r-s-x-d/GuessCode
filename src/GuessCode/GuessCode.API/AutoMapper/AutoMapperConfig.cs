using AutoMapper;
using GuessCode.API.AutoMapper.Resolvers;
using GuessCode.API.Models.V1.Auth;
using GuessCode.API.Models.V1.Kata;
using GuessCode.API.Models.V1.Leaderboard;
using GuessCode.API.Models.V1.User;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Auth.Models;
using GuessCode.Domain.Models;

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
                options =>
                {
                    options.MapFrom<KataRawJsonContentResolver>();
                })
            .ForMember(kata => kata.KataJsonContent, options => options.Ignore());
        CreateMap<Kata, KataDto>()
            .ForMember(dto => dto.KataJsonContent,
                options => options.MapFrom<KataRawJsonContentResolver>())
            .ForPath(dto => dto.KataJsonContent.AnswerOptionsRawJson, options => options.Ignore());
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
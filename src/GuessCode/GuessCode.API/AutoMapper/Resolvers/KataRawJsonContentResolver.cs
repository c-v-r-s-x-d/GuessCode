using AutoMapper;
using GuessCode.API.Models.V1.Kata;
using GuessCode.DAL.Models.KataAggregate;
using Newtonsoft.Json;

namespace GuessCode.API.AutoMapper.Resolvers;

public class KataRawJsonContentResolver : IValueResolver<KataDto, Kata, string>
{
    public string Resolve(KataDto source, Kata destination, string destMember, ResolutionContext context)
    {
        source.KataJsonContent.AnswerOptionsRawJson = 
            JsonConvert.SerializeObject(source.KataJsonContent.AnswerOptions);
        return JsonConvert.SerializeObject(source.KataJsonContent);
    }
}
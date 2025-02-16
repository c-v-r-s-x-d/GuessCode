using AutoMapper;
using GuessCode.API.Models.V1.Kata;
using GuessCode.DAL.Models.KataAggregate;
using Newtonsoft.Json;

namespace GuessCode.API.AutoMapper.Resolvers;

/*public class KataRawJsonContentResolver : IValueResolver<KataDto, Kata, string>, IValueResolver<Kata, KataDto, KataJsonContent>
{
    /*public string Resolve(KataDto source, Kata destination, string destMember, ResolutionContext context)
    {
        source.KataJsonContent.AnswerOptionsRawJson = 
            JsonConvert.SerializeObject(source.KataJsonContent.AnswerOptions);
        return JsonConvert.SerializeObject(source.KataJsonContent);
    }

    public KataJsonContent Resolve(Kata source, KataDto destination, KataJsonContent destMember, ResolutionContext context)
    {
        if (source.KataRawJsonContent is null)
        {
            return new KataJsonContent();
        }
        
        var kataJsonContent = JsonConvert.DeserializeObject<KataJsonContent>(source.KataRawJsonContent);
        kataJsonContent!.AnswerOptions = kataJsonContent.AnswerOptionsRawJson is null
            ? []
            : JsonConvert.DeserializeObject<List<AnswerOption>>(kataJsonContent.AnswerOptionsRawJson);
        return kataJsonContent;
    }#1#
}*/
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;

namespace GuessCode.Domain.Contracts;

public interface IKataSearchService
{
    Task<Kata> GetKataById(long kataId, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<Kata>> Search(KataDifficulty? kataDifficulty, ProgrammingLanguage? kataLanguage,
        KataType? kataType, CancellationToken cancellationToken);
}
using GuessCode.DAL.Models.Enums;

namespace GuessCode.Domain.Utils;

public static class RankUtils
{
    public static Rank CheckRank(long currentRating)
    {
        return currentRating switch
        {
            < 100 => Rank.FifthKyu,
            < 200 => Rank.FourthKyu,
            < 300 => Rank.ThirdKyu,
            < 400 => Rank.SecondKyu,
            < 500 => Rank.FirstKyu,
            < 600 => Rank.FirstDan,
            < 700 => Rank.SecondDan,
            >= 700 => Rank.Master
        };
    }
}
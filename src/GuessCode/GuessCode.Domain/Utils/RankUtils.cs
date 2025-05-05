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
            < 1000 => Rank.SecondDan,
            >= 1000 => Rank.Master
        };
    }

    public static int GetKataDifficultyRewardPoints(KataDifficulty difficulty)
    {
        return difficulty switch
        {
            KataDifficulty.FifthKyu => 10,
            KataDifficulty.FourthKyu => 30,
            KataDifficulty.ThirdKyu => 45,
            KataDifficulty.SecondKyu => 60,
            KataDifficulty.FirstKyu => 80,
            KataDifficulty.FirstDan => 100,
            KataDifficulty.SecondDan => 120,
            KataDifficulty.Master => 150,
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
        };
    }
}
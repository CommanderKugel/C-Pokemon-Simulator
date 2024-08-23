
public static class Stats
{

    public const int HP = 0,
                    Atk = 1,
                    Def = 2,
                    SpA = 3,
                    SpD = 4,
                   Init = 5;


    public static int[] calculate_stats(
        int lvl, int[] BaseStats, byte[] EVs,
        Nature nat
    ) {
        return new int[] {
            CalcHP(lvl, BaseStats[HP], EVs[HP]),
            CalcStat(Natures.GetNatMult(Atk, nat), lvl, BaseStats[Atk], EVs[Atk]),
            CalcStat(Natures.GetNatMult(Def, nat), lvl, BaseStats[Def], EVs[Def]),
            CalcStat(Natures.GetNatMult(SpA, nat), lvl, BaseStats[SpA], EVs[SpA]),
            CalcStat(Natures.GetNatMult(SpD, nat), lvl, BaseStats[SpD], EVs[SpD]),
            CalcStat(Natures.GetNatMult(Init, nat), lvl, BaseStats[Init], EVs[Init]),
        };
    }

    public static int CalcHP(int lvl, int BaseStat, int EV) 
    {
        const int IV = 31;
        return (BaseStat * 2 + IV + EV / 4) * lvl / 100 + lvl + 10;
    }

    public static int CalcStat(float NatMult, int lvl, int BaseStat, int EV) {
        const int IV = 31;

        return (int)(((2 * BaseStat + IV + EV / 4) * lvl / 100 + 5) * NatMult);
    }

    public static float GetStatChangeMult (int x) => StatChangeMult[x + 6];
    private static readonly float[] StatChangeMult = {
        0.25f, 0.28f, 0.33f, 0.4f, 0.5f, 0.66f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f,
    };

}

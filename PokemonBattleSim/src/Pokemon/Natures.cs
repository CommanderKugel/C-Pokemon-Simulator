using static Stats;


public enum Nature
{
    Hardy,
    Bold,
    Modest,
    Calm,
    Timid,
    Lonely,
    Docile,
    Mild,
    Gentle,
    Hasty,
    Adamant,
    Impish,
    Bashful,
    Careful,
    Jolly,
    Naughty,
    Lax,
    Rash,
    Quirky,
    Naive,
    Brave,
    Relaxed,
    Quiet,
    Sassy,
    Serious,
};


public static class Natures
{

    public static float GetNatMult (int Stat, Nature nat) 
    {
        if (Stat == HP) throw new Exception("HP doenst have a Nature Multiplier!");

        return NatureMultiplier[(int)nat, Stat - 1];
    }

    private static readonly float[,] NatureMultiplier = {
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        { 0.9f, 1.1f, 1.0f, 1.0f, 1.0f },
        { 0.9f, 1.0f, 1.1f, 1.0f, 1.0f },
        { 0.9f, 1.0f, 1.0f, 1.1f, 1.0f },
        { 0.9f, 1.0f, 1.0f, 1.0f, 1.1f },
        { 1.1f, 0.9f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 0.9f, 1.1f, 1.0f, 1.0f },
        { 1.0f, 0.9f, 1.0f, 1.1f, 1.0f },
        { 1.0f, 0.9f, 1.0f, 1.0f, 1.1f },
        { 1.1f, 1.0f, 0.9f, 1.0f, 1.0f },
        { 1.0f, 1.1f, 0.9f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 0.9f, 1.1f, 1.0f },
        { 1.0f, 1.0f, 0.9f, 1.0f, 1.1f },
        { 1.1f, 1.0f, 1.0f, 0.9f, 1.0f },
        { 1.0f, 1.1f, 1.0f, 0.9f, 1.0f },
        { 1.0f, 1.0f, 1.1f, 0.9f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 0.9f, 1.1f },
        { 1.1f, 1.0f, 1.0f, 1.0f, 0.9f },
        { 1.0f, 1.1f, 1.0f, 1.0f, 0.9f },
        { 1.0f, 1.0f, 1.1f, 1.0f, 0.9f },
        { 1.0f, 1.0f, 1.0f, 1.1f, 0.9f },
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
    };
    
}
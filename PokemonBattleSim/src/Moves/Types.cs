
public enum PType
{
    Normal,     // 0
    Fire,       // 1
    Water,      // 2
    Grass,      // 3
    Electric,   // 4

    Ice,        // 5
    Fighting,   // 6
    Poison,     // 7
    Ground,     // 8
    Flying,     // 9

    Psychic,    // 10
    Bug,        // 11
    Rock,       // 12
    Ghost,      // 13
    Dragon,     // 14
    
    Dark,       // 15
    Steel,      // 16
    Fairy,      // 17

    NONE,       // 18
}

public static class Types
{

    /// <summary>
    /// Uses the Types of the attacking Move and the defending Pokemon
    /// returns Damage multiplier for the Attack on that Pokemon
    /// </summary>
    public static float AttackEffecticityMultiplier(Move move, Pokemon pokemon) {
        float primary = effectivity[(int)move.Type, (int)pokemon.PrimType];
        float secondary = effectivity[(int)move.Type, (int)pokemon.SecType];
        return primary * secondary;
    }

    private static readonly float[,] effectivity = {
       // Norm. Grass Fire  Water Elec. Figh. Flyi. Pois. Grou. Rock  Bug   Ice   Ghost Drag. Psy.  Dark  Steel Fairy None
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f }, // Normal
        { 1.0f, 0.5f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f }, // Grass
        { 1.0f, 2.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f }, // Fire
        { 1.0f, 0.5f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f }, // Water
        { 1.0f, 1.0f, 2.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f }, // Electric

        { 1.0f, 0.5f, 0.5f, 0.5f, 2.0f, 0.5f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f }, // Fighting
        { 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 0.5f, 0.5f, 2.0f, 0.5f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f }, // Flying
        { 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f }, // Poison
        { 1.0f, 2.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f }, // Ground
        { 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f }, // Rock

        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f }, // Bug
        { 1.0f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f }, // Ice
        { 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }, // Ghost
        { 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f }, // Psychic
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f }, // Dragon

        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f }, // Dark
        { 1.0f, 0.5f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f }, // Steel
        { 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f }, // Fairy

        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }, // None
    };
    
    
}

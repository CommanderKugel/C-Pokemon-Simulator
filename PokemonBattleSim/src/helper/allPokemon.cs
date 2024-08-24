using static allMoves;
using static allSpecies;

public static class allPokemon
{
    public static Pokemon MyGarchomp = new("Chompa", Garchomp, [0, 252, 0, 0, 0, 252 ], Nature.Adamant, 50, [Earthquake, Dragonclaw, Swordsdance]);
    public static Pokemon MyHeatran = new("Train", Heatran, [252, 0, 0, 252, 0, 0, 0], Nature.Modest, 50, [Flamethrower]);
    public static Pokemon MyCloister = new("Oi-ster", Cloyster, [252, 0, 0, 252, 0, 0], Nature.Modest, 50, [Hydropump]);
    public static Pokemon MyGengar = new("Gengars-Khan", Gengar, [0, 0, 0, 252, 0, 252], Nature.Modest, 50, [Shadowball]);
}

using static allMoves;
using static allSpecies;

public static class allPokemon
{
    public static Pokemon MyGarchomp = new("Chompa", Garchomp, new byte[] { 0, 252, 0, 0, 0, 252 }, Nature.Adamant, 50, new Move[] { Earthquake, Dragonclaw, Swordsdance });
    public static Pokemon MyHeatran = new("Train", Heatran, new byte[] { 252, 0, 0, 252, 0, 0, 0 }, Nature.Modest, 50, new Move[] { Flamethrower});

}
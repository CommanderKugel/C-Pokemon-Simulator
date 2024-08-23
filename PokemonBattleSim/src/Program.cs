using static Stats;
using static allPokemon;
using static allMoves;

Pokemon[] TeamA = new Pokemon[] { MyGarchomp };
Pokemon[] TeamB = new Pokemon[] { MyHeatran };
Battle b = new Battle(TeamA, TeamB);

b.MakeTurn(Swordsdance, Flamethrower);
b.MakeTurn(Earthquake, Flamethrower);

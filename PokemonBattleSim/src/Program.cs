using static Stats;
using static Helper;

Pokemon[] TeamA = new Pokemon[] { MyGarchomp };
Pokemon[] TeamB = new Pokemon[] { MyHeatran };

Battle b = new Battle(TeamA, TeamB);

b.MakeTurn(Dragonclaw, Flamethrower);
b.MakeTurn(Dragonclaw, Flamethrower);

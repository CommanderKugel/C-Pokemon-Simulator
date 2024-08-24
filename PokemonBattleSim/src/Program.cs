using static Stats;
using static allPokemon;
using static allMoves;

Pokemon[] TeamA = [MyGarchomp, MyCloister];
Pokemon[] TeamB = [MyHeatran, MyGengar];
Battle b = new Battle(TeamA, TeamB);

var cloyCond = b.CurrPos.allConditions[0][1];

b.CurrPos.print();

b.MakeTurn(Earthquake, Flamethrower);

b.goBackTurn();

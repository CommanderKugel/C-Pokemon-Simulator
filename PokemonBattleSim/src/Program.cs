using static Stats;
using static allPokemon;
using static allMoves;
using System.Diagnostics;

Pokemon[] TeamA = [MyGarchomp, MyCloister];
Pokemon[] TeamB = [MyHeatran, MyGengar];
Battle b = new Battle(TeamA, TeamB);

var cloyCond = b.CurrPos.allConditions[0][1];

b.CurrPos.print();

var clock = new Stopwatch();
clock.Start();

for (int i=0; i<10_000_000; i++)
{
    b.MakeTurn(Dragonclaw, Flamethrower);
    b.goBackTurn();
}

clock.Stop();
float nps = 10_000_000 / clock.ElapsedMilliseconds * 1000;
Console.WriteLine($"nps: {nps}");
Console.WriteLine($"time: {clock.Elapsed}");

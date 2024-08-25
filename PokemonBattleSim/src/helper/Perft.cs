using static allPokemon;
using System.Diagnostics;

public static class Perft
{
    public static void goPerft(Trainer myTrainer, int numGames=100_000)
    {
        Pokemon[] TeamA = [MyCloister, MyZapdos, MyGarchomp];
        Pokemon[] TeamB = [MyHeatran, MyGengar, MySnorlax];
        Battle parent = new Battle(TeamA, TeamB, myTrainer, myTrainer);

        long nodes = 0;
        int resSum = 0;
        var watch = new Stopwatch();
        watch.Start();
        for (int i=0; i<numGames; i++)
        {
            Battle b = new(parent);

            while (!b.CurrPos.isGameOver())
            {
                var actA = myTrainer.chooseAction(b, 0);
                var actB = myTrainer.chooseAction(b, 1);
                b.MakeTurn(actA, actB);
            }
            resSum += b.CurrPos.getGameResult();
            nodes += b.nodeCount;
        }
        watch.Stop();

        Console.WriteLine($"games played: {numGames}");
        Console.WriteLine($"total turns played: {nodes}");
        Console.WriteLine($"avrg. turns per game: {nodes/numGames}");
        Console.WriteLine($"avrg. winner: {(float)resSum / (float)numGames}");
        Console.WriteLine($"nps: {nodes * 1000 / watch.ElapsedMilliseconds}");
        Console.WriteLine($"time in s: {watch.ElapsedMilliseconds / 1000}");
    }
}
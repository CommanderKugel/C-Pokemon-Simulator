using static allPokemon;
using static allMoves;
using System.Diagnostics;
using static Stats;

public static class Perft
{
    public static void goPerft()
    {
        Pokemon[] TeamA = [MyGarchomp, MyCloister, MyZapdos];
        Pokemon[] TeamB = [MyHeatran, MyGengar, MySnorlax];
        Battle b = new Battle(TeamA, TeamB);

        var clock = new Stopwatch();
        int resSum = 0;
        int numGames = 1_000_000;

        clock.Start();
        for (int i=0; i<numGames; i++)
            resSum += doRandomRollout(b);
        clock.Stop();

        Console.WriteLine($"games played: {numGames}");
        Console.WriteLine($"total turns played: {b.nodeCount}");
        Console.WriteLine($"avrg. turns per game: {(float)b.nodeCount/(float)numGames}");
        Console.WriteLine($"avrg winner: {(float)resSum / (float)numGames}");

        float nps = (float)b.nodeCount / (float)clock.ElapsedMilliseconds * 1000;
        Console.WriteLine($"nps: {nps}");
        Console.WriteLine($"time in s: {clock.ElapsedMilliseconds / 1000}");
    }


    private static int doRandomRollout (Battle b) 
    {
        var randB = new Battle(b);
        return rollout(randB);
    }

    private static int rollout (Battle b, int depth=Battle.MAX_PLY)
    {   
        if (depth <= 0)
            return 0;

        Pos pos = b.CurrPos;

        if (pos.isGameOver())
            return pos.getGameResult();

        // assumed that
        Action actA = b.trainers[0].chooseAction(b, 0);
        Action actB = b.trainers[1].chooseAction(b, 1);

        b.MakeTurn(actA, actB);
        int randRes = rollout(b, depth-1);
        b.goBackTurn();

        return randRes;
    }

}
using static allPokemon;
using static allMoves;
using System.Diagnostics;

public static class Perft
{
    public static void goPerft()
    {
        Pokemon[] TeamA = [MyGarchomp, MyCloister, MyZapdos];
        Pokemon[] TeamB = [MyHeatran, MyGengar, MySnorlax];
        // not specifying trainers creates two randomTrainers
        Battle b = new Battle(TeamA, TeamB);

        var clock = new Stopwatch();
        int resSum = 0;
        int numGames = 1_000_000;
        clock.Start();

        for (int i=0; i<numGames; i++)
            resSum += randomRollout(b);
        
        clock.Stop();

        Console.WriteLine($"games played: {numGames}");
        Console.WriteLine($"total turns played: {b.nodeCount}");
        Console.WriteLine($"avrg. turns per game: {b.nodeCount/numGames}");
        Console.WriteLine($"avrg winner: {(float)resSum / (float)numGames}");

        float nps = b.nodeCount / clock.ElapsedMilliseconds * 1000;
        Console.WriteLine($"nps: {nps}");
        Console.WriteLine($"time in s: {clock.ElapsedMilliseconds / 1000}");
    }

    private static int randomRollout (Battle b)
    {
        Pos pos = b.CurrPos;
        if (pos.isGameOver())
            return pos.getGameresult();

        Action actA = b.trainers[0].chooseAction(b, 0);
        Action actB = b.trainers[1].chooseAction(b, 1);

        b.MakeTurn(actA, actB);
        int randRes = randomRollout(b);
        b.goBackTurn();

        return randRes;
    }

}
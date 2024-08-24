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
        // not specifying trainers creates two randomTrainers
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
        Console.WriteLine($"avrg. turns per game: {b.nodeCount/numGames}");
        Console.WriteLine($"avrg winner: {(float)resSum / (float)numGames}");

        float nps = b.nodeCount / clock.ElapsedMilliseconds * 1000;
        Console.WriteLine($"nps: {nps}");
        Console.WriteLine($"time in s: {clock.ElapsedMilliseconds / 1000}");
    }


    public static int doRandomRollout (Battle b)
    {
        Battle bCopy = new Battle(b.Teams[0], b.Teams[1], new RandomTrainer(), new RandomTrainer());
        return randomRollout(bCopy, 500);
    }

    private static int randomRollout (Battle b, int depth)
    {   
        if (depth <= 0)
            return 0;

        Pos pos = b.CurrPos;
        if (pos.isGameOver())
            return pos.getGameresult();

        Action actA = b.trainers[0].chooseAction(b, 0);
        Action actB = b.trainers[1].chooseAction(b, 1);

        b.MakeTurn(actA, actB);
        int randRes = randomRollout(b, depth-1);
        b.goBackTurn();

        return randRes;
    }

    public static int debugRollout (Battle b)
    {
        Pos pos = b.CurrPos;

        if (pos.isGameOver())
        {
            int endRes = pos.getGameresult();
            Console.WriteLine($"Game Over! winner is: {endRes}");
            return endRes;
        }

        Console.WriteLine($"\nTurn {b.ply}");

        var pokA = pos.getActivePokeCond(0);
        var pokB = pos.getActivePokeCond(1);
        Action actA = b.trainers[0].chooseAction(b, 0);
        Action actB = b.trainers[1].chooseAction(b, 1);
        

        if (b.goesFirst(actA.priority, actB.priority, pokA.StatsEffective[Init], pokB.StatsEffective[Init]))
        {
            if (actA.isMove) Console.WriteLine($"{pokA.Nickname} uses {(actA as Move).name}");
            else Console.WriteLine($"{pokA.Nickname} switches out for {(actA as Switch).bankedMon.Nickname}");

            if (actB.isMove) Console.WriteLine($"{pokB.Nickname} uses {(actB as Move).name}");
            else Console.WriteLine($"{pokB.Nickname} switches out for {(actB as Switch).bankedMon.Nickname}");
        }
        else
        {
            if (actB.isMove) Console.WriteLine($"{pokB.Nickname} uses {(actB as Move).name}");
            else Console.WriteLine($"{pokB.Nickname} switches out for {(actB as Switch).bankedMon.Nickname}");

            if (actA.isMove) Console.WriteLine($"{pokA.Nickname} uses {(actA as Move).name}");
            else Console.WriteLine($"{pokA.Nickname} switches out for {(actA as Switch).bankedMon.Nickname}");
        }
        

        b.MakeTurn(actA, actB);
        int res = debugRollout(b);
        b.goBackTurn();

        return res;
    }

}
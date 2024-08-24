using static allPokemon;
using static allMoves;
using System.Diagnostics;

public static class Perft
{
    public static void goPerft()
    {
        Pokemon[] TeamA = [MyGarchomp, MyCloister, MyZapdos];
        Pokemon[] TeamB = [MyHeatran, MyGengar, MySnorlax];
        Battle b = new Battle(TeamA, TeamB);

        var clock = new Stopwatch();
        clock.Start();
        for (int i=0; i<10_000_000; i++)
        {
            var actA = getRandomAction(0, b.CurrPos);
            var actB = getRandomAction(1, b.CurrPos);
            b.MakeTurn(actA, actB);
            b.goBackTurn();
        }
        clock.Stop();

        float nps = b.nodeCount / clock.ElapsedMilliseconds * 1000;
        Console.WriteLine($"nps: {nps}");
        Console.WriteLine($"time: {clock.Elapsed}");
    }

    private static Action getRandomAction(int Team, Pos pos)
    {
        var moves = pos.getActivePokemon(Team).Moveset;
        var switches = pos.getAllSwitches(Team);
        
        int index = Helper.rng.Next(moves.Length + switches.Length);
        return index < moves.Length ? moves[index] : switches[index - moves.Length];
    }

}
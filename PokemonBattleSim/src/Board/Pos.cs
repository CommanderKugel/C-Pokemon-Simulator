using static Stats;

public struct Pos
{
    public Battle battle;
    public PokeCond[][] allConditions;

    public PokeCond getActivePokemon(int Team) => allConditions[Team][0];

    public Switch[] getAllSwitches(int Team)
    {
        List<Switch> allSwitches = new List<Switch>();
        foreach (var pc in this.allConditions[Team])
            if (!pc.isFainted)
                allSwitches.Add(new Switch(pc, Team));
        
        return allSwitches.ToArray();
    }

    public Pos(Battle b)
    {
        this.battle = b;
        allConditions = [
            b.Teams[0].Select(x => new PokeCond(x)).ToArray(),
            b.Teams[1].Select(x => new PokeCond(x)).ToArray(),
        ];
        // set index for all conditions
        for (byte i=0; i<allConditions[0].Length; i++)
        {
            allConditions[0][i].index = i;
            allConditions[1][i].index = i;
        }
    }

    public Pos(Pos prev)
    {
        this.battle = prev.battle;
        int l = prev.allConditions[0].Length;
        
        allConditions = [
            new PokeCond[l], 
            new PokeCond[l]
        ];

        for (int i=0; i<l; i++)
        {
            allConditions[0][i] = new(prev.allConditions[0][i]);
            allConditions[1][i] = new(prev.allConditions[1][i]);
        }
    }

    public Pos() {}
    public static Pos emptyPos => new Pos();

    public void print()
    {
        var pA = getActivePokemon(0);
        var pB = getActivePokemon(1);
        Console.WriteLine($"active A: {pA.Nickname}, B: {pB.Nickname}");
        Console.WriteLine($"\tHP A: {pA.StatsEffective[HP]}, B: {pB.StatsEffective[HP]}");
    }
}

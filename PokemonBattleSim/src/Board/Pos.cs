using static Stats;

public struct Pos
{
    public Battle battle;
    public PokeCond[][] allConditions;

    public PokeCond getActivePokemon(int Team) => allConditions[Team][0];

    public Pos(Battle b)
    {
        this.battle = b;
        allConditions = [
            b.Teams[0].Select(x => new PokeCond(x)).ToArray(),
            b.Teams[1].Select(x => new PokeCond(x)).ToArray(),
        ];
    }

    public Pos(Pos prev)
    {
        this.battle = prev.battle;
        allConditions = prev.allConditions.Select(
            arr => arr.Select(x => new PokeCond(x)).ToArray()
        ).ToArray();
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

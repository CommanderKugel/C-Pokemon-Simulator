public struct Pos
{
    public Battle battle;
    public PokeCond[][] allConditions;
    public int[] activeMonIndex;

    public ref PokeCond getActivePokemon(int Team) => ref allConditions[Team][activeMonIndex[Team]];

    public Pos(Battle b)
    {
        this.battle = b;
        allConditions = new PokeCond[][] {
            b.Teams[0].Select(x => new PokeCond(x)).ToArray(),
            b.Teams[1].Select(x => new PokeCond(x)).ToArray(),
        };
        this.activeMonIndex = new int[] { 0, 0 };
    }

    public Pos(Pos prev)
    {
        this.battle = prev.battle;
        allConditions = prev.allConditions.Select(
            arr => arr.Select(x => new PokeCond(x)).ToArray()
        ).ToArray();
        this.activeMonIndex = prev.activeMonIndex.ToArray();
    }
}

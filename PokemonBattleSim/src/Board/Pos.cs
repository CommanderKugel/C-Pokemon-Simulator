public struct Pos
{
    public Battle battle;
    public PokeCond[][] allConditions;

    public PokeCond getActivePokemon(int Team) => allConditions[Team][0];

    public Pos(Battle b)
    {
        this.battle = b;
        allConditions = new PokeCond[][] {
            b.Teams[0].Select(x => new PokeCond(x)).ToArray(),
            b.Teams[1].Select(x => new PokeCond(x)).ToArray(),
        };
    }

    public Pos(Pos prev)
    {
        this.battle = prev.battle;
        allConditions = prev.allConditions.Select(
            arr => arr.Select(x => new PokeCond(x)).ToArray()
        ).ToArray();
    }
}

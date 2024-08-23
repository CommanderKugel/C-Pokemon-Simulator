using static Stats;

public struct PokeCond
{
    public readonly Pokemon pokemon;
    public int[] StatsEffective;
    public sbyte[] StatChanges;

    public PokeCond(Pokemon p)
    {
        pokemon = p;
        StatsEffective = p.stats.ToArray();
        StatChanges = new sbyte[6];
    }

    public PokeCond(PokeCond pc) 
    {
        pokemon = pc.pokemon;
        StatsEffective = pc.StatsEffective.ToArray();
        StatChanges = pc.StatChanges.ToArray();
    } 

    // QUALITY OF LIFE METHODS
    public string Nickname => this.pokemon.NickName;
    public Species Species => this.pokemon.Species;
    public int Level => this.pokemon.Level;
    public PType PrimType => this.pokemon.PrimType;
    public PType SecType => this.pokemon.SecType;
    public int[] stats => this.pokemon.stats;
    public Move[] Moveset => this.pokemon.MoveSet;

    // QUALITY IF LIFE METHODS THAT ARE NOT PLAIN REFERENCES
    public bool isFainted => StatsEffective[HP] <= 0;
    public void dealDamage(int dmg) => this.StatsEffective[HP] = Math.Max(this.StatsEffective[HP] - dmg, 0);

    public bool moveIsLearned(Move move) => this.pokemon.MoveSet.Contains(move);
    public bool canUseMove(Move move) => this.moveIsLearned(move); // Choice Items Here


    public void CalculateStatsEffective() 
    {
        for (int i=Atk; i<=Init; i++)
        {
            float mult = GetStatChangeMult(StatChanges[i]);
            StatsEffective[i] = (int)(pokemon.stats[i] * mult);
        }
    }

    public void ChangeStats(int stat, int c) => StatChanges[stat] = (sbyte) Math.Clamp(StatChanges[stat] + c, -6, 6);
}

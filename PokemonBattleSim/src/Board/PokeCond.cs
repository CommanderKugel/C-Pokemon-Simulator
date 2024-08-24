using static Stats;

public class PokeCond
{
    public readonly Pokemon pokemon;
    public short[] StatsEffective;
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
        StatsEffective = new short[6];
        StatChanges = new sbyte[6];
        Array.Copy(pc.StatsEffective, this.StatsEffective, 6);
        Array.Copy(pc.StatChanges, this.StatChanges, 6);
    } 

    // QUALITY OF LIFE METHODS
    public string Nickname => this.pokemon.NickName;
    public Species Species => this.pokemon.Species;
    public int Level => this.pokemon.Level;
    public PType PrimType => this.pokemon.PrimType;
    public PType SecType => this.pokemon.SecType;
    public short[] stats => this.pokemon.stats;
    public Move[] Moveset => this.pokemon.MoveSet;

    // QUALITY IF LIFE METHODS THAT ARE NOT PLAIN REFERENCES
    public bool isFainted => StatsEffective[HP] <= 0;
    public void dealDamage(int dmg) => this.StatsEffective[HP] = (short) Math.Max(this.StatsEffective[HP] - dmg, 0);

    public bool moveIsLearned(Move move) => this.pokemon.MoveSet.Contains(move);
    public bool canUseMove(Move move) => this.moveIsLearned(move); // Choice Items Here
    public bool canMove() => false; // paralysis-, confusion-, flinchcheck here


    public void CalculateStatsEffective() 
    {
        for (int i=Atk; i<=Init; i++)
        {
            float mult = GetStatChangeMult(StatChanges[i]);
            StatsEffective[i] = (short)(pokemon.stats[i] * mult);
        }
    }

    public void ChangeStats(int stat, int c) => StatChanges[stat] = (sbyte)Math.Clamp(StatChanges[stat]+c, -6, 6);
}

using static Stats;

public class Pokemon
{
    public readonly string NickName;
    public readonly Species Species;

    public readonly int Level;
    public readonly PType PrimType;
    public readonly PType SecType;

    public readonly Nature nat;
    public readonly short[] stats;
    public readonly byte[] EVs;

    public readonly Move[] MoveSet;


    public Pokemon(
        string NickName,
        Species Species, byte[] EVs, Nature nat,
        int Level,
        Move[] MoveSet
    ) {
        this.NickName = NickName;

        this.Level = Level;
        this.PrimType = Species.PrimType;
        this.SecType = Species.SecType;

        this.nat = nat;
        this.EVs = EVs;
        this.stats = calculate_stats(Level, Species.BaseStats, EVs, nat);

        this.MoveSet = MoveSet;
    }

    public void printStats() 
    {
        string[] s = { "HP", "Atk", "Def", "SpA", "SpD", "Init" };
        for (int i=HP; i<=Init; i++)
            Console.WriteLine($"{s[i]}: {this.stats[i]}");
    }

}

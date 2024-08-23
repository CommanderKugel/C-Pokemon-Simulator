
public class Species
{
    public readonly int[] BaseStats;

    public readonly PType PrimType;
    public readonly PType SecType;

    public Species(int[] BaseStats, PType PrimType, PType SecType = PType.NONE)
    {
        this.BaseStats = BaseStats;
        this.PrimType = PrimType;
        this.SecType = SecType;
    }
}

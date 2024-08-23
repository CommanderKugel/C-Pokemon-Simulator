using static Stats;

public static class Helper
{
    public static Random rng = new Random();

    public static Species Garchomp = new(new[] { 108, 130, 95, 80, 85, 102 }, PType.Dragon, PType.Ground);
    public static Species Heatran = new(new[] { 91, 90, 106, 130, 106, 66 }, PType.Fire, PType.Steel);

    public static Move Earthquake = new("Earthquake", Category.Physical, PType.Ground, 100);
    public static Move Flamethrower = new("Flamethrower", Category.Special, PType.Fire, 90);
    public static Move Dragonclaw = new("Dragonclaw", Category.Physical, PType.Dragon, 80);

    public static Pokemon MyGarchomp = new("Chompa", Garchomp, new byte[] { 0, 252, 0, 0, 0, 252 }, Nature.Adamant, 50, new Move[] { Earthquake, Dragonclaw });
    public static Pokemon MyHeatran = new("Train", Heatran, new byte[] { 252, 0, 0, 252, 0, 0, 0 }, Nature.Modest, 50, new Move[] { Flamethrower});
}

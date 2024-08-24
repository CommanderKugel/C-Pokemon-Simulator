using static OnHitEffects;

public static class allMoves
{
    // Easy Moves
    public static Move Earthquake = new("Earthquake", Category.Physical, PType.Ground, 100);
    public static Move Dragonclaw = new("Dragonclaw", Category.Physical, PType.Dragon, 80);
    public static Move Hydropump = new("Hydro Pump", Category.Special, PType.Water, 120, Accuracy: 80);

    // ToDo Moves
    public static Move Flamethrower = new("Flamethrower", Category.Special, PType.Fire, 90);
    public static Move Thunderbolt = new("Thunderbolt", Category.Special, PType.Electric, 90);
    public static Move Bodyslam = new("Bodyslyam", Category.Physical, PType.Normal, 85);

    // Stat-Lowering Moves
    public static Move Shadowball = new("Shadow Ball", Category.Special, PType.Ghost, 80, OnHitEffAct: lowerDefendersSpD, OnHitChance: 20);

    // Status Moves
    public static Move Swordsdance = new("Swords Dance", Category.Status, PType.Normal, OnHitEffAct: raiseAttackTwice);
}

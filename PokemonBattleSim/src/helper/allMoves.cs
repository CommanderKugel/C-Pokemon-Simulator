public static class allMoves
{
    // Easy Moves
    public static Move Earthquake = new("Earthquake", Category.Physical, PType.Ground, 100);
    public static Move Dragonclaw = new("Dragonclaw", Category.Physical, PType.Dragon, 80);
    public static Move Hydropump = new("Hydro Pump", Category.Special, PType.Water, 120, Accuracy: 80);

    // ToDo Moves
    public static Move Flamethrower = new("Flamethrower", Category.Special, PType.Fire, 90);
    public static Move Shadowball = new("Shadow Ball", Category.Special, PType.Ghost, 80);

    // Status Moves
    public static Move Swordsdance = new("Swords Dance", Category.Status, PType.Normal, OnHitEffAct: OnHitEffects.raiseAttackTwice);
}

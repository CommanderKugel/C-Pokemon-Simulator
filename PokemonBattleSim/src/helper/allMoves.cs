public static class allMoves
{
    public static Move Earthquake = new("Earthquake", Category.Physical, PType.Ground, 100);
    public static Move Flamethrower = new("Flamethrower", Category.Special, PType.Fire, 90);
    public static Move Dragonclaw = new("Dragonclaw", Category.Physical, PType.Dragon, 80);
    public static Move Swordsdance = new("Swords Dance", Category.Status, PType.Normal, OnHitEffAct: OnHitEffects.raiseAttackTwice);
}

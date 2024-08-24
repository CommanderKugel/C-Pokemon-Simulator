using static OnHitEffects;
using static Stats;

public static class allMoves
{
    // Easy Moves
    public static Move Earthquake = new("Earthquake", Category.Physical, PType.Ground, 100);
    public static Move Dragonclaw = new("Dragonclaw", Category.Physical, PType.Dragon, 80);
    public static Move Hydropump = new("Hydro Pump", Category.Special, PType.Water, 120, Accuracy: 80);

    private static int EruptionDmgCalc(Move move, PokeCond attacker, PokeCond defender) => attacker.StatsEffective[HP] / attacker.stats[HP] * DamageCalc.CalculateRawDamage(move, attacker, defender);
    public static Move Eruption = new("Eruption", Category.Special, PType.Fire, 250, CalcDmgFunc: EruptionDmgCalc);

    // ToDo Moves
    public static Move Flamethrower = new("Flamethrower", Category.Special, PType.Fire, 90);
    public static Move Thunderbolt = new("Thunderbolt", Category.Special, PType.Electric, 90);
    public static Move Bodyslam = new("Bodyslyam", Category.Physical, PType.Normal, 85);
    public static Move StoneEdge = new("Stone Edge", Category.Physical, PType.Rock, 100, Accuracy: 80);

    // Stat-Lowering Moves
    public static Move Shadowball = new("Shadow Ball", Category.Special, PType.Ghost, 80, OnHitEffAct: lowerDefendersSpD, OnHitChance: 20);

    // Status Moves
    public static Move Swordsdance = new("Swords Dance", Category.Status, PType.Normal, OnHitEffAct: raiseAttackTwice);
    public static Move Roost = new("Roost", Category.Status, PType.Flying, OnHitEffAct: healBy50);
}

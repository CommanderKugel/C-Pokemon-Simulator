using static Stats;

public static class OnHitEffects
{

    public static void NoEffect(PokeCond attacker, PokeCond defender, int OnHitChance) {}

    public static void raiseAttackTwice(PokeCond attacker, PokeCond defender, int OnHitChance) => attacker.ChangeStats(Atk, 2);

    public static void lowerAttackersDefSpD(PokeCond attacker, PokeCond defender, int OnHitChance) { attacker.ChangeStats(Def, -1); attacker.ChangeStats(SpD, -1); }

    public static void lowerDefendersSpD(PokeCond attacker, PokeCond defender, int OnHitChance) { if(Helper.getRandomRoll(OnHitChance)) defender.ChangeStats(SpD, -1); }
}

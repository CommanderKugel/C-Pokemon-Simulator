using static Stats;

public static class OnHitEffects
{

    public static void NoEffect(PokeCond attacker, PokeCond defender) {}

    public static void raiseAttackTwice(PokeCond attacker, PokeCond defender) => attacker.ChangeStats(Atk, 2);

    public static void lowerAttackersDefSpD(PokeCond attacker, PokeCond defender) { attacker.ChangeStats(Def, -1); attacker.ChangeStats(SpD, -1); }
}

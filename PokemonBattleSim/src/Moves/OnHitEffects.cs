using static Stats;

public static class OnHitEffects
{

    public static void NoEffect(PokeCond attacker, PokeCond defender) {}

    public static void riseAttackTwice(PokeCond attacker, PokeCond defender) => attacker.ChangeStats(Atk, 2);
}

using static Stats;

public static class OnHitEffects
{

    public static void NoEffect(PokeCond attacker, PokeCond defender) {}

    public static void raiseAttackTwice(PokeCond attacker, PokeCond defender) => attacker.ChangeStats(Atk, 2);
    public static void lowerAttackersDefSpD(PokeCond attacker, PokeCond defender) { attacker.ChangeStats(Def, -1); attacker.ChangeStats(SpD, -1); }
    public static void lowerDefendersSpD(PokeCond attacker, PokeCond defender) => defender.ChangeStats(SpD, -1);

    public static void healBy50(PokeCond attacker, PokeCond defender) => Math.Min(attacker.StatsEffective[HP] += (short)(attacker.pokemon.stats[HP] * 0.5), attacker.stats[HP]);

    public static void flinch(PokeCond attacker, PokeCond defender) => defender.makeFlinch();
}

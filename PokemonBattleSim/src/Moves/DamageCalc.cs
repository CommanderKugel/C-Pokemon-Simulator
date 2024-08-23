using static Stats;

public static class DamageCalc
{
    public static int CalculateRawDamage (Move move, PokeCond attacker, PokeCond defender)
    {
        // https://bulbapedia.bulbagarden.net/wiki/Damage

        // update Stats after changes
        // eg. defender moves first and uses Close Combat, that drops its defences
        attacker.CalculateStatsEffective();
        defender.CalculateStatsEffective();
        
        float AoD = move.Category == Category.Physical ? (float)attacker.StatsEffective[Atk] / (float)defender.StatsEffective[Def] 
                                                       : (float)attacker.StatsEffective[SpA] / (float)defender.StatsEffective[SpD];

        float Type = Types.AttackEffecticityMultiplier(move, defender.pokemon);

        float STAB = (attacker.pokemon.PrimType == move.Type || attacker.pokemon.SecType == move.Type) 
                   ?  1.5f : 1.0f;

        int dmg = (int)((2 * attacker.pokemon.Level / 5 + 2) * move.Power * AoD / 50 + 2);
        dmg = (int)(dmg * STAB);
        dmg = (int)(dmg * Type);
        
        // Spreadmove Penalty -> 0.75 if more than 1 Target
        // Weather -> 0.5 or 1.5 respectively
        // Critical
        // Random -> RandInt(85, 100) / 100
        // Burn
        // Other - inlcudes Item, Rollout bonus, etc.
        // !!! Item !!!
        
        return dmg;
    }

    public static float getRandomDamagemult => (float)Helper.rng.Next(85, 101) / 100f;
}

using static Stats;

public class Battle
{
    public Pokemon[][] Teams;
    public Pos[] Positions;

    const int MAX_PLY = 64;
    public int ply;
    public ref Pos CurrPos => ref Positions[ply];

    public Battle(Pokemon[] TeamA, Pokemon[] TeamB)
    {
        Teams = new Pokemon[][] { TeamA, TeamB };
        Positions = new Pos[64];
        Positions[0] = new(this);
        ply = 0;
    }


    public void MakeSingleMove (
        Move move, PokeCond attacker, PokeCond defender,
        bool useRandomRanges=true,
        bool useCritRoll=true,
        bool canMiss=true
    ) {
        if (attacker.isFainted || !attacker.canUseMove(move))
        {
            Console.WriteLine($"{attacker.Nickname} can not use {move.name}!");
            return;
        }

        Console.WriteLine($"{attacker.Nickname} used {move.name}!");

        if (canMiss && move.Accuracy < 100 && Helper.rng.Next(0, 100) > move.Accuracy)
        {
            Console.WriteLine("it missed!");
            return;
        }

        if (move.Category != Category.Status)
        {
            // damaging part
            int dmg = move.CalcDmg(attacker, defender);
            if (useRandomRanges) dmg = (int)(dmg * DamageCalc.getRandomRollvalue);
            if (useCritRoll) dmg = (int)(dmg * DamageCalc.getCritRollValue);
            
            defender.dealDamage(dmg);
            Console.WriteLine($"{defender.Nickname} took {(int)((float)dmg / (float)defender.stats[HP] * 100)}% damage");

            // faint check
            if (defender.isFainted) 
            {
                Console.WriteLine($"{defender.Nickname} fainted!");
                return;
            }
        }
        
        // damaging moves and status moves can both have effects
        move.OnHitEffect(attacker, defender);
        
    }

    

    public void MakeTurn(Move moveA, Move moveB)
    {
        ref Pos pos = ref CurrPos;
        PokeCond pokeA = pos.getActivePokemon(0);
        PokeCond pokeB = pos.getActivePokemon(1);

        Console.WriteLine($"\nTurn {ply}");
        
        // determine move order
        bool aGoesFirst = goesFirst(moveA.priority, moveB.priority, pokeA.StatsEffective[Init], pokeB.StatsEffective[Init]);

        // make moves in order
        if (aGoesFirst)
        {
            MakeSingleMove(moveA, pokeA, pokeB);
            MakeSingleMove(moveB, pokeB, pokeA);
        } 
        else 
        {
            MakeSingleMove(moveB, pokeB, pokeA);
            MakeSingleMove(moveA, pokeA, pokeB);
        }

        pokeA.CalculateStatsEffective();
        pokeB.CalculateStatsEffective();

        // update boardstate
        this.ply++;
        Pos newPos = new Pos(pos);
        Positions[this.ply] = newPos;

        Console.WriteLine($"{pokeA.Nickname} HP: {pokeA.StatsEffective[HP]}, {pokeB.Nickname} HP: {pokeB.StatsEffective[HP]}");
    }

    public bool goesFirst (int prioA, int prioB, int initA, int initB)
    {
        if (prioA != prioB) return prioA > prioB;
        if (initA != initB) return initA > initB;
        return Helper.rng.Next(0, 2) == 0;
    }
}

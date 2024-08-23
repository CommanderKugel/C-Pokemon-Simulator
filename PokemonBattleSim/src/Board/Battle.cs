using static Stats;

public class Battle
{
    public Pokemon[][] Teams;
    public Pos[] Positions;

    const int MAX_PLY = 64;
    public int ply;
    public Pos CurrPos => Positions[ply];

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


    public void MakeSwitch(Switch s, PokeCond active)
    {
        var arr = CurrPos.allConditions[s.Team];
        int index = Array.IndexOf(arr, s.bankedMon);
        (arr[index], arr[0]) = (arr[0], arr[index]);

        // update to remove certain effects
        // e.g. choice-moves, confusion, ...
    }

    public void TakeAction (Action a, PokeCond attacker, PokeCond defender) 
    {
        if (a is Move) MakeSingleMove(a as Move, attacker, defender);
        else MakeSwitch(a as Switch, attacker);
    }    

    public void MakeTurn(Action actA, Action actB)
    {
        Pos pos = CurrPos;
        PokeCond pokeA = pos.getActivePokemon(0);
        PokeCond pokeB = pos.getActivePokemon(1);

        Console.WriteLine($"\nTurn {ply}");
        
        // determine move order
        bool aGoesFirst = goesFirst(actA.priority, actB.priority, pokeA.StatsEffective[Init], pokeB.StatsEffective[Init]);

        // make moves in order
        if (aGoesFirst)
        {
            TakeAction(actA, pokeA, pokeB);
            TakeAction(actB, pokeB, pokeA);
        } 
        else 
        {
            TakeAction(actB, pokeB, pokeA);
            TakeAction(actA, pokeA, pokeB);
        }

        // update if last Action changed Stats
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

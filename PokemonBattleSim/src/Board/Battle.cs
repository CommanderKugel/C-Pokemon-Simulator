using static Stats;

public class Battle
{
    public Pokemon[][] Teams;
    public Pos[] Positions;

    const int MAX_PLY = 64;
    public int ply;
    public int nodeCount;
    public Pos CurrPos => this.Positions[ply];

    public Battle(Pokemon[] TeamA, Pokemon[] TeamB)
    {
        Teams = [TeamA, TeamB];
        Positions = new Pos[64];
        Positions[0] = new(this);
        ply = 0;
        nodeCount = 0;
    }


    public bool faintCheckAll(int Team)
    {
        foreach (var pc in CurrPos.allConditions[Team])
            if (!pc.isFainted) return false;
        return true;
    }


    public void MakeMove(
        Move move, PokeCond attacker, PokeCond defender,
        bool useRandomRanges=true,
        bool useCritRoll=true,
        bool canMiss=true
    ) {
        
        // Faint check
        if (attacker.isFainted) return;

        // check if making the move is even allowed
        if (!attacker.canUseMove(move))
            return;

        //Console.WriteLine($"{attacker.Nickname} used {move.name}!");

        // try missing the move
        if (canMiss && move.Accuracy < 100 && Helper.rng.Next(0, 100) > move.Accuracy)
            return;

        // deal the damage if it deals any
        if (move.Category != Category.Status)
        {
            // damaging part
            int dmg = move.CalcDmg(attacker, defender);
            if (useRandomRanges) dmg = (int)(dmg * DamageCalc.getRandomRollvalue);
            if (useCritRoll) dmg = (int)(dmg * DamageCalc.getCritRollValue);
            
            defender.dealDamage(dmg);

            // faint check
            if (defender.isFainted) 
                return;

        }
        
        // damaging moves and status moves can both have effects
        move.OnHitEffect(attacker, defender);
        
    }


    public void MakeSwitch(Switch s)
    {
        if (s.bankedMon == this.CurrPos.getActivePokemon(s.Team))
            return;

        var arr = CurrPos.allConditions[s.Team];
        var bankIndex = s.bankedMon.index;

        (arr[bankIndex], arr[0]) = (arr[0], arr[bankIndex]);
        arr[0].index = 0;
        arr[bankIndex].index = bankIndex;

        // update to remove certain effects
        // e.g. choice-moves, confusion, ...
        Array.Fill<sbyte>(arr[bankIndex].StatChanges, 0);
    }



    public void TakeAction (Action a, PokeCond attacker, PokeCond defender) 
    {
        if (a is Move) MakeMove(a as Move, attacker, defender);
        else MakeSwitch(a as Switch);
    }    

    public void MakeTurn(PokeCond p, Action actB) => MakeTurn(new Switch(p, 0), actB);
    public void MakeTurn(Action actA, PokeCond p) => MakeTurn(actA, new Switch(p, 1));
    public void MakeTurn(PokeCond pA, PokeCond pB) => MakeTurn(new Switch(pA, 0), new Switch(pB, 1));

    public void MakeTurn(Action actA, Action actB)
    {
        if (this.ply >= MAX_PLY) 
        {
            //Console.WriteLine("Max ply from root reached!");
            throw null;
        }

        // first copy, then make
        Pos pos = new Pos(CurrPos);
        this.ply++;
        this.nodeCount++;
        Positions[this.ply] = pos;

        PokeCond pokeA = pos.getActivePokemon(0);
        PokeCond pokeB = pos.getActivePokemon(1);
        
        // determine move order
        bool aGoesFirst = goesFirst(actA.priority, actB.priority, pokeA.StatsEffective[Init], pokeB.StatsEffective[Init]);

        // make moves in order
        if (aGoesFirst)
        {
            TakeAction(actA, pokeA, pokeB);
            pokeA = pos.getActivePokemon(0);

            if (pokeB == pos.getActivePokemon(1))
                TakeAction(actB, pokeB, pokeA);
            else
                pokeB = pos.getActivePokemon(1);
        } 
        else 
        {
            TakeAction(actB, pokeB, pokeA);
            pokeB = pos.getActivePokemon(1);

            if (pokeA == pos.getActivePokemon(0))
                TakeAction(actA, pokeA, pokeB);
            else
                pokeA = pos.getActivePokemon(0);
        }

        // update if last Action changed Stats
        pokeA.CalculateStatsEffective();
        pokeB.CalculateStatsEffective();

        // Faint-check & switch ins
        if (pokeA.isFainted) 
        {
            if (faintCheckAll(0))
                return;

            MakeSwitch(new(pos.allConditions[0][1], 0));
        }
        if (pokeB.isFainted) 
        {
            if (faintCheckAll(1))
                return;

            MakeSwitch(new(pos.allConditions[1][1], 1));
        }
    }

    public bool goesFirst (int prioA, int prioB, int initA, int initB)
    {
        if (prioA != prioB) return prioA > prioB;
        if (initA != initB) return initA > initB;
        return Helper.rng.Next(0, 2) == 0;
    }

    public void goBackTurn() 
    {
        this.Positions[this.ply] = Pos.emptyPos;
        this.ply--;
    }
}

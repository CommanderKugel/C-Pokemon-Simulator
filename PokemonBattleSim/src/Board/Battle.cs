using static Stats;

public class Battle
{
    public readonly Trainer[] trainers;
    public readonly Pokemon[][] Teams;
    public Pos[] Positions;

    public const int MAX_PLY = 500;
    public int ply;
    public int nodeCount;
    public Pos CurrPos => this.Positions[ply];


    public Battle(Pokemon[] TeamA, Pokemon[] TeamB, Trainer t1=null, Trainer t2=null)
    {
        trainers = [
            t1 == null ? new RandomTrainer() : t1, 
            t2 == null ? new RandomTrainer() : t2
        ];
        Teams = [TeamA, TeamB];
        Positions = new Pos[MAX_PLY];
        Positions[0] = new(this);
        ply = 0;
        nodeCount = 0;
    }

    /* METHODS USED TO COPY A BATTLE AND PASS IT TO THE BOT */
    /* AND AFTERWARDS UPDATE IT TO THE "ORIGINAL" BATTLE    */
    public Battle(Battle parent)
    {
        this.trainers = parent.trainers;
        this.Teams = parent.Teams;
        Positions = new Pos[MAX_PLY];
        this.Positions[0] = new(this);
        this.ply = 0;
        this.nodeCount = 0;
    }
    public void copyFromParent(Battle parent)
    {
        this.Positions[0] = parent.CurrPos;
        this.ply = 0;
    }


    // HELPER METHODS 

    public bool faintCheckAll(int Team)
    {
        foreach (var pc in CurrPos.allConditions[Team])
            if (!pc.isFainted) return false;
        return true;
    }


    // MAKE- UNMAKE ACTIONS

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

        // try missing the move
        if (canMiss && move.misses)
            return;

        // deal the damage if the move deals any
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
        if (s.bankedMon == this.CurrPos.getActivePokeCond(s.Team))
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


    public void MakeTurn(PokeCond pA, Action actB) => MakeTurn(new Switch(pA, 0), actB);
    public void MakeTurn(Action actA, PokeCond pB) => MakeTurn(actA, new Switch(pB, 1));
    public void MakeTurn(PokeCond pA, PokeCond pB) => MakeTurn(new Switch(pA, 0), new Switch(pB, 1));
    public void MakeTurn(Action actA, Action actB)
    {
        if (this.ply >= MAX_PLY-1) 
            return;

        // first copy, then make
        Pos pos = new Pos(CurrPos);
        this.ply++;
        this.nodeCount++;
        Positions[this.ply] = pos;

        PokeCond pokeA = pos.getActivePokeCond(0);
        PokeCond pokeB = pos.getActivePokeCond(1);
        
        // determine move order
        bool aGoesFirst = goesFirst(actA.priority, actB.priority, pokeA.StatsEffective[Init], pokeB.StatsEffective[Init]);

        // make moves in order
        if (aGoesFirst)
        {
            TakeAction(actA, pokeA, pokeB);
            pokeA = pos.getActivePokeCond(0);

            if (pokeB == pos.getActivePokeCond(1))
                TakeAction(actB, pokeB, pokeA);
            else
                pokeB = pos.getActivePokeCond(1);
        } 
        else 
        {
            TakeAction(actB, pokeB, pokeA);
            pokeB = pos.getActivePokeCond(1);

            if (pokeA == pos.getActivePokeCond(0))
                TakeAction(actA, pokeA, pokeB);
            else
                pokeA = pos.getActivePokeCond(0);
        }

        // Faint-check & switch ins if fainted
        if (pokeA.isFainted) 
        {
            if (faintCheckAll(0))
                return;

            Switch[] switches = pos.getAllSwitches(0);
            Switch s = switches[Helper.rng.Next(switches.Length)];
            MakeSwitch(s);
        }
        if (pokeB.isFainted) 
        {
            if (faintCheckAll(1))
                return;

            Switch[] switches = pos.getAllSwitches(1);
            Switch s = switches[Helper.rng.Next(switches.Length)];
            MakeSwitch(s);
        }

        // update if last Action changed Stats
        // or switched pokemon changes Stats, e.g. via Intimidate
        pokeA.CalculateStatsEffective();
        pokeB.CalculateStatsEffective();
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


    // HELPER METHOD FOR DEBUGGING
    public void MakeTurnAndWrite(Action actA, Action actB)
    {
        if (this.ply >= MAX_PLY-1)
        {
            Console.WriteLine("tried going further that 500 moves!");
            return;
        }

        // first copy, then make
        Pos pos = new Pos(CurrPos);
        this.ply++;
        this.nodeCount++;
        Positions[this.ply] = pos;

        Console.WriteLine($"\nTurn {this.ply}");

        PokeCond pokeA = pos.getActivePokeCond(0);
        PokeCond pokeB = pos.getActivePokeCond(1);
        
        // determine move order
        bool aGoesFirst = goesFirst(actA.priority, actB.priority, pokeA.StatsEffective[Init], pokeB.StatsEffective[Init]);

        // make moves in order
        if (aGoesFirst)
        {
            if (actA is Move) Console.WriteLine($"{pokeA} used {actA as Move}!");
            else Console.WriteLine($"{pokeA} switched to {(actA as Switch).bankedMon}!");
            TakeActionAndWrite(actA, pokeA, pokeB);
            pokeA = pos.getActivePokeCond(0);

            if (pokeB == pos.getActivePokeCond(1) && !pokeB.isFainted)
            {
                if (actB is Move) Console.WriteLine($"{pokeB} used {actB as Move}!");
                else Console.WriteLine($"{pokeB} switched to {(actB as Switch).bankedMon}!");
                TakeActionAndWrite(actB, pokeB, pokeA);
            }
            else
                pokeB = pos.getActivePokeCond(1);
        } 
        else 
        {
            if (actB is Move) Console.WriteLine($"{pokeB} used {actB as Move}!");
            else Console.WriteLine($"{pokeB} switched to {(actB as Switch).bankedMon}!");
            TakeActionAndWrite(actB, pokeB, pokeA);
            pokeB = pos.getActivePokeCond(1);

            if (pokeA == pos.getActivePokeCond(0) && !pokeB.isFainted)
            {
                if (actA is Move) Console.WriteLine($"{pokeA} used {actA as Move}!");
                else Console.WriteLine($"{pokeA} switched to {(actA as Switch).bankedMon}!");
                TakeActionAndWrite(actA, pokeA, pokeB);
            }
            else
                pokeA = pos.getActivePokeCond(0);
        }

        // Faint-check & switch ins if fainted
        if (pokeA.isFainted) 
        {
            if (faintCheckAll(0))
                return;

            Switch[] switches = pos.getAllSwitches(0);
            Switch s = switches[Helper.rng.Next(switches.Length)];
            MakeSwitch(s);
            Console.WriteLine($"Go, {s.bankedMon}!");
        }
        if (pokeB.isFainted) 
        {
            if (faintCheckAll(1))
                return;

            Switch[] switches = pos.getAllSwitches(1);
            Switch s = switches[Helper.rng.Next(switches.Length)];
            MakeSwitch(s);
            Console.WriteLine($"Go, {s.bankedMon}!");
        }

        // update if last Action changed Stats
        // or switched pokemon changes Stats, e.g. via Intimidate
        pokeA.CalculateStatsEffective();
        pokeB.CalculateStatsEffective();
    }


    public void MakeMoveAndWrite (
        Move move, PokeCond attacker, PokeCond defender,
        bool useRandomRanges=true,
        bool useCritRoll=true,
        bool canMiss=true
    ) {
        
        // Faint check
        if (attacker.isFainted) return;

        // check if making the move is even allowed
        if (!attacker.canUseMove(move))
        {
            Console.WriteLine($"{attacker} cant use {move}!");
            return;
        }

        // try missing the move
        if (canMiss && move.misses)
        {
            Console.WriteLine("It missed!");
            return;
        }

        // deal the damage if the move deals any
        if (move.Category != Category.Status)
        {
            // damaging part
            int dmg = move.CalcDmg(attacker, defender);
            if (useRandomRanges) dmg = (int)(dmg * DamageCalc.getRandomRollvalue);
            if (useCritRoll && Helper.rng.Next(16) == 0)
            {
                Console.WriteLine("A critical hit!");
                dmg = (int)(dmg * DamageCalc.getCritRollValue);
            }
            
            defender.dealDamage(dmg);
            Console.WriteLine($"It dealt {dmg} damage!");

            // faint check
            if (defender.isFainted) 
            {
                Console.WriteLine($"{defender} fainted!");
                return;
            }

        }
        
        // damaging moves and status moves can both have effects
        move.OnHitEffect(attacker, defender);
    }

    public void TakeActionAndWrite (Action a, PokeCond attacker, PokeCond defender) 
    {
        if (a is Move) MakeMoveAndWrite(a as Move, attacker, defender);
        else MakeSwitch(a as Switch);
    }   

}

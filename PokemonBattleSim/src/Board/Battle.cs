using static Stats;

public class Battle
{
    public Trainer[] trainers;
    public Pokemon[][] Teams;
    public Pos[] Positions;

    public const int MAX_PLY = 64;
    public int ply;
    public int nodeCount;
    public Pos CurrPos => this.Positions[ply];

    public Battle(Pokemon[] TeamA, Pokemon[] TeamB, Trainer t1=null, Trainer t2=null)
    {
        trainers = [t1 is null ? new RandomTrainer() : t1, t2 is null ? new RandomTrainer() : t2];
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

    public void MakeTurn(PokeCond p, Action actB) => MakeTurn(new Switch(p, 0), actB);
    public void MakeTurn(Action actA, PokeCond p) => MakeTurn(actA, new Switch(p, 1));
    public void MakeTurn(PokeCond pA, PokeCond pB) => MakeTurn(new Switch(pA, 0), new Switch(pB, 1));

    public void MakeTurn(Action actA, Action actB)
    {
        if (this.ply >= MAX_PLY-1) 
        {
            throw new Exception("Game took too long and exceeded the Ply-limit!");
        }

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
            MakeSwitch(switches[Helper.rng.Next(switches.Length)]);
        }
        if (pokeB.isFainted) 
        {
            if (faintCheckAll(1))
                return;

            Switch[] switches = pos.getAllSwitches(1);
            MakeSwitch(switches[Helper.rng.Next(switches.Length)]);
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
}

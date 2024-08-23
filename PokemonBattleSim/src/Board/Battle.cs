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

    public void MakeSingleMove(Move move, PokeCond attacker, PokeCond defender,
                               bool useRandomRanges=true)
    {
        if (attacker.isFainted || !attacker.canUseMove(move))
        {
            Console.WriteLine($"{attacker.Nickname} used Splash!");
            return;
        }

        Console.WriteLine($"{attacker.Nickname} used {move.name}!");
        int dmg = move.CalcDmg(attacker, defender);

        if (useRandomRanges) dmg = (int)(dmg * DamageCalc.getRandomDamagemult);

        defender.dealDamage(dmg);
        Console.WriteLine($"{defender.Nickname} took {(int)((float)dmg / (float)defender.stats[HP] * 100)}% damage");

        if (defender.isFainted)
            Console.WriteLine($"{defender.Nickname} fainted!");
    }

    public void MakeTurn(Move moveA, Move moveB)
    {
        ref Pos pos = ref CurrPos;
        PokeCond pokeA = pos.getActivePokemon(0);
        PokeCond pokeB = pos.getActivePokemon(1);

        Console.WriteLine($"\nTurn {ply}");
        
        // determine move order
        var (firstMove, firstMon, secondMove, secondMon) = orderTurn(moveA, pokeA, moveB, pokeB);

        // make moves in order
        MakeSingleMove(firstMove, firstMon, secondMon);
        if (!secondMon.isFainted)
            MakeSingleMove(secondMove, secondMon, firstMon);

        // update boardstate
        this.ply++;
        Pos newPos = new Pos(pos);
        Positions[this.ply] = newPos;

        Console.WriteLine($"{pokeA.Nickname} HP: {pokeA.StatsEffective[HP]}, {pokeB.Nickname} HP: {pokeB.StatsEffective[HP]}");
    }

    public (Move, PokeCond, Move, PokeCond) orderTurn(Move moveA, PokeCond pokeA, Move moveB, PokeCond pokeB)
    {
        if (moveA.priority != moveB.priority)
        {
            return (moveA.priority > moveB.priority)
                   ? (moveA, pokeA, moveB, pokeB)
                   : (moveB, pokeB, moveA, pokeA);
        }
        else if (pokeA.StatsEffective[Init] == pokeB.StatsEffective[Init])
        {
            return (Helper.rng.Next(2) == 0)
                   ? (moveA, pokeA, moveB, pokeB)
                   : (moveB, pokeB, moveA, pokeA);
        }
        else // prio is equal, init is not equal
        {
            return (pokeA.StatsEffective[Init] > pokeB.StatsEffective[Init])
                   ? (moveA, pokeA, moveB, pokeB)
                   : (moveB, pokeB, moveA, pokeA);
        }
    }
}

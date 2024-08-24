public class MostDmgTrainer : Trainer
{
    public override Action chooseAction(Battle b, int us)
    {
        var pos = b.CurrPos;
        var ourMon = pos.getActivePokeCond(us);
        var theirMon = pos.getActivePokeCond(us ^ 1);

        return getMostDmgMove(ourMon, theirMon);
    }

    private Move getMostDmgMove(PokeCond attacker, PokeCond defender)
    {
        Move mostDmgMove = attacker.Moveset[0];
        int mostDmg = 0;

        foreach (var move in attacker.Moveset)
        {
            int dmg = move.CalcDmg(attacker, defender);
            if (dmg > mostDmg)
            {
                mostDmg = dmg;
                mostDmgMove = move;
            }
        }

        return mostDmgMove;
    }

    public override Switch chooseSwitch(Battle b, int us)
    {
        var arr = b.CurrPos.getAllSwitches(us);
        var defender = b.CurrPos.getActivePokeCond(us ^ us);

        PokeCond mostDmgMon = arr[0].bankedMon;
        int mostDmg = 0;
        foreach (var s in arr)
        {
            PokeCond pc = s.bankedMon;
            Move mostDmgMove = getMostDmgMove(pc, defender);
            int dmg = mostDmgMove.CalcDmg(pc, defender);
            if (dmg > mostDmg)
            {
                mostDmg = dmg;
                mostDmgMon = pc;
            }
        }

        return new Switch(mostDmgMon, us);
    }
}
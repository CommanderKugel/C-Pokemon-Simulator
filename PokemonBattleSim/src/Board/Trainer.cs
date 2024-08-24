using static Stats;

public abstract class Trainer
{
    public abstract Action chooseAction(Battle b, int us);
    public abstract Switch chooseSwitch(Battle b, int us);
}

public class RandomTrainer : Trainer
{
    public override Action chooseAction(Battle b, int us)
    {
        var pos = b.CurrPos;
        var moves = pos.getActivePokeCond(us).Moveset;
        var switches = pos.getAllSwitches(us);
        
        int index = Helper.rng.Next(moves.Length + switches.Length);
        return index < moves.Length ? moves[index] : switches[index - moves.Length];
    }

    public override Switch chooseSwitch(Battle b, int us)
    {
        var arr = b.CurrPos.getAllSwitches(us);
        return arr[Helper.rng.Next(arr.Length)];
    }
}


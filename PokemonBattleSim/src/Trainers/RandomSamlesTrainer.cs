public class RandomSamlesTrainer : Trainer
{
    public override Action chooseAction(Battle b, int us)
    {

        int bestScore = -1000;
        Action bestAction = default;

        var ourActions = getAllActions(b, us);
        var theirActions = getAllActions(b, us ^ 1);

        foreach (Action actA in ourActions)
        {
            int score = 0;

            foreach (Action actB in theirActions)
            {
                b.MakeTurn(actA, actB);

                for (int i=0; i<10; i++) 
                    score += Perft.doRandomRollout(b);

                b.goBackTurn();
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestAction = actA;
            }
        }

        return bestAction;
    }

    private Action[] getAllActions(Battle b, int Team)
    {
        var mon = b.CurrPos.getActivePokeCond(Team);
        var moves = mon.Moveset;
        var switches = b.CurrPos.getAllSwitches(Team);

        Action[] allActions = new Action[moves.Length + switches.Length];

        for (int i=0; i<moves.Length; i++)
            allActions[i] = moves[i];

        for (int j=0; j<switches.Length; j++)
            allActions[j+moves.Length] = switches[j];
        
        return allActions;
    }

    public override Switch chooseSwitch(Battle b, int us)
    {
        throw new NotImplementedException();
    } 
}
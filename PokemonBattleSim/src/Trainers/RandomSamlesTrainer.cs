public class RandomSamlesTrainer : Trainer
{
    Random rng = new Random();

    public override Action chooseAction(Battle b, int us)
    {
        var pos = b.CurrPos;
        var ourActions   = getAllActions(pos, us);
        var theirActions = getAllActions(pos, us ^ 1);

        int bestScore = -1000;
        Action bestAction = null;

        foreach (Action actA in ourActions)
        {
            int score = 0;

            foreach (Action actB in theirActions)
            {
                b.MakeTurn(actA, actB);
                for (int i=0; i<100; i++) 
                    score += randomRollout(new Battle(b));
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

    private Action[] getAllActions(Pos p, int us) => [.. p.getActivePokeCond(us).Moveset, .. p.getAllSwitches(us)];

    private int randomRollout (Battle b)
    {
        Pos p = b.CurrPos;
        if (p.isGameOver())
            return p.getGameResult();

        var allActionsA = getAllActions(p, 0);
        var allActionsB = getAllActions(p, 1);
        var actionA = allActionsA[rng.Next(allActionsA.Length)];
        var actionB = allActionsB[rng.Next(allActionsB.Length)];
        
        b.MakeTurn(actionA, actionB);
        int sample = randomRollout(b);
        b.goBackTurn();

        return sample;
    }

    public override Switch chooseSwitch(Battle b, int us)
    {
        var switches = b.CurrPos.getAllSwitches(us);
        return switches[rng.Next(switches.Length)];
    } 
}

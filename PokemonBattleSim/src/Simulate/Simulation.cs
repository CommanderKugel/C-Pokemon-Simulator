public static class Simulation
{
    public static void Simulate(Pokemon[] teamA, Pokemon[] teamB, Trainer trainerA, Trainer trainerB)
    {

        Battle parentBattle = new Battle(teamA, teamB, trainerA, trainerB);
        Battle copyBattleA = new Battle(parentBattle);
        Battle copyBattleB = new Battle(parentBattle);

        while (!parentBattle.CurrPos.isGameOver())
        {
            Action actA = trainerA.chooseAction(copyBattleA, 0);
            Action actB = trainerB.chooseAction(copyBattleB, 1);

            parentBattle.MakeTurnAndWrite(actA, actB);

            copyBattleA.copyFromParent(parentBattle);
            copyBattleB.copyFromParent(parentBattle);
        }

        int result = parentBattle.CurrPos.getGameresult();
        Console.WriteLine(result);

    }
}

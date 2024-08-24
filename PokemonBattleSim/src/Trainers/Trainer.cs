public abstract class Trainer
{
    public abstract Action chooseAction(Battle b, int us);
    public abstract Switch chooseSwitch(Battle b, int us);
}

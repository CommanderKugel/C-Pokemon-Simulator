public abstract class Action
{
    public bool isMove;
    public int priority;

    public Action(bool isMove, int priority) 
    {
        this.isMove = isMove;
        this.priority = priority;
    }
}
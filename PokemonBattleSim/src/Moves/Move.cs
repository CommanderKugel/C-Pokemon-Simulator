using static Stats;

public class Move
{
    public readonly string name;
    public readonly Category Category;
    public readonly PType Type;

    public readonly int Accuracy;
    public readonly int Power;
    public readonly int priority;

    private readonly Func<Move, PokeCond, PokeCond, int> CalcDmgFunc;
    public int CalcDmg(PokeCond att, PokeCond def) => this.CalcDmgFunc(this, att, def);    

    public Move (
        string name,
        Category Category, 
        PType Type, 
        int Power, 
        int Accuracy = 100, 
        Func<Move, PokeCond, PokeCond, int> CalcDmgFunc = null,
        int Priority = 0
    ) {
        this.name = name;
        this.Category = Category;
        this.Type = Type;
        this.Accuracy = Accuracy;
        this.Power = Power;
        this.CalcDmgFunc = CalcDmgFunc is null ? DamageCalc.CalculateRawDamage : CalcDmgFunc;
        this.priority = Priority;
    }

}

public enum Category 
{
    Physical,
    Special,
    Status,
}

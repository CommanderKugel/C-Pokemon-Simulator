using static Stats;

public class Move : Action
{
    public readonly string name;
    public readonly Category Category;
    public readonly PType Type;

    public readonly int Power;
    public readonly int Accuracy;

    public bool misses => Accuracy < 100 && Helper.rng.Next(0, 100) > Accuracy;

    private readonly Func<Move, PokeCond, PokeCond, int> CalcDmgFunc;
    public int CalcDmg(PokeCond att, PokeCond def) => this.CalcDmgFunc(this, att, def);

    public readonly int OnHitChance;
    private readonly Action<PokeCond, PokeCond> OnHitEffAct;
    public void OnHitEffect(PokeCond attacker, PokeCond defender) 
    { 
        if (Helper.getRandomRoll(this.OnHitChance))
            OnHitEffAct(attacker, defender);
    }
 
    public Move (
        string name,
        Category Category, 
        PType Type, 
        int Power = 0, 
        int Accuracy = 100, 
        int OnHitChance = 100,
        Func<Move, PokeCond, PokeCond, int> CalcDmgFunc = null,
        int Priority = 0,
        Action<PokeCond, PokeCond> OnHitEffAct = null
    ) 
    : base(true, Priority) 
    {
        this.name = name;
        this.Category = Category;
        this.Type = Type;
        this.Accuracy = Accuracy;
        this.OnHitChance = OnHitChance;
        this.Power = Power;
        this.CalcDmgFunc = CalcDmgFunc is null ? DamageCalc.CalculateRawDamage : CalcDmgFunc;
        this.OnHitEffAct = OnHitEffAct is null ? OnHitEffects.NoEffect : OnHitEffAct;
    }

    public override string ToString() => this.name;

}

public enum Category 
{
    Physical,
    Special,
    Status,
}

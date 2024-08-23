public class Switch : Action
{
    public PokeCond bankedMon;
    int Team;

    public Switch(PokeCond bankedMon, int Team) : base(false) 
    {
        this.bankedMon = bankedMon;
        this.Team = Team;
    }

    private void doSwitch(PokeCond banked, Pos pos)
    {
        var arr = pos.allConditions[Team];
        int bankedIndex = Array.IndexOf(arr, banked);
        (arr[bankedIndex], arr[0]) = (arr[0], arr[bankedIndex]);

        // update PokeConds here, e.g. choice Moves or Confusion
    }
}

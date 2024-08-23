using static Stats;

public static class Helper
{
    public static readonly Random rng = new Random();
    public static bool getRandomRoll(int chance) => rng.Next(0, 100) > chance;
}   

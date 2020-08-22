using System;

public class RandomChance
{
    
    public static bool TenPercentChance()
    {
        var rand = new Random();
        var result = rand.Next(1, 100);
        return (result <= 10) ;
    }
}

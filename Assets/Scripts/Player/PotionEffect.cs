
using System;

public class PotionEffect
{
    public int speed;
    public long expireIn;

    public PotionEffect(int speed, long expireIn)
    {
        this.speed = speed;
        this.expireIn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + expireIn;
    }
}
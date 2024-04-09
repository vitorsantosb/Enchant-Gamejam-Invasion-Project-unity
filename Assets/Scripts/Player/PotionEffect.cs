
using System;

public class PotionEffect
{
    public int speed;
		public int damage;
    public long expireIn;

    public PotionEffect(int speed, int damage, long expireIn)
    {
        this.speed = speed;
        this.expireIn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + expireIn;
    }
}
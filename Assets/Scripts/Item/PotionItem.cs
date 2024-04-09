using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Item/New Potion", fileName = "New Potion")]
public class PotionItem : Item
{
    public int increaseDamage;
    public int increaseSpeed;
		public int duration;
}


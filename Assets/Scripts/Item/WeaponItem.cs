using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Item/New weapon", fileName = "New Weapon")]
public class WeaponItem : Item
{
   	public float bulletSpeed = 10f;
		public float bulletLifeTime = 2f;
		public int bulletDamage = 4;
		public float bulletCooldown = 0.5f;
		public int bulletAmount = 1;
		public float bulletAcurracyAngle = 10f;
		public float bulletSpreadAngle = 10f;
}
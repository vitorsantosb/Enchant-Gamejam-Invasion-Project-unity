using UnityEngine;
using UnityEngine.Tilemaps;

public class Item : ScriptableObject
{
    public int id;
    public Sprite image;
    public ItemType type;
    public TileBase tile;
    public ActionType actionType;
    public bool stackable;
}

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

[CreateAssetMenu(menuName = "Item/New Food", fileName = "New Food")]
public class FoodItem : Item
{
    public int lifeHeal;
    public int foodHeal;
}

[CreateAssetMenu(menuName = "Item/New Potion", fileName = "New Potion")]
public class PotionItem : Item
{
    public int increaseDamage;
    public int increaseSpeed;
}

[CreateAssetMenu(menuName = "Item/New Placeable", fileName = "New Placeable")]
public class PlaceableItem : Item
{
    public int life;
}

[CreateAssetMenu(menuName = "Item/New Spaceship Material", fileName = "New Spaceship Material")]
public class ShipMaterialItem : Item
{
}

public enum ItemType
{
    Weapon,
    Food,
    Potion,
    Placeable,
    ShipMaterial
}

public enum ActionType
{
    Shoot,
    Heal,
		Eat,
		Speed,
    Place,
    Fix,
}


 
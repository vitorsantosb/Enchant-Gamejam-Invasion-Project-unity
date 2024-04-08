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
    public new ItemType type = ItemType.Weapon;
    public int firerate;
    public int damage;
    public int maxAmmo;
}

[CreateAssetMenu(menuName = "Item/New Food", fileName = "New Food")]
public class FoodItem : Item
{
    public new ItemType type = ItemType.Food;
    public int lifeHeal;
    public int foodHeal;
}

[CreateAssetMenu(menuName = "Item/New Potion", fileName = "New Potion")]
public class PotionItem : Item
{
    public new ItemType type = ItemType.Potion;
    public int increaseDamage;
    public int increaseSpeed;
}

[CreateAssetMenu(menuName = "Item/New Placeable", fileName = "New Placeable")]
public class PlaceableItem : Item
{
    public new ItemType type = ItemType.Placeable;

    public int life;
}

[CreateAssetMenu(menuName = "Item/New Spaceship Material", fileName = "New Spaceship Material")]
public class ShipMaterialItem : Item
{
    public new ItemType type = ItemType.ShipMaterial;
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
    Eat,
    Place,
    Fix,
}
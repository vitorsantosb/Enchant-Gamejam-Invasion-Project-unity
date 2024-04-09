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
    Damage,
    Place,
    Fix,
}


 
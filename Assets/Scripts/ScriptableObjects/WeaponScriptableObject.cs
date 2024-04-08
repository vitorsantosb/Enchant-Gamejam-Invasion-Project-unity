using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "weaponObject", menuName = "weapon/new weapon",order = 0)]
public class WeaponScript : ScriptableObject
{
   public int ammoCount;
   public float ammoSpeed;
   public Sprite weaponSprite;
}  

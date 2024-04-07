using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageArea : MonoBehaviour
{
	
	public Enemy Owner { get; private set; }
	public int areaDamage = 4;
	public float areaLifeTime = 4;

	// Start is called before the first frame update
	public void Start()
	{
			Destroy(gameObject, areaLifeTime);
	}

	public void OnCollisionEnter(Collision collision)
	{
			Destroy(gameObject);
	}

	// Update is called once per frame

	public void InitializeDamageArea(Enemy owner, float lifeTime, int damage)
	{
			Owner = owner;
			areaLifeTime = lifeTime;
			areaDamage = damage;
	}
}

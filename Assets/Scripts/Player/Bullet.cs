using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Player Owner { get; private set; }
	public float bulletSpeed = 10f;
	public float bulletLifeTime = 2f;
	public int bulletDamage = 4;


	public void Start()
	{
			Destroy(gameObject, bulletLifeTime);
	}

	public void OnCollisionEnter(Collision collision)
	{
			Destroy(gameObject);
	}

	public void InitializeBullet(Player owner, float speed, float lifeTime, int damage)
	{
			Owner = owner;
			bulletSpeed = speed;
			bulletLifeTime = lifeTime;
			bulletDamage = damage;

			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
			rb.velocity = direction * bulletSpeed;

			transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<Enemy>()){
			other.GetComponent<Enemy>().RemoveHealth(bulletDamage);
			Destroy(gameObject);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyObject;


// Player, Enemy, and POI needs to be tagged correctly

public class Enemy : MonoBehaviour
{
    
		public EnemyScriptableObject enemyObject;

		private int health;
		private float speed;
		private float damage;
		private EnemyType type;
		private float attackRange = 1.0f;
		private float attackRangeDistance = 5.0f;
		private float poiEyesighDistance = 5.0f;
		private bool isDead = false;

		public GameObject smallerEnemyPrefab;
		public GameObject enemyBulletPrefab;


		// enemy possible behaviors
		public enum EnemyBehavior
		{
				IDLE,
				CHASE_PLAYER,
				CHASE_POI,
				CHASE_ENEMY,
				ATTACK_PLAYER,
				ATTACK_POI,
				ATTACK_RANGED_PLAYER,
				ATTACK_RANGED_POI,
				HEAL,
				SPLIT
		}

		
		// Start is called before the first frame update
    void Start()
    {
			if (enemyObject != null)
			{
				this.health = enemyObject.enemyHealth;
				this.speed = enemyObject.enemySpeed;
				this.damage = enemyObject.enemyDamage;
				this.type = enemyObject.enemyType;
			}
			else
			{
				Debug.LogError("Enemy object is not assigned!");
			}
    }

    // Update is called once per frame
    void Update()
    {
			// get behaviors based on the enemy type
			EnemyBehavior[] behaviors = GetBehavior(type);

			// execute behaviors based on priority, first index is highest priority
			// if a behavior returns false, move to the next behavior
			if(!isDead){
				foreach (EnemyBehavior behavior in behaviors)
				{
					switch (behavior)
					{
						case EnemyBehavior.IDLE:
							if (Idle()) {
								Debug.Log("Idle executed"); 
								return;
							}else{
								Debug.Log("Idle failed");
							}
							break;
						case EnemyBehavior.CHASE_PLAYER:
							if (ChasePlayer()) {
								Debug.Log("ChasePlayer executed"); 
								return;
							}else{
								Debug.Log("ChasePlayer failed");
							}
							break;
						case EnemyBehavior.CHASE_POI:
							if (ChasePOI()) {
								Debug.Log("ChasePOI executed"); 
								return;
							}else{
								Debug.Log("ChasePOI failed");
							}
							break;
						case EnemyBehavior.CHASE_ENEMY:
							if (ChaseEnemy()) {
								Debug.Log("ChaseEnemy executed"); 
								return;
							}else{
								Debug.Log("ChaseEnemy failed");
							}
							break;
						case EnemyBehavior.ATTACK_PLAYER:
							if (AttackPlayer()) {
								Debug.Log("AttackPlayer executed"); 
								return;
							}else{
								Debug.Log("AttackPlayer failed");
							}
							break;
						case EnemyBehavior.ATTACK_POI:
							if (AttackPOI()) {
								Debug.Log("AttackPOI executed"); 
								return;
							}else{
								Debug.Log("AttackPOI failed");
							}
							break;
						case EnemyBehavior.ATTACK_RANGED_PLAYER:
							if (AttackRangedPlayer()) {
								Debug.Log("AttackRangedPlayer executed"); 
								return;
							}else{
								Debug.Log("AttackRangedPlayer failed");
							}
							break;
						case EnemyBehavior.ATTACK_RANGED_POI:
							if (AttackRangedPOI()) {
								Debug.Log("AttackRangedPOI executed"); 
								return;
							}else{
								Debug.Log("AttackRangedPOI failed");
							}
							break;
						case EnemyBehavior.HEAL:
							if (HealNearbyEnemies()) {
								Debug.Log("HealNearbyEnemies executed"); 
								return;
							}else{
								Debug.Log("HealNearbyEnemies failed");
							}
							break;
						case EnemyBehavior.SPLIT:
							if (Split()) {
								Debug.Log("Split executed"); 
								return;
							}else{
								Debug.Log("Split failed");
							}
							break;
					}
				}
			}

			// check if the enemy is dead, if so, destroy the game object and trigger death animation
			CheckAndDie();
    }


		// returns a GameObject with the nearest player
		private GameObject FindNearestPlayer()
		{
				GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
				GameObject nearestPlayer = null;
				float minDistance = Mathf.Infinity;

				foreach (GameObject player in players)
				{
						float distance = Vector3.Distance(player.transform.position, transform.position);
						if (distance < minDistance)
						{
								minDistance = distance;
								nearestPlayer = player;
						}
				}

				Debug.Log("Nearest player: " + nearestPlayer.name);

				return nearestPlayer;
		}

		// return a GameObject with the nearest Point of Interest
		private GameObject FindNearestPOI()
		{
				GameObject[] pois = GameObject.FindGameObjectsWithTag("POI");
				GameObject nearestPOI = null;
				float minDistance = Mathf.Infinity;

				foreach (GameObject poi in pois)
				{
						float distance = Vector3.Distance(poi.transform.position, transform.position);
						if (distance < minDistance)
						{
								minDistance = distance;
								nearestPOI = poi;
						}
				}

				return nearestPOI;
		}

		// get nearby enemies within a radius
		private GameObject[] FindNearbyEnemies(float radius)
		{
				GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
				List<GameObject> nearbyEnemies = new List<GameObject>();

				foreach (GameObject enemy in enemies)
				{
						float distance = Vector3.Distance(enemy.transform.position, transform.position);
						if (distance < radius)
						{
								nearbyEnemies.Add(enemy);
						}
				}

				return nearbyEnemies.ToArray();
		}


		// idle behavior, move randomly a few seconds in a direction
		private bool Idle()
		{
				Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
				transform.position = Vector3.MoveTowards(transform.position, randomDirection, speed * Time.deltaTime);
				return true;
		}

		
		// chase the nearest player
		private bool ChasePlayer()
		{
				GameObject player = FindNearestPlayer();
				if (player != null)
				{
					transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
					return true;
				}
				return false;
		}

		// chase the nearest Point of Interest
		private bool ChasePOI()
		{
				GameObject poi = FindNearestPOI();
				if (poi != null)
				{
					if (Vector3.Distance(poi.transform.position, transform.position) <= poiEyesighDistance)
					{
						transform.position = Vector3.MoveTowards(transform.position, poi.transform.position, speed * Time.deltaTime);
						return true;
					}
				}
				return false;
		}

		// chase the nearest enemy
		private bool ChaseEnemy()
		{
				GameObject[] enemies = FindNearbyEnemies(5.0f);
				if (enemies.Length > 0)
				{
					GameObject nearestEnemy = enemies[0];
					transform.position = Vector3.MoveTowards(transform.position, nearestEnemy.transform.position, speed * Time.deltaTime);
					return true;
				}
				return false;
		}

		// attack the nearest player
		private bool AttackPlayer()
		{
				GameObject player = FindNearestPlayer();
				if (player != null)
				{
					if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
					{
						// TODO: attack player
						return true;
					}
				}
				return false;
		}

		// attack the nearest Point of Interest
		private bool AttackPOI()
		{
				GameObject poi = FindNearestPOI();
				if (poi != null)
				{
					if (Vector3.Distance(poi.transform.position, transform.position) <= attackRange)
					{
						// TODO: attack poi
						return true;
					}
				}
				return false;
		}

		// attack the nearest player from a distance
		private bool AttackRangedPlayer()
		{
				GameObject player = FindNearestPlayer();
				if (player != null)
				{
					if (Vector3.Distance(player.transform.position, transform.position) <= attackRangeDistance)
					{
						// TODO: attack player
						return true;
					}
				}
				return false;
		}

		// attack the nearest Point of Interest from a distance
		private bool AttackRangedPOI()
		{
				GameObject poi = FindNearestPOI();
				if (poi != null)
				{
					if (Vector3.Distance(poi.transform.position, transform.position) <= attackRangeDistance)
					{
						// TODO: attack poi
						return true;
					}
				}
				return false;
		}

		// heal nearby enemies
		private bool HealNearbyEnemies()
		{
				GameObject[] nearbyEnemies = FindNearbyEnemies(5.0f);
				// TODO: trigger healing animation

				foreach (GameObject enemy in nearbyEnemies)
				{
					enemy.GetComponent<Enemy>().health += 10;
					// TODO: spawn healing particles
				}

				return nearbyEnemies.Length > 0; // return true if there are enemies to heal
		}

		// spawn 2 smaller enemies, position them randomly within a radius
		private bool Split()
		{
				if (type != EnemyType.SPLITTER) return false;
				
				if (smallerEnemyPrefab == null)
				{
					Debug.LogError("Smaller enemy prefab is not assigned!");
					return false;
				}

				GameObject enemy1 = Instantiate(smallerEnemyPrefab, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)), Quaternion.identity);
				GameObject enemy2 = Instantiate(smallerEnemyPrefab, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)), Quaternion.identity);

				enemy1.GetComponent<Enemy>().health = 10;
				enemy2.GetComponent<Enemy>().health = 10;

				return true;
		}

		// Kill the enemy if health is 0, trigger death animation
		private void CheckAndDie()
		{
				if (health <= 0)
				{
					isDead = true;
					// TODO: trigger death animation
					if (type == EnemyType.SPLITTER) Split();
					Destroy(gameObject);
				}
		}

		// get behaviors based on the enemy type, first index is highest priority, poi is before than player
		private EnemyBehavior[] GetBehavior(EnemyType enemyType)
		{
				switch (enemyType)
				{
					case EnemyType.NORMAL:
					case EnemyType.STRONG:
					case EnemyType.WEAK:
					case EnemyType.SPLITTER:
						return new EnemyBehavior[] {EnemyBehavior.ATTACK_POI, EnemyBehavior.ATTACK_PLAYER, EnemyBehavior.CHASE_POI, EnemyBehavior.CHASE_PLAYER, EnemyBehavior.IDLE};
					case EnemyType.RANGED:
						return new EnemyBehavior[] {EnemyBehavior.ATTACK_RANGED_POI, EnemyBehavior.ATTACK_RANGED_PLAYER, EnemyBehavior.CHASE_POI, EnemyBehavior.CHASE_PLAYER, EnemyBehavior.IDLE};
					case EnemyType.HEALER:
						return new EnemyBehavior[] {EnemyBehavior.HEAL, EnemyBehavior.CHASE_ENEMY, EnemyBehavior.ATTACK_POI, EnemyBehavior.ATTACK_PLAYER, EnemyBehavior.CHASE_POI, EnemyBehavior.CHASE_PLAYER, EnemyBehavior.IDLE};
					default:
						return new EnemyBehavior[] {EnemyBehavior.IDLE};
				}
		}

}

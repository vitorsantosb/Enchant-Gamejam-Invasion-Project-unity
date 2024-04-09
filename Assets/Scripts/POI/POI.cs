using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POI : MonoBehaviour
{
		[HideInInspector]
		public int health;
		public int maxHealth;
		public GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
      health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

		public void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.GetComponent<Enemy>()) {
	      //   Enemy enemy = other.gameObject.GetComponent<Enemy>();
	      //   RemoveHealth(enemy.damage);
	      //   Debug.Log("HIT COLISION");
        // }
				
				
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<EnemyDamageArea>()) {
	        EnemyDamageArea enemyDamageArea = other.gameObject.GetComponent<EnemyDamageArea>();
	        RemoveHealth(enemyDamageArea.areaDamage);
	        Destroy(other.gameObject);
				}
    }
    public int GetLife() => this.health;
    public void SetLife(int increment)
    {
        if (increment >= this.health)
        {
            this.health =  this.maxHealth;
        }
        else
        {
            this.health = increment;
        }
        if (this.health <= 0)
        {
            DeathController();
        }
    }   
    public void AddHealth(int lifeIncrement) => this.SetLife(this.health + lifeIncrement);
    public void RemoveHealth(int lifeReduced) => this.SetLife(this.health - lifeReduced);
    //public void UpdateLifeBar() => this.LifeBar.fillAmount = ((1.6f / this.maxHealth) * this.health);
    public void DeathController()
    {
	    GameObject defaultScreen = GameObject.FindGameObjectWithTag("MainCanvas");
	    defaultScreen.SetActive(false);
	    deathScreen.SetActive(true);
	    Destroy(this.gameObject);   
    }
}

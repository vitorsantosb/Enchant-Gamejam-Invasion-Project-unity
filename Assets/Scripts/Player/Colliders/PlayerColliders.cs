using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColliders : MonoBehaviour
{
    
		[HideInInspector]
		public int playerHealth;
    public int maxHealth = 120;


    void Start()
    {
			playerHealth = maxHealth;
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
	        Enemy enemy = other.gameObject.GetComponent<Enemy>();
	        RemoveHealth(enemy.damage);
	        Debug.Log("HIT COLISION");
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "bullet")
        {
            RemoveHealth(50);
            Debug.Log("HIT !!!!");
        }
    }
    public int GetLife() => this.playerHealth;
    public void SetLife(int increment)
    {
        if (increment >= this.playerHealth)
        {
            this.playerHealth =  this.maxHealth;
        }
        else
        {
            this.playerHealth = increment;
        }
        if (this.playerHealth <= 0)
        {
            DeathController();
        }
    }   
    public void AddHealth(int lifeIncrement) => this.SetLife(this.playerHealth + lifeIncrement);
    public void RemoveHealth(int lifeReduced) => this.SetLife(this.playerHealth - lifeReduced);
    //public void UpdateLifeBar() => this.LifeBar.fillAmount = ((1.6f / this.maxHealth) * this.playerHealth);
    public void DeathController() => Debug.Log("Player is dead");
}

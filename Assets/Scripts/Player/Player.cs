using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private float forceMultiplier = 4;
    private List<PotionEffect> effects;
    private Text potionListText;
    public PhotonView view;
    public GameObject cameraPrefab;

    private float maxHealth = 100;

    private float health;
    private int strength = 0;

    public GameObject bulletPrefab;
    private float bulletCooldownTimer = 0f;
    private bool shootContinuously = false;

    public InventoryManager inventoryManager;


    // Start is called before the first frame update
    void Start()
    {
        this.health = this.maxHealth;
        this.potionListText = GameObject.FindGameObjectWithTag("potionText").GetComponent<Text>();

        this.effects = new List<PotionEffect>();
        this.body = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();

        // Check if the player is local, if it is set a camera, if not disable the camera
        if (view.IsMine)
        {
            GameObject cameraObj = Instantiate(cameraPrefab, transform.position, Quaternion.identity);
            cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, cameraObj.transform.position.y, -10f);
            cameraObj.transform.parent = transform;
        }
        else
        {
            Camera camera = GetComponentInChildren<Camera>();
            if (camera != null)
            {
                camera.gameObject.SetActive(false);
            }
        }

				// add mockup items to the inventory
				for (int i = 0; i < inventoryManager.GetItems().Length; i++)
				{
						inventoryManager.AddItem(inventoryManager.GetItems()[i]);
				}

    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            HandlePotions();
            OnFire();
            FireBullet();

						// handles item use clicks
						if(Input.GetMouseButtonDown(0)){
							OnConsumePotions();
							OnConsumeFood();
						}

					
             

           

            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");

            // Move if inventory is closed
            if (!inventoryManager.IsOpened())
            {
                body.velocity = transform.up * moveV * forceMultiplier + transform.right * moveH * forceMultiplier;
            }
        }

        // Disable collisions between players, using de Layer Id 6 (Player)
        Physics2D.IgnoreLayerCollision(6, 6);
    }

    void HandlePotions()
    {
				potionListText.text = "";
				for (int i = 0; i < effects.Count; i++)
				{
						PotionEffect effect = effects[i];
						if (effect.expireIn < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
						{
								effects.RemoveAt(i);
								i--;
								forceMultiplier -= effect.speed;
								strength -= effect.damage;
								continue;
						}

						float remainingTime = effect.expireIn - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
						potionListText.text += "Poção velocidade: " + effect.speed + "x (" + remainingTime + "ms)\n";
						potionListText.text += "Poção dano: " + effect.damage + " (" + remainingTime + "ms)\n"; // Display the damage effect
				}
    }

		void OnConsumePotions(){
			// Dont shoot if inventory is opened
        if (inventoryManager.IsOpened()) return;

        Item itemInHand = inventoryManager.GetSelectedItem();
				int slot = inventoryManager.GetSelectedSlot();
        // There is no item in hand
        if (!itemInHand) return;

        // Item in hand is a Weapon and Shooter
        if (itemInHand.type == ItemType.Potion && itemInHand.actionType == ActionType.Speed)
        {
						PotionItem potionItem = (PotionItem)itemInHand;
						this.effects.Add(new PotionEffect(potionItem.increaseSpeed, 0, potionItem.duration));
        		forceMultiplier += potionItem.increaseSpeed;
						// remove the potion from the inventory
						inventoryManager.RemoveItemFromSlot(slot);
				}

				// Item in hand is a Weapon and Shooter
        if (itemInHand.type == ItemType.Potion && itemInHand.actionType == ActionType.Damage)
        {
						PotionItem potionItem = (PotionItem)itemInHand;
						this.effects.Add(new PotionEffect(0, potionItem.increaseDamage, potionItem.duration));
						strength += potionItem.increaseDamage;
						// remove the potion from the inventory
						inventoryManager.RemoveItemFromSlot(slot);
				}
		}

		void OnConsumeFood(){
			// Dont shoot if inventory is opened
				if (inventoryManager.IsOpened()) return;

				Item itemInHand = inventoryManager.GetSelectedItem();
				int slot = inventoryManager.GetSelectedSlot();
				// There is no item in hand
				if (!itemInHand) return;

				// Item in hand is a Weapon and Shooter
				if (itemInHand.type == ItemType.Food && itemInHand.actionType == ActionType.Heal)
				{
						FoodItem foodItem = (FoodItem)itemInHand;
						this.AddHealth(foodItem.lifeHeal);
						// remove the potion from the inventory
						inventoryManager.RemoveItemFromSlot(slot);
				}

				if (itemInHand.type == ItemType.Food && itemInHand.actionType == ActionType.Eat)
				{
						FoodItem foodItem = (FoodItem)itemInHand;
						this.AddHealth(foodItem.foodHeal);
						// remove the potion from the inventory
						inventoryManager.RemoveItemFromSlot(slot);
				}
		}



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<EnemyDamageArea>())
        {
            EnemyDamageArea enemyDamageArea = other.gameObject.GetComponent<EnemyDamageArea>();
            RemoveHealth(enemyDamageArea.areaDamage);
            Destroy(other.gameObject);
        }
        if (other.gameObject.GetComponent<CollectibleItem>())
        {
            CollectibleItem collectibleItem = other.gameObject.GetComponent<CollectibleItem>();
            inventoryManager.AddItem(collectibleItem.item);
            Destroy(other.gameObject);
        }
    }
	
  

    public float GetHealth() => this.health;
    public void RemoveHealth(float _health)
    {
        this.health -= _health;
        if (this.health <= 0.0)
        {
            Die();
        }
    }
    public void AddHealth(float _health)
    {
        this.health += _health;
        if (this.health >= this.maxHealth)
        {
            this.health = this.maxHealth;
        }
    }
    public float GetMaxHealth() => this.maxHealth;
    public int GetStrength() => this.strength;

    public void Die()
    {
        this.health = this.maxHealth;
        transform.position = new Vector3(0, 0, 0);
    }


    private void OnFire()
    {
        shootContinuously = Input.GetMouseButton(0);
    }

    private void FireBullet()
    {
        // Dont shoot if inventory is opened
        if (inventoryManager.IsOpened()) return;

        Item itemInHand = inventoryManager.GetSelectedItem();
        // There is no item in hand
        if (!itemInHand) return;

        // Item in hand is a Weapon and Shooter
        if (itemInHand.type == ItemType.Weapon && itemInHand.actionType == ActionType.Shoot)
        {
						WeaponItem weaponItem = (WeaponItem)itemInHand;
						float bulletSpeed = weaponItem.bulletSpeed;
						float bulletLifeTime = weaponItem.bulletLifeTime;
						int bulletDamage = weaponItem.bulletDamage + strength;
						float bulletCooldown = weaponItem.bulletCooldown;
						int bulletAmount = weaponItem.bulletAmount;
						float bulletAcurracyAngle = weaponItem.bulletAcurracyAngle;
						float bulletSpreadAngle = weaponItem.bulletSpreadAngle;

            if (shootContinuously && bulletCooldownTimer <= 0f)
            {
								// get the main camera and convert the mouse position to world point
								Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
								Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

								if (bulletAmount > 1) {
									float startAngle = ((bulletAmount * bulletSpreadAngle) / 2) * -1;
									float stepAngle = bulletSpreadAngle;

									for (int i = 0; i < bulletAmount; i++)
									{
										GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
										Vector2 newDirection = Quaternion.Euler(0, 0, startAngle + (i * stepAngle)) * direction;
										bullet.GetComponent<Bullet>().InitializeBullet(this, bulletSpeed, bulletLifeTime, bulletDamage, newDirection);
									}
								}
								else
								{
									GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
									Vector2 newDirection = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-bulletAcurracyAngle, bulletAcurracyAngle)) * direction;
									bullet.GetComponent<Bullet>().InitializeBullet(this, bulletSpeed, bulletLifeTime, bulletDamage, newDirection);
								}

                bulletCooldownTimer = bulletCooldown;
            }

            bulletCooldownTimer -= Time.deltaTime;
        }
    }
}

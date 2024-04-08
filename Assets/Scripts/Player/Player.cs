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
    private float strength = 5;

    // bullet
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 2f;
    public int bulletDamage = 4;
    public float bulletCooldown = 0.5f;
    private float bulletCooldownTimer = 0f;
    private bool shootContinuously = false;

    public InventoryManager inventoryManager;
    public Item item;


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

    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            HandlePotions();
            OnFire();
            FireBullet();

            if (Input.GetKeyDown(KeyCode.C))
            {
                inventoryManager.AddItem(item);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                AddPotionEffect(new PotionEffect(5, 5000));
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
                continue;
            }

            float remainingTime = effect.expireIn - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            potionListText.text += "Poção velocidade: " + effect.speed + "x (" + remainingTime + "ms)\n";
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
    }

    void AddPotionEffect(PotionEffect potion)
    {
        this.effects.Add(potion);
        forceMultiplier += potion.speed;
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
    public float GetStrength() => this.strength;

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

            if (shootContinuously && bulletCooldownTimer <= 0f)
            {
                // instantiate bullet and set its properties
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().InitializeBullet(this, bulletSpeed, bulletLifeTime, bulletDamage);

                bulletCooldownTimer = bulletCooldown;
            }

            bulletCooldownTimer -= Time.deltaTime;
        }
    }
}

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
    private Inventory inventory;
    public PhotonView view;
    public GameObject cameraPrefab;
		

    public Image healthBar;
    private float maxHealth = 100;
    private float health = 100;
    private float strength = 5;

		// bullet
		public GameObject bulletPrefab;
		public float bulletSpeed = 10f;
		public float bulletLifeTime = 2f;
		public float bulletDamage = 4f;
		public float bulletCooldown = 0.5f;
		private float bulletCooldownTimer = 0f;
		private bool shootContinuously = false;


    // Start is called before the first frame update
    void Start()
    {
        this.potionListText = GameObject.FindGameObjectWithTag("potionText").GetComponent<Text>();

        this.effects = new List<PotionEffect>();
        this.body = GetComponent<Rigidbody2D>();
        this.inventory = GetComponent<Inventory>();
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
            UpdateLifeBar();
            HandlePotions();
						OnFire();
						FireBullet();

            if (Input.GetKeyDown(KeyCode.F))
            {
                AddPotionEffect(new PotionEffect(5, 5000));
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                this.health -= 15;
            }

            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");

            if (!inventory.IsOpened())
            {
                body.velocity = transform.up * moveV * forceMultiplier + transform.right * moveH * forceMultiplier;
            }
            else
            {
                body.velocity = new Vector2(0, 0);
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

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HIT COLISION");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HIT  ");
        }
    }


    void AddPotionEffect(PotionEffect potion)
    {
        this.effects.Add(potion);
        forceMultiplier += potion.speed;
    }

    public Inventory GetInventory() => this.inventory;
    public float GetHealth() => this.health;
    public float GetMaxHealth() => this.maxHealth;
    public float GetStrength() => this.strength;


    public void UpdateLifeBar() => this.healthBar.fillAmount = 1.0f / this.maxHealth * this.health;

		private void OnFire()
		{
			 shootContinuously = Input.GetMouseButton(0);
		}

		

		private void FireBullet()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private Rigidbody2D body;
    private float forceMultiplier = 4;
    private List<PotionEffect> effects;

    // Start is called before the first frame update
    void Start()
    {
        this.effects = new List<PotionEffect>();
        this.body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddPotionEffect(new PotionEffect(5, Time.deltaTime + 5));
        }

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        for (int i = 0; i < effects.Count; i++) {
            PotionEffect effect = effects[i];

        }

        body.velocity = transform.up * moveV * forceMultiplier + transform.right * moveH * forceMultiplier;
    }

    void AddPotionEffect(PotionEffect potion)
    {
        this.effects.Add(potion);
    }
}

public class PotionEffect
{
    public int speed;
    public float expireIn;

    public PotionEffect(int speed, float expireIn)
    {
        this.speed = speed;
        this.expireIn = expireIn;
    }
}
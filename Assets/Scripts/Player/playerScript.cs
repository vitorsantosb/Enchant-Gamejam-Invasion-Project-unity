using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private Rigidbody2D body;
    private float forceMultiplier = 4;
    private List<PotionEffect> effects;
    private TextMeshPro potionListText;

    // Start is called before the first frame update
    void Start()
    {
        this.potionListText = GetComponent<TextMeshPro>();
        this.effects = new List<PotionEffect>();
        this.body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddPotionEffect(new PotionEffect(5, 5000));
        }

        HandlePotions();

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        body.velocity = transform.up * moveV * forceMultiplier + transform.right * moveH * forceMultiplier;
    }

    void HandlePotions()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            PotionEffect effect = effects[i];

            Debug.Log(effect.expireIn - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            if (effect.expireIn < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                effects.RemoveAt(i);
                i--;
                forceMultiplier -= effect.speed;
                continue;
            }

            forceMultiplier += effect.speed;
        }
    }

    void AddPotionEffect(PotionEffect potion)
    {
        this.effects.Add(potion);
    }
}

public class PotionEffect
{
    public int speed;
    public long expireIn;

    public PotionEffect(int speed, long expireIn)
    {
        this.speed = speed;
        this.expireIn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + expireIn;
    }
}
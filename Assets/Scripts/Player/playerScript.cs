using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class playerScript : MonoBehaviour
{
    private Rigidbody2D body;
    private float forceMultiplier = 4;
    private List<PotionEffect> effects;
    private TextMeshPro potionListText;

    public PhotonView view;
    public GameObject cameraPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.potionListText = GetComponent<TextMeshPro>();
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                AddPotionEffect(new PotionEffect(5, 5000));
            }

            HandlePotions();

            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");

            body.velocity = transform.up * moveV * forceMultiplier + transform.right * moveH * forceMultiplier;
        }
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

    void AddPotionEffect(PotionEffect potion)
    {
        this.effects.Add(potion);
        forceMultiplier += potion.speed;
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
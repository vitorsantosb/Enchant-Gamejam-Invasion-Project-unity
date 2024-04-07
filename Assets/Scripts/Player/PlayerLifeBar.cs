using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeBar : MonoBehaviour
{
  	private GameObject owner;

    // Start is called before the first frame update
    void Start()
    {

				// the current object where this script is attached is under Player > Canvas > Image > Lifebar
				// we should access the player object to get the max health
				owner = transform.parent.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeBar();
    }

		public void UpdateLifeBar()
    {
        Image image = this.GetComponent<Image>();
        PlayerColliders ownerPlayer = owner.GetComponent<PlayerColliders>();
				Debug.Log(ownerPlayer.playerHealth);
        image.fillAmount = (float)ownerPlayer.playerHealth / ownerPlayer.maxHealth;
    }
}

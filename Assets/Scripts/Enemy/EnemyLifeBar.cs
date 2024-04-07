using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeBar : MonoBehaviour
{
		private GameObject owner;

    // Start is called before the first frame update
    void Start()
    {

				// the current object where this script is attached is under Enemy > Canvas > Image > Lifebar
				// we should access the enemy object to get the max health
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
        Enemy ownerEnemy = owner.GetComponent<Enemy>();
				Debug.Log(ownerEnemy.health);
        image.fillAmount = ((1.6f / ownerEnemy.maxHealth) * ownerEnemy.health);
    }
}

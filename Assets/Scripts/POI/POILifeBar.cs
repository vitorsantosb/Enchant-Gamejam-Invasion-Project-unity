using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POILifeBar : MonoBehaviour
{
		private GameObject owner;

    // Start is called before the first frame update
    void Start()
    {

				// the current object where this script is attached is under POI > Canvas > Image > Lifebar
				// we should access the POI object to get the max health
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
			POI ownerPOI = owner.GetComponent<POI>();
			Debug.Log(ownerPOI.health);
			image.fillAmount = (float)ownerPOI.health / ownerPOI.maxHealth;
		}
}

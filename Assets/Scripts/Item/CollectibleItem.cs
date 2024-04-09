using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{

		public Item item;

    // Start is called before the first frame update
    void Start()
    {
        // use item.image to add a sprite to the game object
				GetComponent<SpriteRenderer>().sprite = item.image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

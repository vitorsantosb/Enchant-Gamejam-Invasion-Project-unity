using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isReady;
    public Text buttonText;

    public void IsReady()
    {
        isReady = !isReady;
        buttonText.text = isReady ? "READY" : "UNREADY";
    }
}

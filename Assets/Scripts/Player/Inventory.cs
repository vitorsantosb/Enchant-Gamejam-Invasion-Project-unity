using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (!panel.activeSelf)
            {
                Open();
            }
        }
        else
        {
            if (panel.activeSelf)
            {
                Close();

            }
        }
    }

    public bool IsOpened() => this.panel.activeSelf;

    public void Open()
    {
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
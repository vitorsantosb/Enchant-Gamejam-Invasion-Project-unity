using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public InventorySlot[] inventorySlots;
    public Item[] items;
    public GameObject inventoryItemPrefab;
    public GameObject panel;

		[HideInInspector]
    private int selectedSlot = -1;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
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

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ChangeSelectedSlot(selectedSlot + 1 >= 7 ? 0 : selectedSlot + 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ChangeSelectedSlot(selectedSlot - 1 >= 0 ? selectedSlot - 1 : 6);
        }

        // Key pad 1 - 7 to select slot
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

		public void RemoveItem(Item item)
		{
			for (int i = 0; i < inventorySlots.Length; i++)
			{
					InventorySlot slot = inventorySlots[i];
					InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

					if (itemInSlot != null && itemInSlot.item == item)
					{
							Destroy(itemInSlot.gameObject);
							return;
					}
			}
		}

		public void RemoveItemFromSlot(int slot)
		{
			InventorySlot inventorySlot = inventorySlots[slot];
			InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();

			if (itemInSlot != null)
			{
					Destroy(itemInSlot.gameObject);
			}
		}

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            return itemInSlot.item;
        }

        return null;
    }

		public int GetSelectedSlot()
		{
			return selectedSlot;
		}

    public Item[] GetItems()
    {
        return this.items;
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

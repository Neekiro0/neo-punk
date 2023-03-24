using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public bool isEquipmentShown = false;
    public List<ItemData> items = new List<ItemData>();

    private GameObject MainUi;
    private GameObject InventoryUi;

    public void AddItem(ItemData itemData)
    {
        items.Add(itemData);
        foreach (var item in items)
        {
            Debug.Log(item.GetName());
            Debug.Log(items.Count);
        }
    }

    public void Start()
    {
        MainUi = GameObject.Find("Main User Interface");
        InventoryUi = GameObject.Find("Equipment Interface");
        MainUi.SetActive(true);
        InventoryUi.SetActive(false);
    }

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.I ))
        {
            if (isEquipmentShown == false)
            {
                Debug.Log("Działa pokazanie");
                ShowEquipment();
            }
            else
            {
                Debug.Log("Działa ukrycie");
                HideEquipment();
            }
        }
    }

    private void ShowEquipment()
    {
        isEquipmentShown = true;
        //Time.timeScale = 0;
        MainUi.SetActive(false);
        InventoryUi.SetActive(true);
    }
    public void HideEquipment()
    {
        isEquipmentShown = false;
        //Time.timeScale = 1;
        MainUi.SetActive(true);
        InventoryUi.SetActive(false);
    }
}

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
    public int selectedItemIndex = 0;
    public Sprite fieldImage;
    public Sprite selectedFieldImage;

    private GameObject MainUi;
    private GameObject InventoryUi;
    private GameObject fields;

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
        
        fields = InventoryUi.transform.Find("ItemsFields").gameObject;
    }

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.I ))
        {
            if (isEquipmentShown == false)
            {
                ShowEquipment();
            }
            else
            {
                HideEquipment();
            }
        }

        if (isEquipmentShown)
        {
            if ( Input.GetKeyDown( KeyCode.A ))
            {
                selectedItemIndex = (selectedItemIndex == 0)?3:selectedItemIndex-1;
                UpdateEquipmentFrames();
            }
            if ( Input.GetKeyDown( KeyCode.D ))
            {
                selectedItemIndex = (selectedItemIndex == 3)?0:selectedItemIndex+1;
                UpdateEquipmentFrames();
            }
        }
    }

    private void ShowEquipment()
    {
        isEquipmentShown = true;
        Time.timeScale = 0;
        MainUi.SetActive(false);
        InventoryUi.SetActive(true);
        selectedItemIndex = 0;
    }
    public void HideEquipment()
    {
        isEquipmentShown = false;
        Time.timeScale = 1;
        MainUi.SetActive(true);
        InventoryUi.SetActive(false);
    }

    private void UpdateEquipmentFrames()
    {
        foreach (Transform child in fields.transform)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = fieldImage;
            }
        }

        GameObject selectedField = fields.transform.GetChild(selectedItemIndex).gameObject;
        if (selectedFieldImage != null && selectedField != null)
        {
            selectedField.GetComponent<Image>().sprite = selectedFieldImage;
        }

    }
}

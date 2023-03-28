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

    public void AddItem(ItemData itemData, GameObject objectToDelete)
    {
        StartCoroutine(WaitForAction(itemData, objectToDelete));
    }
    
    private IEnumerator WaitForAction(ItemData itemData, GameObject objectToDelete)
    {
        ShowEquipment();
        while (isEquipmentShown)
        {
            if (Input.GetKey(KeyCode.E))
            {
                items.Insert(selectedItemIndex, itemData);
                Destroy(objectToDelete);
                HideEquipment();
            }
            yield return null;
        }

    }

    public void Start()
    {
        MainUi = GameObject.Find("Main User Interface");
        InventoryUi = GameObject.Find("Equipment Interface");
        MainUi.SetActive(true);
        InventoryUi.SetActive(false);
        
        fields = InventoryUi.transform.Find("ItemsFields").gameObject;
        
        items.Add( new ItemData( "", "", "", "", "") );
        items.Add( new ItemData( "", "", "", "", "") );
        items.Add( new ItemData( "", "", "", "", "") );
        items.Add( new ItemData( "", "", "", "", "") );
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
            GameObject actionButtonImage = child.transform.Find("ActionButtonImage").gameObject;
            if (actionButtonImage != null)
            {
                actionButtonImage.SetActive(false);
            }
        }

        GameObject selectedField = fields.transform.GetChild(selectedItemIndex).gameObject;
        if (selectedFieldImage != null && selectedField != null)
        {
            selectedField.GetComponent<Image>().sprite = selectedFieldImage;
            
            // pokazywanie przycisku do zmiany ekwipunku
            GameObject actionButtonImage = selectedField.transform.Find("ActionButtonImage").gameObject;
            if (actionButtonImage != null)
            {
                actionButtonImage.SetActive(true);
            }
        }

    }
}

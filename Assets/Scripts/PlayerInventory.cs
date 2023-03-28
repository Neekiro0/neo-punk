using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public bool isEquipmentShown = false;
    public bool isPlayerPickingItem = false;
    public List<ItemData> items = new List<ItemData>();
    public int selectedItemIndex = 0;
    public Sprite fieldImage;
    public Sprite selectedFieldImage;

    private GameObject MainUi;
    private GameObject InventoryUi;
    private GameObject fields;
    private GameObject selectedItemDesc;
    private GameObject incomingItemInfo;
    public void AddItem(ItemData itemData, GameObject objectToDelete)
    {
        isPlayerPickingItem = true;
        StartCoroutine(WaitForAction(itemData, objectToDelete));
    }
    
    private IEnumerator WaitForAction(ItemData itemData, GameObject pickedObject)
    {
        ShowEquipment();
        while (isEquipmentShown)
        {
            SetIncomingItemInfo(itemData);
            if (Input.GetKey(KeyCode.E))
            {
                items.Insert(selectedItemIndex, itemData);
                SetImageAtSlot(itemData);
                SetIncomingItemInfo(new ItemData("", "", "", "", ""));
                Destroy(pickedObject);
                HideEquipment();
                isPlayerPickingItem = false;
            }
            yield return null;
        }
    }

    public void Start()
    {
        MainUi = GameObject.Find("Main User Interface");
        InventoryUi = GameObject.Find("Equipment Interface");
        selectedItemDesc = GameObject.Find("SelectedItemInfo").gameObject;
        incomingItemInfo = GameObject.Find("IncomingItemInfo").gameObject;
        MainUi.SetActive(true);
        InventoryUi.SetActive(false);
        
        fields = InventoryUi.transform.Find("ItemsFields").gameObject;
        
        // ustawianie wyekwipowanych itemów na starcie gry
        ItemData emptyItemsData = new ItemData("", "", "", "", "");
        items.Add( emptyItemsData );
        items.Add( emptyItemsData );
        items.Add( emptyItemsData );
        items.Add( emptyItemsData );
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

    /*
     * Metoda chowająca ekwipunek
     */
    private void ShowEquipment()
    {
        isEquipmentShown = true;
        Time.timeScale = 0;
        selectedItemIndex = 0;
        UpdateEquipmentFrames();
        MainUi.SetActive(false);
        InventoryUi.SetActive(true);
    }
    
    /*
     * Metoda pokazująca ekwipunek
     */
    public void HideEquipment()
    {
        isEquipmentShown = false;
        Time.timeScale = 1;
        MainUi.SetActive(true);
        InventoryUi.SetActive(false);
    }

    /*
     * Metoda aktualizująca obrazek w UI, na podstawie podanych danych
     */
    private void SetImageAtSlot(ItemData itemData)
    {
        if (itemData.GetImagePath() != "")
        {
            GameObject selectedSlot = fields.transform.GetChild(selectedItemIndex).Find("ItemImage").gameObject;

            Texture2D texture2D = Resources.Load<Texture2D>(itemData.GetImagePath());
            if (texture2D != null)
            {
                selectedSlot.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
                MainUi.transform.Find("Items").GetChild(selectedItemIndex).GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            }
        }
    }

    private void UpdateEquipmentFrames()
    {
        // zwaracnie wszystkich pól eq do podstawowego stanu
        foreach (Transform child in fields.transform)
        {
            // resetowanie obramowania wszystkich pól
            Image image = child.transform.Find("Frame").GetComponent<Image>();
            if (image != null)
            {
                image.sprite = fieldImage;
            }

            // ukrywanie przycisków do zmiany eq
            GameObject actionButtonImage = child.transform.Find("ActionButtonImage").gameObject;
            if (actionButtonImage != null)
            {
                actionButtonImage.SetActive(false);
            }
        }

        // odpowiednie ustawienie pola wybranego
        GameObject selectedField = fields.transform.GetChild(selectedItemIndex).gameObject;
        if (selectedFieldImage != null && selectedField != null)
        {
            // ustawienie obramowania dla wybranego pola
            selectedField.transform.Find("Frame").GetComponent<Image>().sprite = selectedFieldImage;
            
            // pokazywanie przycisku do zmiany ekwipunku
            if (isPlayerPickingItem)
            {
                GameObject actionButtonImage = selectedField.transform.Find("ActionButtonImage").gameObject;
                if (actionButtonImage != null)
                {
                    actionButtonImage.SetActive(true);
                }
            }

            SetSelectedItemInfo(items[selectedItemIndex]);
        }
    }

    private void SetSelectedItemInfo(ItemData itemData)
    {
        if (itemData != null)
        {
            //Ustawianie opisu przedmiotu
            selectedItemDesc.transform.Find("ItemName").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetName();
            selectedItemDesc.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetDescription();
            selectedItemDesc.transform.Find("Lore").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetLore();
        }
    }
    private void SetIncomingItemInfo(ItemData itemData)
    {
        if (itemData != null)
        {
            //Ustawianie opisu przedmiotu
            incomingItemInfo.transform.Find("ItemName").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetName();
            incomingItemInfo.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetDescription();
            incomingItemInfo.transform.Find("Lore").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetLore();
        }
    }
}
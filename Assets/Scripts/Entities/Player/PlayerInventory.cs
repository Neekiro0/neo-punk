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
    private List<TextMeshProUGUI> itemsCooldowns = new List<TextMeshProUGUI>();

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
                List<ItemData> matchingItemsList = items.FindAll(obj => obj.GetName() == itemData.GetName());
                
                Debug.Log(matchingItemsList.Count);
                /*if ((items[selectedItemIndex].itemName == itemData.itemName))
                {
                    Debug.Log("Dodajesz item na to samo miejsce");
                }*/
                
                if (matchingItemsList.Count > 0 && (items[selectedItemIndex].GetName() != itemData.GetName()))
                {
                    Debug.Log("Istnieje już taki item w ekwipunku");
                }
                else
                {
                    items[selectedItemIndex].OnItemDisband();
                    items[selectedItemIndex] = itemData;
                    SetImageAtSlot(itemData);
                    SetIncomingItemInfo(itemData);
                    Destroy(pickedObject);
                
                    HideEquipment();
                    isPlayerPickingItem = false;
                }
                
            }
            yield return null;
        }
    }

    public void Awake()
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
        
        
        GameObject itemsCooldownsParent = MainUi.transform.Find("ItemsCooldowns").gameObject;

        for (int i = 0; i < 4; i++)
        {
            GameObject itemCooldownObject = itemsCooldownsParent.transform.GetChild(i).gameObject;
            TextMeshProUGUI itemCooldownTextComponent = itemCooldownObject.GetComponent<TextMeshProUGUI>();

            itemsCooldowns.Add(itemCooldownTextComponent);
        }
    }

    void Update()
    {
        /*
         * Używanie przedmiotów
         */
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseItem(3);
        
        /*
         * Zdolności pasywne
         */
        UsePassive(0);
        UsePassive(1);
        UsePassive(2);
        UsePassive(3);
        
        /*
         * Aktualizacja timera cooldownu
         */
        UpdateCooldownTimer(0);
        UpdateCooldownTimer(1);
        UpdateCooldownTimer(2);
        UpdateCooldownTimer(3);
        
        
        if ( Input.GetKeyDown( InputManager.InventoryMenuKey ))
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
            if ( Input.GetKeyDown( InputManager.MoveLeftKey ))
            {
                selectedItemIndex = (selectedItemIndex == 0)?3:selectedItemIndex-1;
                UpdateEquipmentFrames();
            }
            if ( Input.GetKeyDown( InputManager.MoveRightKey ) )
            {
                selectedItemIndex = (selectedItemIndex == 3)?0:selectedItemIndex+1;
                UpdateEquipmentFrames();
            }
            if ( Input.GetKeyDown( InputManager.PauseMenuKey ) )
            {
                HideEquipment();
            }
        }
    }

    public void UseItem(int ItemPos)
    {
        ItemData usedItem = items[ItemPos];
        if ( !(usedItem.GetCurrentCooldown() > 0) )
        {
            usedItem.UseItem();
            usedItem.SetCurrentCooldown(usedItem.GetCooldown());
            StartCoroutine(CooldownTimer(usedItem, ItemPos));
            
            // wyczernienie przedmiotu
            try
            {
                Image itemImage = MainUi.transform.Find("Items").transform.GetChild(ItemPos).GetComponent<Image>();

                itemImage.color = new Color32(55, 55, 55, 255);

            } catch (Exception) {}
        }
    }
    
    private IEnumerator CooldownTimer(ItemData item, int ItemPos)
    {
        while (item.GetCurrentCooldown() > 0)
        {
            yield return new WaitForSeconds(1.0f);
            item.SetCurrentCooldown( item.GetCurrentCooldown() - 1.0f );
        }
        item.SetCurrentCooldown(0);
        try
        {
            Image itemImage = MainUi.transform.Find("Items").transform.GetChild(ItemPos).GetComponent<Image>();

            itemImage.color = Color.white;

        } catch (Exception) {}
    }

    public void UsePassive(int itemPos)
    {
        ItemData usedItem = items[itemPos];
        usedItem.PassiveAbility();
    }

    public void OnItemDisband(int itemPos)
    {
        ItemData usedItem = items[itemPos];
        usedItem.OnItemDisband();
    }

    /*
     * Metoda aktualizująca timer na UI
     */
    private void UpdateCooldownTimer(int itemPos)
    {
        ItemData item = items[itemPos];
        if ( item.GetCurrentCooldown() <= item.GetCooldown() && item.GetCurrentCooldown() > 0 )
        {
            itemsCooldowns[itemPos].text = item.GetCurrentCooldown().ToString();
        }
        else
        {
            itemsCooldowns[itemPos].text = "";
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
                MainUi.transform.Find("Items").GetChild(selectedItemIndex).GetComponent<Image>().color = Color.white;
            }
        }
    }
    
    /*
     * Metoda do dynamicznej aktualizacji obrazka
     */
    public void SetImageAtSlotByIndex(String imagePath, String itemName)
    {
        int itemIndex = items.FindIndex(obj => obj.GetName() == itemName);
        
        if (imagePath != "")
        {
            GameObject selectedSlot = fields.transform.GetChild(itemIndex).Find("ItemImage").gameObject;

            Texture2D texture2D = Resources.Load<Texture2D>(imagePath);
            if (texture2D != null)
            {
                selectedSlot.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
                MainUi.transform.Find("Items").GetChild(itemIndex).GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
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
            selectedItemDesc.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetActiveDescription();
            selectedItemDesc.transform.Find("Lore").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetPassiveDescription();
        }
    }
    private void SetIncomingItemInfo(ItemData itemData)
    {
        if (itemData != null)
        {
            //Ustawianie opisu przedmiotu
            incomingItemInfo.transform.Find("ItemName").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetName();
            incomingItemInfo.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetActiveDescription();
            incomingItemInfo.transform.Find("Lore").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.GetPassiveDescription();
        }
    }
}
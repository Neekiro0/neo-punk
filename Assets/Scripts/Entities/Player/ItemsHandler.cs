using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsHandler : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();
    private GameObject MainUi;
    private List<TextMeshProUGUI> itemsCooldowns = new List<TextMeshProUGUI>();
    private PlayerInventory playerInventory;

    private void Start()
    {
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

        playerInventory = gameObject.GetComponent<PlayerInventory>();
    }

    private void Update()
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
        /*UsePassive(0);
        UsePassive(1);
        UsePassive(2);
        UsePassive(3);*/
        
        /*
         * Aktualizacja timera cooldownu
         */
        /*UpdateCooldownTimer(0);
        UpdateCooldownTimer(1);
        UpdateCooldownTimer(2);
        UpdateCooldownTimer(3);*/
    }
    
    public void AddItem(ItemData itemData, GameObject objectToDelete)
    {
        playerInventory = gameObject.GetComponent<PlayerInventory>();   
        if (playerInventory)
        {
            playerInventory.isPlayerPickingItem = true;
            StartCoroutine(WaitForAction(itemData, objectToDelete));
        }
        else
        {
            Debug.LogError("Nie znaleziono PlayerInventory");
        }
    }
    
    private IEnumerator WaitForAction(ItemData itemData, GameObject pickedObject)
    {
        gameObject.GetComponent<PlayerInventory>().ShowEquipment();
        while (gameObject.GetComponent<PlayerInventory>().isEquipmentShown)
        {
            /*
             * Wyświetlenie podnoszonego przedmiotu na UI
             */
            playerInventory.PickupItem(itemData);
            
            if (Input.GetKey(KeyCode.E))
            {
                
                List<ItemData> matchingItemsList = items.FindAll(obj => obj.GetName() == itemData.GetName());
                if (matchingItemsList.Count > 0 && (items[playerInventory.selectedItemIndex].GetName() != itemData.GetName()))
                {
                    Debug.Log("Istnieje już taki item w ekwipunku");
                }
                else
                {
                    items[playerInventory.selectedItemIndex].OnItemDisband();
                    items[playerInventory.selectedItemIndex] = itemData;
                    playerInventory.SetImageAtSlot(itemData);
                    
                    playerInventory.EndPickingItem();
                    Destroy(pickedObject);
                
                    playerInventory.HideEquipment();
                    playerInventory.isPlayerPickingItem = false;
                }
                
            }
            yield return null;
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
}

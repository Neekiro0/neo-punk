using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class LootItem
{
    public int itemId;          // ID przedmiotu
    [Range(0, 100)]
    public float dropChance;    // Szansa na dropnięcie przedmiotu w procentach
}

public class LootTable : MonoBehaviour
{
    [SerializeField]
    private List<LootItem> lootItems;  // Lista możliwych dropów

    private GameObject scriptableObjectManager;


    private void Start()
    {
        // Znalezienie obiektu ScriptableObjectManager w scenie
        scriptableObjectManager = GameObject.Find("ScriptableObjectManager");
    }

    public void DropLoot()
    {
        // Sprawdzenie, czy lootItems nie jest puste
        if (lootItems == null || lootItems.Count == 0)
        {
            Debug.LogWarning("LootTable is empty or null!");
            return;
        }

        // Przeliczanie szansy na drop
        foreach (var lootItem in lootItems)
        {
            float roll = Random.Range(0f, 100f);  // Losowanie liczby od 0 do 100

            if (roll <= lootItem.dropChance)  // Sprawdzenie, czy przedmiot dropnie
            {
                SpawnItem(lootItem.itemId);
                break;  // Wychodzimy po pierwszym dropie
            }
        }
    }

    private void SpawnItem(int itemId)
    {
        // Znajdowanie odpowiedniego ItemData w ScriptableObjectManager
        
        var itemData = scriptableObjectManager.GetComponent<ScriptableObjectManager>().GetItemData(itemId);

        if (itemData == null)
        {
            Debug.LogError($"Item with ID {itemId} not found in ScriptableObjectManager.");
            return;
        }

        // Wczytanie prefaba przedmiotu z zasobów
        GameObject itemPrefab = Resources.Load<GameObject>($"Items/Prefabs/ItemPrefab");

        if (itemPrefab == null)
        {
            Debug.LogError("ItemPrefab not found in Resources/Items/Prefabs.");
            return;
        }

        // Spawnowanie przedmiotu
        Vector3 position = transform.position;
        position.z = 5;
        GameObject spawnedItem = Instantiate(itemPrefab, position, Quaternion.identity);
        
        // Ustawienie ID przedmiotu na odpowiednie
        var itemComponent = spawnedItem.GetComponentInChildren<ItemObject>();
        itemComponent.ScriptableObjectManager = scriptableObjectManager;
        if (itemComponent != null)
        {
            itemComponent.ItemId = itemId;
        }
        else
        {
            Debug.LogError("No ItemComponent found on the spawned item's child.");
        }
    }
}

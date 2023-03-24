using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();

    public void AddItem(ItemData itemData)
    {
        items.Add(itemData);
        foreach (var item in items)
        {
            Debug.Log(item.GetName());
            Debug.Log(items.Count);
        }
    }
}

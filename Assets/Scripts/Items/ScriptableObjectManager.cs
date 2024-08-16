using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    public static ScriptableObjectManager Instance { get; private set; }

    // Lista lub pojedyncze referencje do obiektów ScriptableObject
    public List<ItemData> itemDataList; // Możesz dodać wiele obiektów

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zabezpiecza przed zniszczeniem przy zmianie scen
        }
        else
        {
            Destroy(gameObject); // Usuwamy duplikaty menedżera
        }
    }

    public ItemData GetItemData(int id)
    {
        return itemDataList[id];
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UnityScheduler : MonoBehaviour
{
    private static UnityScheduler _instance;
    public static UnityScheduler Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject(typeof(UnityScheduler).Name);
                _instance = go.AddComponent<UnityScheduler>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

public class ItemObject : MonoBehaviour
{
    // Referencja do ScriptableObject z danymi przedmiotu
    public ItemData itemData;
    public Color CommonColor;
    public Color RareColor;
    public Color QuantumColor;

    private SpriteRenderer spriteRenderer;
    private Light2D light2D;
    private ParticleSystem particleSystem;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2D = GetComponentInParent<Light2D>();
        particleSystem = gameObject.transform.parent.GetComponentInChildren<ParticleSystem>();

        // Ustawienie wyglądu i właściwości przedmiotu
        if (itemData != null)
        {
            SetItemAppearance();
            UpdateItemLightColor();
        }
        else
        {
            Debug.LogWarning("ItemData jest null dla obiektu: " + gameObject.name);
        }
    }

    private void SetItemAppearance()
    {
        if (itemData.itemIcon != null)
        {
            spriteRenderer.sprite = itemData.itemIcon;
        }
        else
        {
            Debug.LogWarning("Nie można załadować sprite'a dla przedmiotu: " + itemData.itemName);
        }
    }

    private void UpdateItemLightColor()
    {
        var main = particleSystem.main;
        switch (itemData.rarity)
        {
            case "Common":
                light2D.color = CommonColor;
                main.startColor = CommonColor;
                break;
            case "Rare":
                light2D.color = RareColor;
                main.startColor = RareColor;
                break;
            case "Quantum":
                light2D.color = QuantumColor;
                main.startColor = QuantumColor;
                break;
            default:
                light2D.color = CommonColor;
                main.startColor = CommonColor;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && Input.GetKeyDown(InputManager.InteractKey))
        {
            ItemsHandler itemsHandler = col.GetComponent<ItemsHandler>();
            if (itemsHandler != null && itemData != null)
            {
                StartCoroutine(WaitForFrameThenAddItem(itemsHandler));
            }
        }
    }

    private IEnumerator WaitForFrameThenAddItem(ItemsHandler itemsHandler)
    {
        yield return new WaitForEndOfFrame();
        itemsHandler.AddItem(itemData, gameObject);
        gameObject.SetActive(false); // Ukryj przedmiot po podniesieniu, zamiast go niszczyć
    }
}

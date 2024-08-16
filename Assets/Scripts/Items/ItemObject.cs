using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("CoroutineRunner");
                instance = obj.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
}

public class ItemObject : MonoBehaviour
{
    public GameObject ScriptableObjectManager;
    public int ItemId;
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
        itemData = ScriptableObjectManager.GetComponent<ScriptableObjectManager>().GetItemData(ItemId);

        if (itemData != null)
        {
            spriteRenderer.sprite = itemData.itemIcon;
            UpdateItemLightColor();

            // Inicjalizujemy przypisane zdolności (itemAbility) tylko raz
            if (itemData.itemAbility != null)
            {
                itemData.itemAbility.Apply(); // Możemy od razu zastosować zdolność
            }
        }
        else
        {
            Debug.LogError("ItemData is not assigned!");
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
        if (col.CompareTag("Player") && Input.GetKey(InputManager.InteractKey))
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
    }
}
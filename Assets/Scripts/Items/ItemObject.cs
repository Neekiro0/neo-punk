using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    // Referencja do ScriptableObject z danymi przedmiotu
    public ItemData itemData;

    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (itemData != null)
        {
            // Ustawienie wyglądu obiektu na podstawie danych przedmiotu
            SetItemAppearance();
        }
    }

    private void SetItemAppearance()
    {
        // Wczytywanie sprite'a na podstawie ścieżki z itemData
        if (itemData.itemIcon != null)
        {
            spriteRenderer.sprite = itemData.itemIcon;
        }
        else
        {
            Debug.LogWarning("Nie można załadować sprite'a dla przedmiotu: " + itemData.itemName);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        GameObject entity = col.gameObject;
        if ( entity.CompareTag("Player") && Input.GetKeyDown( InputManager.InteractKey ) )
        {
            // dodawanie do ekwipunku
            try
            {
                ItemsHandler itemsHandler = entity.GetComponent<ItemsHandler>();
                StartCoroutine(WaitForFrameThenAddItem(itemData, gameObject, itemsHandler));
            } catch (Exception) {
                Debug.Log("Wystąpił błąd");
            }
        }
        
        IEnumerator WaitForFrameThenAddItem(ItemData itemData, GameObject gameObject, ItemsHandler itemsHandler)
        {
            yield return 0;
            itemsHandler.AddItem(itemData, gameObject);
        }
    }
}
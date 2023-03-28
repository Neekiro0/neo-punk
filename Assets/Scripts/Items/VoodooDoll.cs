using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static Items.ItemData;

public class VoodooDoll : MonoBehaviour
{
    public string itemName = "Voodoo Doll";
    public string damageType = "none";
    public string description = "Gives 15% more damage, but if you get hit, doll gets a needle stack. If you get hit when Doll have 3 stacks you will die.";
    public string lore;
    private string rarity;
    private double damage;
    private double minPlayerLvl;
    
    private Items.ItemData itemData;
    private GameObject titleObject;
    private GameObject player;
    public bool _isPlayerInsideOfRange = false;
    
    void Start()
    {
        itemData = new Items.ItemData(
            itemName,
            damageType,
            description,
            lore,
            rarity
            );
        titleObject = gameObject.transform.Find("Title").gameObject;
        titleObject.GetComponent<Renderer>().enabled = false;
        titleObject.GetComponent<TextMesh>().text = itemName;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject entity = col.gameObject;
        if (entity.CompareTag("Player"))
        {
            titleObject.GetComponent<Renderer>().enabled = true;

            _isPlayerInsideOfRange = true;
            player = entity;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        GameObject entity = col.gameObject;
        if (entity.CompareTag("Player"))
        {
            titleObject.GetComponent<Renderer>().enabled = false;
            _isPlayerInsideOfRange = false;
            //Destroy(gameObject);
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        GameObject entity = col.gameObject;
        if ( entity.CompareTag("Player") && ( Input.GetKey( KeyCode.F ) ) )
        {
            // dodawanie do ekwipunku
            try
            {
                player.GetComponent<PlayerInventory>().AddItem(itemData, gameObject);
            }
            catch (Exception)
            {
                Debug.Log("Wystąpił błąd");
            }
        }
    }
    
}

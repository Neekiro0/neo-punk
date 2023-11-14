using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    internal class DemonBell : ItemData
    {
        public EntityStatus playerStatus;
        public PlayerInventory playerInventory;
        public Player player;

        private float effectDuration = 10.0f; // Czas trwania efektu w sekundach

        private bool isEffectActive = false; // Czy efekt jest aktywny
        private float effectEndTime = 0.0f; // Czas zakończenia efektu
        private int lastElementalType; // ostatnio używany element
        public DemonBell(string itemName, string passiveDescription, string activeDescription, string rarity,
            string imagePath = "", float cooldown = 0, float currentCooldown = 0, float minPlayerLvl = 0, int needleStacks = 0, EntityStatus playerStatus = null, PlayerInventory playerInventory = null)
            : base(itemName, passiveDescription, activeDescription, rarity, imagePath, cooldown, currentCooldown, minPlayerLvl)
        {
            this.playerStatus = GameObject.Find("Player").GetComponent<EntityStatus>();
            this.playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
            this.player = GameObject.Find("Player").GetComponent<Player>();
            Debug.Log(this.playerStatus);
        }

        public override void PassiveAbility()
        {
            
            // część UseItem
            if (isEffectActive && !(Time.time < effectEndTime))
            {
                // po zakończeniu UseItem
                player.ChangeElementalType(0);

                isEffectActive = false; // Wyłączamy flagę aktywności efektu
            }
        }

        public override void UseItem()
        {
            if (!isEffectActive)
            {
                // Rozpoczynamy efekt zwiększenia obrażeń
                isEffectActive = true;
                effectEndTime = Time.time + effectDuration;
                lastElementalType = player.UsedElementalTypeId;
                player.ChangeElementalType(5);
            }
        }
    }

    public class DemonBellItem : MonoBehaviour
    {
        
        private GameObject titleObject;
        private GameObject player;
        public bool _isPlayerInsideOfRange = false;
        private DemonBell demonBell; 
        
        void Start()
        {
            demonBell = new DemonBell(
                "Demon Bell",
                "Player takes 35% more damage, but also deals 25% more.",
                "Overrides damage type to Bloody for 10 seconds.",
                "Common",
                "Items/DemonBell/DemonBell",
                40.0f,
                0.0f );
            
            titleObject = gameObject.transform.Find("Title").gameObject;
            titleObject.GetComponent<Renderer>().enabled = false;
            titleObject.GetComponent<TextMesh>().text = demonBell.GetName();
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
            }
        }

        public void OnTriggerStay2D(Collider2D col)
        {
            GameObject entity = col.gameObject;
            if ( entity.CompareTag("Player") && ( Input.GetKey( KeyCode.F ) ) )
            {
                // dodawanie do ekwipunku
                try {
                    player.GetComponent<PlayerInventory>().AddItem(demonBell, gameObject);
                } catch (Exception) {
                    Debug.Log("Wystąpił błąd");
                }
            }
        }
    }
}

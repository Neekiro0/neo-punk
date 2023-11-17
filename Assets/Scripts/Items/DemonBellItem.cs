using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    internal class DemonBell : ItemData
    {
        public EntityStatus playerStatus;
        public GameObject playerEntity;
        public Player player;
        private HitboxBehaviour swordHitbox;
        
        private float effectDuration = 10.0f; // Czas trwania efektu w sekundach

        private bool isEffectActive = false; // Czy efekt jest aktywny
        private float effectEndTime = 0.0f; // Czas zakończenia efektu
        private int lastElementalType; // ostatnio używany element

        private float additionalDamage = 0.25f;
        private float addedDamage = 0.0f;

        private float defenceLoweringPercent = 0.35f;
        private float loweredDefence = 0.0f;
        
        
        
        private bool isDamageBonusGranted = false;
        
        public DemonBell(string itemName, string passiveDescription, string activeDescription, string rarity,
            string imagePath = "", float cooldown = 0, float currentCooldown = 0, float minPlayerLvl = 0, int needleStacks = 0, EntityStatus playerStatus = null, PlayerInventory playerInventory = null)
            : base(itemName, passiveDescription, activeDescription, rarity, imagePath, cooldown, currentCooldown, minPlayerLvl)
        {
            this.playerEntity = GameObject.Find("Player").gameObject;
            this.playerStatus = playerEntity.GetComponent<EntityStatus>();
            this.player = GameObject.Find("Player").GetComponent<Player>();
            this.swordHitbox = player.transform.Find("SwordHitbox").gameObject.GetComponent<HitboxBehaviour>();
        }

        public override void PassiveAbility()
        {
            /*
             * Zainicjowanie pasywnych bonusów
             */
            if (!isDamageBonusGranted)
            {
                addedDamage = playerStatus.GetBaseAttackDamage() * additionalDamage;
                loweredDefence = playerStatus.incomingDamagePercent * defenceLoweringPercent;

                playerStatus.AttackDamage = playerStatus.GetBaseAttackDamage() + addedDamage;
                playerStatus.incomingDamagePercent += loweredDefence;
                
                isDamageBonusGranted = true;
            }
            
            // część UseItem
            if (isEffectActive && !(Time.time < effectEndTime))
            {
                // po zakończeniu UseItem
                player.ChangeElementalType(lastElementalType);

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

        public override void OnItemDisband()
        {
            playerStatus.AttackDamage = playerStatus.AttackDamage - addedDamage;
            playerStatus.incomingDamagePercent -= loweredDefence;
            
            isDamageBonusGranted = false;
            player.ChangeElementalType(lastElementalType);
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
            // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
            demonBell = new DemonBell(
                "Demon bell",
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
                    player.GetComponent<ItemsHandler>().AddItem(demonBell, gameObject);
                } catch (Exception) {
                    Debug.Log("Wystąpił błąd");
                }
            }
        }
    }
}

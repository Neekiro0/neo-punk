using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static ItemData;

namespace Items
{
    internal class VoodooDoll : ItemData
    {
        private float needleStacks;
        private float lastNoticedPlayerHp;
        public EntityStatus playerStatus;
        public PlayerInventory playerInventory;
        private bool isItemInInventory = true;
        
        private float baseDamage; // Bazowa wartość obrażeń
        private float damageIncreasePercentage = 0.4f; // Procent zwiększenia obrażeń o 40%
        private float effectDuration = 10.0f; // Czas trwania efektu w sekundach

        private bool isEffectActive = false; // Czy efekt jest aktywny
        private float effectEndTime = 0.0f; // Czas zakończenia efektu

        public VoodooDoll(string itemName, string passiveDescription, string activeDescription, string rarity,
            string imagePath = "", float cooldown = 0, float currentCooldown = 0, float minPlayerLvl = 0, int needleStacks = 0, EntityStatus playerStatus = null, PlayerInventory playerInventory = null)
            : base(itemName, passiveDescription, activeDescription, rarity, imagePath, cooldown, currentCooldown, minPlayerLvl)
        {
            this.needleStacks = needleStacks;
            this.playerStatus = GameObject.Find("Player").GetComponent<EntityStatus>();
            this.playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
            
            
            baseDamage = this.playerStatus.GetAttackDamageCount();
        }

        public override void PassiveAbility()
        {

            if (isItemInInventory)
            {
                lastNoticedPlayerHp = playerStatus.GetHp();
                needleStacks = 0;
                isItemInInventory = false;
            }
            
            if (playerStatus.GetHp() < lastNoticedPlayerHp)
            {
                needleStacks += 1;
                lastNoticedPlayerHp = playerStatus.GetHp();
                
                playerInventory.SetImageAtSlotByIndex("Items/VoodooDoll/VoodooDoll_"+needleStacks.ToString(), "Voodoo Doll");
            }

            if (needleStacks > 3)
            {
                playerStatus.PlayerDeathEvent();
            }

            if (isEffectActive && Time.time < effectEndTime)
            {
                // Zwiększamy obrażenia o 40% podczas trwania efektu
                float currentDamage = baseDamage * (1.0f + damageIncreasePercentage);
                playerStatus.SetAttackDamageCount(currentDamage);
            }
            else if (isEffectActive)
            {
                // Po zakończeniu efektu przywracamy normalne obrażenia
                playerStatus.SetAttackDamageCount(baseDamage);

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
            }
        }

        public override void OnItemDisband()
        {
            isItemInInventory = false;
            playerStatus.SetAttackDamageCount(baseDamage);
        }
    }
    public class VoodooDollItem : MonoBehaviour
    {
        private GameObject titleObject;
        private GameObject player;
        public bool _isPlayerInsideOfRange = false;
        private VoodooDoll voodooDoll; 
        
        void Awake()
        {
            // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
            voodooDoll = new VoodooDoll(
                "Voodoo doll",
                "If you get hit, doll gets a needle stack. If you get hit when Doll have 3 stacks you will die.",
                "Amplifies damage by 40% for 10 seconds.",
                "Common",
                "Items/VoodooDoll/VoodooDoll",
                30.0f,
                0.0f );
            
            titleObject = gameObject.transform.Find("Title").gameObject;
            titleObject.GetComponent<Renderer>().enabled = false;
            titleObject.GetComponent<TextMesh>().text = voodooDoll.GetName();
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
            if ( entity.CompareTag("Player") && Input.GetKeyDown( InputManager.InteractKey ) )
            {
                // dodawanie do ekwipunku
                try {
                    StartCoroutine(WaitForFrameThenAddItem(voodooDoll, gameObject));
                } catch (Exception) {
                    Debug.Log("Wystąpił błąd");
                }
            }
        }
        
        IEnumerator WaitForFrameThenAddItem(VoodooDoll voodooDoll, GameObject gameObject)
        {
            yield return 0;
            player.GetComponent<ItemsHandler>().AddItem(voodooDoll, gameObject);
        }
    }
}

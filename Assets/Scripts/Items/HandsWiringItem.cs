using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    internal class HandsWiring : ItemData
    {
        public EntityStatus playerStatus;
        public GameObject playerEntity;
        public Player player;
        private HitboxBehaviour swordHitbox;
        
        private float effectDuration = 10.0f; // Czas trwania efektu w sekundach

        private bool isEffectActive = false; // Czy efekt jest aktywny
        private float effectEndTime = 0.0f; // Czas zakończenia efektu
        private int lastElementalType; // ostatnio używany element

        private int playerInitialElementalType;
        
        private bool isPassiveGranted = false;
        public GameObject explosionEffectPrefab;
        public float explosionRange;
        public float explosionForce;
        
        public HandsWiring(string itemName, string passiveDescription, string activeDescription, string rarity,
            string imagePath = "", float cooldown = 0, float currentCooldown = 0, float minPlayerLvl = 0, GameObject explosionEffectPrefab = null, float explosionRange = 0, float explosionForce = 0)
            : base(itemName, passiveDescription, activeDescription, rarity, imagePath, cooldown, currentCooldown, minPlayerLvl)
        {
            this.explosionEffectPrefab = explosionEffectPrefab;
            this.explosionRange = explosionRange;
            this.explosionForce = explosionForce;
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
            if (!isPassiveGranted)
            {
                playerInitialElementalType = player.UsedElementalTypeId;
                player.ChangeElementalType(1);
                
                isPassiveGranted = true;
            }
            
            // część UseItem
            if (isEffectActive && !(Time.time < effectEndTime))
            {
                // po zakończeniu UseItem

                isEffectActive = false; // Wyłączamy flagę aktywności efektu
            }
        }

        public override void UseItem()
        {
            if (explosionEffectPrefab != null)
            {
                GameObject explosion = Instantiate(explosionEffectPrefab, playerEntity.transform.position, Quaternion.identity);
                explosion.transform.parent = playerEntity.transform;
                
                // Dodaj inne operacje związane z użyciem przedmiotu

                // Sprawdź, czy efekt wybuchu posiada komponent ParticleSystem
                ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                    // Pobierz moduł kolizji

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(playerEntity.transform.position, explosionRange);

                    foreach (Collider2D nearbyObject in colliders)
                    {
                        Rigidbody2D rigidbody2D = nearbyObject.GetComponent<Rigidbody2D>();
                        if (rigidbody2D && nearbyObject.gameObject.CompareTag("Enemy") )
                        {
                            /*
                             * Odpychanie celu
                             */
                            Vector3 direction = (nearbyObject.transform.position - playerEntity.transform.position).normalized;
                            direction.y = explosionForce * 0.02f; 
                            rigidbody2D.AddForce(direction * explosionForce, ForceMode2D.Impulse);
                            
                            /*
                             * Zadawanie mu obrażeń
                             */
                            EntityStatus entityStatus = nearbyObject.gameObject.GetComponent<EntityStatus>();
                            entityStatus.DealDamage(playerStatus.GetAttackDamageCount() * 0.2f );
                        }
                    }
                }
            }

            if (!isEffectActive)
            {
                // Rozpoczynamy efekt zwiększenia obrażeń
                isEffectActive = true;
                effectEndTime = Time.time + effectDuration;
                /*lastElementalType = player.UsedElementalTypeId;
                player.ChangeElementalType(5);*/
            }
        }

        public override void OnItemDisband()
        {
            isPassiveGranted = false;
            player.ChangeElementalType(lastElementalType);
        }
        
        private void OnParticleCollision(GameObject other)
        {
            // Sprawdź, czy obiekt posiada tag "enemy"
            if (other.CompareTag("enemy"))
            {
                // Tutaj możesz umieścić kod reakcji na kolizję z obiektem przeciwnika
                Debug.Log("Enemy hit by explosion!");
            }
        }
    }
    public class HandsWiringItem : MonoBehaviour
    {
        
        private GameObject titleObject;
        private GameObject player;
        private bool _isPlayerInsideOfRange = false;
        private HandsWiring handsWiring; 
        public GameObject explosionEffectPrefab;
        public float explosionRange;
        public float explosionForce;
        
        void Start()
        {
            // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
            handsWiring = new HandsWiring(
                "Hands wiring",
                "Changes basic type of damage to electric.",
                "Produces electric discharge, which deals damage equal 20% of player AD and disables cybernetic enemies for 2 seconds.",
                "Common",
                "Items/HandsWiring/HandsWiring",
                11.0f,
                0.0f, 
                0,
                explosionEffectPrefab,
                explosionRange,
                explosionForce );
            
            titleObject = gameObject.transform.Find("Title").gameObject;
            titleObject.GetComponent<Renderer>().enabled = false;
            titleObject.GetComponent<TextMesh>().text = handsWiring.GetName();
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
                    player.GetComponent<ItemsHandler>().AddItem(handsWiring, gameObject);
                } catch (Exception) {
                    Debug.Log("Wystąpił błąd");
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityScheduler : MonoBehaviour {
    private static UnityScheduler _instance;
    public static UnityScheduler Instance {
        get {
            if(_instance == null) {
                var go = new GameObject(typeof(UnityScheduler).Name) ;
                _instance = go.AddComponent<UnityScheduler>() ;
            }
            return _instance;
        }
    }

    private void OnDestroy () {
        if(_instance == this) {
            _instance = null;
        }
    }
}

// Implementacja aktywnej zdolności Voodoo Doll
[System.Serializable]
public class VoodooDollActiveAbility : ItemData.IActiveAbility
{
    private float damageIncreasePercentage;
    private float effectDuration;

    public VoodooDollActiveAbility(float damageIncreasePercentage, float effectDuration)
    {
        this.damageIncreasePercentage = damageIncreasePercentage;
        this.effectDuration = effectDuration;
    }

    public void Remove()
    {
        GameObject player = GameObject.Find("Map").transform.Find("Player").gameObject;
        player.GetComponent<EntityStatus>().SetAttackDamageCount(player.GetComponent<EntityStatus>().GetBaseAttackDamage());
    }

    public void Use()
    {
        // Zwiększenie obrażeń gracza
        EntityStatus playerStatus = GameObject.Find("Map").transform.Find("Player").gameObject.GetComponent<EntityStatus>();
        float baseDamage = playerStatus.GetBaseAttackDamage();
        
        playerStatus.SetAttackDamageCount(baseDamage * (1.0f + damageIncreasePercentage));
        
        // Rozpoczęcie coroutine dla przywrócenia obrażeń po zakończeniu efektu
        UnityScheduler.Instance.StartCoroutine(ResetDamageAfterDuration(playerStatus, baseDamage));
    }

    private IEnumerator ResetDamageAfterDuration(EntityStatus playerStatus, float baseDamage)
    {
        yield return new WaitForSeconds(effectDuration);
        playerStatus.SetAttackDamageCount(baseDamage);
    }
}

// Implementacja pasywnej zdolności Voodoo Doll
[System.Serializable]
public class VoodooDollPassiveAbility : ItemData.IPassiveAbility
{
    private float needleStacks;
    private EntityStatus playerStatus;
    private PlayerInventory playerInventory;
    private float lastNoticedPlayerHp;
    private bool isItemInInventory;

    public VoodooDollPassiveAbility()
    {
        this.playerStatus = null;
        this.playerInventory = null;
        this.needleStacks = 0;
        this.lastNoticedPlayerHp = GameObject.Find("Map").transform.Find("Player").gameObject.GetComponent<EntityStatus>().GetHp();
    }

    public void Apply()
    {
        if (playerStatus == null)
        {
            playerStatus = GameObject.Find("Map").transform.Find("Player").gameObject.GetComponent<EntityStatus>();
        }
        if (playerInventory == null)
        {
            playerInventory = GameObject.Find("Map").transform.Find("Player").gameObject.GetComponent<PlayerInventory>();
        }

        if (playerStatus.GetHp() < lastNoticedPlayerHp)
        {
            needleStacks += 1;
            lastNoticedPlayerHp = playerStatus.GetHp();

            playerInventory.SetImageAtSlotByIndex("Items/VoodooDoll/VoodooDoll_" + needleStacks.ToString(), "Voodoo Doll");

            if (needleStacks > 3)
            {
                playerStatus.PlayerDeathEvent();
            }
        }
    }
}

[CreateAssetMenu(fileName = "New Voodoo Doll", menuName = "Items/Voodoo Doll")]
public class VoodooDoll : ItemData
{
    private EntityStatus playerStatus;
    private PlayerInventory playerInventory;
    
    private void OnEnable()
    {
        currentCooldown = 0;
        activeAbility = new VoodooDollActiveAbility(0.4f, 10f);
        passiveAbility = new VoodooDollPassiveAbility();
    }
}

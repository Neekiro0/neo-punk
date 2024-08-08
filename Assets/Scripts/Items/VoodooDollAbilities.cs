using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementacja aktywnej zdolności Voodoo Doll
[System.Serializable]
public class VoodooDollActiveAbility : ItemData.IActiveAbility
{
    private float damageIncreasePercentage;
    private float effectDuration;
    private EntityStatus playerStatus;
    private float baseDamage;

    public VoodooDollActiveAbility(float damageIncreasePercentage, float effectDuration)
    {
        this.playerStatus = GameObject.Find("Map").transform.Find("Player").gameObject.GetComponent<EntityStatus>();
        this.damageIncreasePercentage = damageIncreasePercentage;
        this.effectDuration = effectDuration;
        this.baseDamage = playerStatus.GetAttackDamageCount();
    }

    public void Remove()
    {
        playerStatus.SetAttackDamageCount(this.playerStatus.GetAttackDamageCount());
    }

    public void Use()
    {
        // Zwiększenie obrażeń gracza
        playerStatus.SetAttackDamageCount(baseDamage * (1.0f + damageIncreasePercentage));

        // Rozpoczęcie coroutine dla przywrócenia obrażeń po zakończeniu efektu
        playerStatus.StartCoroutine(ResetDamageAfterDuration());
    }

    private IEnumerator ResetDamageAfterDuration()
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

    public VoodooDollPassiveAbility(EntityStatus status, PlayerInventory inventory)
    {
        this.playerStatus = status;
        this.playerInventory = inventory;
        this.needleStacks = 0;
        this.lastNoticedPlayerHp = playerStatus.GetHp();
    }

    public void Apply()
    {
        lastNoticedPlayerHp = playerStatus.GetHp();
        /*if (isItemInInventory)
        {
            needleStacks = 0;
            isItemInInventory = false;
        }*/

        //Debug.Log(playerStatus.GetHp()+" < " + lastNoticedPlayerHp);
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
        GameObject player = GameObject.Find("Map").transform.Find("Player").gameObject;
        playerStatus = player.GetComponent<EntityStatus>();
        playerInventory = player.GetComponent<PlayerInventory>();
        
        activeAbility = new VoodooDollActiveAbility(0.4f, 10f);
        passiveAbility = new VoodooDollPassiveAbility(playerStatus, playerInventory);
    }
}

using System.Collections;
using UnityEngine;

[System.Serializable]
public class VoodooDollAbility : ItemData.IItemAbility
{
    // Pola dla zdolności aktywnej
    private float damageIncreasePercentage;
    private float effectDuration;
    
    // Pola dla zdolności pasywnej
    private float needleStacks;
    private EntityStatus playerStatus;
    private PlayerInventory playerInventory;
    private float lastNoticedPlayerHp;
    private bool isItemInInventory;

    public VoodooDollAbility(float damageIncreasePercentage, float effectDuration)
    {
        this.damageIncreasePercentage = damageIncreasePercentage;
        this.effectDuration = effectDuration;

        this.playerStatus = null;
        this.playerInventory = null;
        this.needleStacks = 0;
        this.lastNoticedPlayerHp = 0;
    }

    // Implementacja zdolności aktywnej
    public void Use()
    {
        // Zwiększenie obrażeń gracza
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

    // Implementacja zdolności pasywnej
    public void Apply()
    {
        if (null == playerStatus)
        {
            this.playerStatus = GameObject.FindWithTag("Player").gameObject.GetComponent<EntityStatus>();
        }
        if (null == playerInventory)
        {
            this.playerInventory = GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerInventory>();
        }
        if (0 == lastNoticedPlayerHp && playerStatus)
        {
            this.lastNoticedPlayerHp = playerStatus.GetHp();
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

    // Implementacja usuwania przedmiotu
    public void Remove()
    {
        playerStatus.SetAttackDamageCount(playerStatus.GetBaseAttackDamage());
    }
}

[CreateAssetMenu(fileName = "New Voodoo Doll", menuName = "Items/Voodoo Doll")]
public class VoodooDoll : ItemData
{
    [SerializeField] private GameObject player;
    private void OnEnable()
    {
        currentCooldown = 0;
        itemAbility = new VoodooDollAbility(0.4f, 10f);
    }
}

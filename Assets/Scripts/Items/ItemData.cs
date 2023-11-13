using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ItemData : MonoBehaviour
{
    public string itemName;
    private string damageType;
    private string description;
    private string lore;
    private float cooldown;
    private float currentCooldown;
    private string rarity;
    private double damage;
    private double minPlayerLvl;
    private string imagePath;

    public ItemData(string itemName, string damageType, string description, string Lore, string rarity, string imagePath = "", float cooldown = 0, float currentCooldown = 0, double damage = 0, double minPlayerLvl = 0 )
    {
        this.itemName = itemName;
        this.damageType = damageType;
        this.description = description;
        this.lore = Lore;
        this.cooldown = cooldown;
        this.currentCooldown = currentCooldown;
        this.imagePath = imagePath;
        this.damage = damage;
        this.rarity = rarity;
        this.minPlayerLvl = minPlayerLvl;
    }

    public void Update()
    {
        PassiveAbility();
    }

    /*
     * Nazwa przedmiotu
     */
    public void SetName(string itemName)
    {
        this.itemName = itemName;
    }
    public string GetName()
    {
        return this.itemName;
    }

    /*
     * Damage type
     */
    public void SetDamageType(string damageType)
    {
        this.damageType = damageType;
    }
    public string GetDamageType()
    {
        return this.damageType;
    }

    /*
     * Description
     */
    public void SetDescription(string description)
    {
        this.description = description;
    }
    public string GetDescription()
    {
        return this.description;
    }

    /*
     * Lore
     */
    public void SetLore(string lore)
    {
        this.lore = lore;
    }
    public string GetLore()
    {
        return this.lore;
    }

    /*
     * Cooldown
     */
    public void SetCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }
    public float GetCooldown()
    {
        return this.cooldown;
    }

    /*
     * Current cooldown
     */
    public void SetCurrentCooldown(float currentCooldown)
    {
        this.currentCooldown = currentCooldown;
    }
    public float GetCurrentCooldown()
    {
        return this.currentCooldown;
    }

    /*
     * ścieżka to obrazka
     */
    public void SetImagePath(string imagePath)
    {
        this.imagePath = imagePath;
    }
    public string GetImagePath()
    {
        return this.imagePath;
    }

    public virtual void UseItem() {}
    public virtual void PassiveAbility() {}
}


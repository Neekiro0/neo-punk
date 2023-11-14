using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ItemData : MonoBehaviour
{
    private string itemName;
    private string passiveDescription;
    private string activeDescription;
    private float cooldown;
    private float currentCooldown;
    private string rarity;
    private float minPlayerLvl;
    private string imagePath;

    public ItemData(string itemName, string passiveDescription, string activeDescription, string rarity, string imagePath = "", float cooldown = 0, float currentCooldown = 0, float minPlayerLvl = 0 )
    {
        this.itemName = itemName;
        this.passiveDescription = passiveDescription;
        this.activeDescription = activeDescription;
        this.cooldown = cooldown;
        this.currentCooldown = currentCooldown;
        this.imagePath = imagePath;
        this.rarity = rarity;
        this.minPlayerLvl = minPlayerLvl;
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
     * passiveDescription
     */
    public void SetPassiveDescription(string passiveDescription)
    {
        this.passiveDescription = passiveDescription;
    }
    public string GetPassiveDescription()
    {
        return this.passiveDescription;
    }

    /*
     * activeDecription
     */
    public void SetActiveDescription(string activeDescription)
    {
        this.activeDescription = activeDescription;
    }
    public string GetActiveDescription()
    {
        return this.activeDescription;
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
     * rzadkość przedmiotu
     */
    public void SetRarity(string rarity)
    {
        this.rarity = rarity;
    }
    public string GetRarity()
    {
        return this.rarity;
    }

    /*
     * Minimalny poziom gracza do podniesienia
     */
    public void SetMinlvl(float minLvl)
    {
        this.minPlayerLvl = minLvl;
    }
    public float GetMinlvl()
    {
        return this.minPlayerLvl;
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


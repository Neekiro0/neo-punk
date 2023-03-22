using System;
using Unity.Collections;
using UnityEngine;

public class EntityStatus : MonoBehaviour
{
    public string entityName = "";
    public int entityLevel = 1;
    public int entityExperiencePoints = 0;
    [ReadOnly] public int entityExperienceToNextLvl = 50;
    public int entityHealthPoints = 100;
    public int entityMaxHelath = 100;
    public int gold = 0;
    public int soulsCount = 1;
    

    /*
     * Nazwa
     */
    public void SetName(string name)
    {
        this.entityName = name;
    }
    public string GetName()
    {
        return this.entityName;
    }
    
    /*
     * Poziom doświadczenia
     */
    public void SetLevel(int level)
    {
        this.entityLevel = level;
    }
    public int GetLevel()
    {
        return this.entityLevel;
    }
    
    /*
     * Ilość punktów doświadczenia
     */
    public void SetXp(int xp)
    {
        this.entityExperiencePoints = xp;
    }
    public int GetXp()
    {
        return this.entityExperiencePoints;
    }
    public void AddXp(int xpAmount)
    {
        while (xpAmount > 0)
        {
            int xpToLvlUp = entityExperienceToNextLvl - GetXp();
            
            Debug.Log(xpAmount + "    " + xpToLvlUp); 
            // Jeżeli nie mamy wystaczająco xp do lvl up
            if (xpAmount < xpToLvlUp)
            {
                SetXp( GetXp() + xpAmount );
                break;
            }
            if ( xpToLvlUp <= xpAmount )
            {
                SetLevel( GetLevel() + 1 );
                xpAmount -= xpToLvlUp;
                this.entityExperiencePoints = 0;
            }
        }
    }
    
    /*
     * Punkty życia
     */
    public void SetHp(int hp)
    {
        this.entityHealthPoints = hp;
    }
    public int GetHp()
    {
        return this.entityHealthPoints;
    }
    
    /*
     * Maksymalne punkty życia
     */
    public void SetMaxHp(int maxHp)
    {
        this.entityMaxHelath = maxHp;
    }
    public int GetMaxHp()
    {
        return this.entityMaxHelath;
    }
    
    /*
     * Ilość złota
     */
    public void SetGold(int gold)
    {
        this.gold = gold;
    }
    public int GetGold()
    {
        return this.gold;
    }
    
    /*
     * Ilość dusz
     */
    public void SetSoulsCount(int souls)
    {
        this.soulsCount = souls;
    }
    public int GetSoulsCount()
    {
        return this.soulsCount;
    }
}

using System;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject mainUserInterface;

    private void Awake()
    {
        mainUserInterface = GameObject.Find("Main User Interface");
    }

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
            
            // Jeżeli nie mamy wystaczająco xp do lvl up
            if (xpAmount < xpToLvlUp)
            {
                SetXp( GetXp() + xpAmount );
                break;
            }
            if ( xpToLvlUp <= xpAmount )
            {
                SetLevel( GetLevel() + 1 );
                SetExpToNextLVl(  Convert.ToInt32(Convert.ToDouble(GetExpToNextLVl()) * 1.25 ) );
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
     * Punkty doświadczenia do następnego poziomu
     */
    public void SetExpToNextLVl(int expToNextLvl)
    {
        this.entityExperienceToNextLvl = expToNextLvl;
    }
    public int GetExpToNextLVl()
    {
        return this.entityExperienceToNextLvl;
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
    public void AddGold(int gold)
    {
        this.gold += gold;

        /*
         * Jeśli encja jest graczem, to wyświetlamy złoto w UI
         */
        if ( gameObject.CompareTag("Player") )
        {
            GameObject UiGoldCount = mainUserInterface.transform.Find("Gold/Count").gameObject;
            if (UiGoldCount)
            {
                UiGoldCount.GetComponent<TextMeshProUGUI>().text = Convert.ToString( this.gold );
            }
        }
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
    public void AddSouls(int souls)
    {
        this.soulsCount += soulsCount;

        /*
         * Jeśli encja jest graczem, to wyświetlamy dusze w UI
         */
        if ( gameObject.CompareTag("Player") )
        {
            GameObject UiSoulsCount = mainUserInterface.transform.Find("Souls/Count").gameObject;
            if (UiSoulsCount)
            {
                UiSoulsCount.GetComponent<TextMeshProUGUI>().text = Convert.ToString( this.soulsCount );
            }
        }
    }
}

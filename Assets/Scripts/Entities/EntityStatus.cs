using System;
using System.Collections;
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
    public float entityHealthPoints = 100.0f;
    public float entityMaxHelath = 100.0f;
    public int gold = 0;
    public float AttackDamage = 0;
    public float MovementSpeed = 0;
    public bool isFacedRight = true;
    public bool isEnemy = false;
    public GameObject detectedTarget;
    public float attackRange;
    public Color lightDamageColor;
    public Color heavyDamageColor;
    public Color deathColor;
    private GameObject entityObject;

    private GameObject mainUserInterface;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        mainUserInterface = GameObject.Find("Main User Interface");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
    public void SetHp(float hp)
    {
        this.entityHealthPoints = hp;
    }
    public float GetHp()
    {
        return this.entityHealthPoints;
    }
    
    /*
     * Maksymalne punkty życia
     */
    public void SetMaxHp(float maxHp)
    {
        this.entityMaxHelath = maxHp;
    }
    public float GetMaxHp()
    {
        return this.entityMaxHelath;
    }

    public void DealDamage(float damage)
    {
        if (gameObject.CompareTag("Player"))
        {
            bool isBLocking = gameObject.GetComponent<Player>().isBlocking;
            bool isParrying = gameObject.GetComponent<Player>().isParrying;

            if (isParrying)
            {
                // gracz sparował cios
                float parryingDamageReduction = 0f; // 0 = 100% redukji
                if ( damage * parryingDamageReduction >= GetHp() )
                {
                    /*
                     * Gracz ginie
                     * TODO: animacja śmierci
                     */
                    PlayerDeathEvent();
                } else if (damage * parryingDamageReduction < GetHp())
                {
                    // gracz otrzymuje obrażenia
                    GettingDamageEvent(damage * parryingDamageReduction);
                    
                }
                // odgrywanie animacji po sparowaniu
                gameObject.GetComponent<Animator>().Play("parryAttack");
                
                
            } else if (isBLocking)
            {
                // gracz zablokował cios
                float blockingDamageReduction = 0.6f; // 0.6 = 40% redukji
                if ( damage * blockingDamageReduction >= GetHp() )
                {
                    /*
                     * Gracz ginie
                     * TODO: animacja śmierci
                     */
                    PlayerDeathEvent();
                } else if (damage * blockingDamageReduction < GetHp())
                {
                    // gracz otrzymuje obrażenia
                    GettingDamageEvent(damage * blockingDamageReduction);
                }
            }
            else
            {
                // gracz nie sparował, ani nie zablokował ciosu
                if ( damage >= GetHp() )
                {
                    /*
                     * Gracz ginie
                     * TODO: animacja śmierci
                     */
                    PlayerDeathEvent();
                } else if (damage < GetHp())
                {
                    // gracz otrzymuje obrażenia
                    GettingDamageEvent(damage);
                }
            }
        }
        else
        {
            // Kod dla wszystkich encji poza graczem
            if ( damage >= GetHp() )
            {
                // Encja ginie
                DeathEvent();
            } else if (damage < GetHp())
            {
                // encja otrzymuje obrażenia
                GettingDamageEvent(damage);
            }
        }
    }

    void DeathEvent()
    {
        StartCoroutine(DeathAnimation(deathColor, 0.1f));
    }

    void PlayerDeathEvent()
    {
        StartCoroutine(DeathAnimation(deathColor, 0.1f));
    }
    
    void GettingDamageEvent( float damage)
    {
        SetHp(GetHp() - damage);
            
        if (damage >= ( GetHp() / 2 ) ) StartCoroutine(ChangeColorForInterval(heavyDamageColor, 0.05f));
        else StartCoroutine(ChangeColorForInterval(lightDamageColor, 0.12f));
    }
    
    private IEnumerator ChangeColorForInterval(Color color, float duration)
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = Color.white;
        }
        else
        {
            Debug.Log("Nie wykryto spriteRenderer");
        }
    }
    
    private IEnumerator DeathAnimation(Color color, float duration)
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = Color.white;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Nie wykryto spriteRenderer");
        }
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
     * Obrażenia
     */
    public void SetAttackDamageCount(float AttackDamage)
    {
        this.AttackDamage = AttackDamage;
    }
    public float GetAttackDamageCount()
    {
        return this.AttackDamage;
    }

    /*
     * Prędkość ruchu
     */
    public void SetMovementSpeed(float MovementSpeed)
    {
        this.MovementSpeed = MovementSpeed;
    }
    public float GetMovementSpeed()
    {
        return this.MovementSpeed;
    }
}

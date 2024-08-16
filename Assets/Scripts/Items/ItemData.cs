using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "NeoPunk/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string passiveDescription;
    public string activeDescription;
    public float cooldown;
    public float currentCooldown;
    public string rarity;
    public float minPlayerLvl;
    public Sprite itemIcon;

    public IItemAbility itemAbility;

    // Możemy dodać funkcję do inicjalizacji przedmiotu
    public virtual void Initialize()
    {
        // To zostanie nadpisane przez konkretne klasy przedmiotów
    }

    public interface IItemAbility
    {
        void Use();
        void Apply();
        void Remove();
    }
}
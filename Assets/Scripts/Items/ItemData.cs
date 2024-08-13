using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "NeoPunk/Item")]
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
    
    public interface IItemAbility
    {
        void Use();
        void Apply();
        void Remove();
    }

}
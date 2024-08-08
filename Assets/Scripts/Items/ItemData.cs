using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
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
    public IActiveAbility activeAbility; // interfejs do zdolności aktywnych
    public IPassiveAbility passiveAbility; // interfejs do zdolności pasywnych
    public IRemoveItem removeItem;
    
    public interface IActiveAbility
    {
        void Use();
    }

    public interface IPassiveAbility
    {
        void Apply();
    }
    public interface IRemoveItem
    {
        void Remove();
    }
}
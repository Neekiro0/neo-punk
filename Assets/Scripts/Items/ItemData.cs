using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemData
    {
        private string itemName;
        private string damageType;
        private string description;
        private string lore;
        private double cooldown;
        private double actualCooldown;
        private string rarity;
        private double damage;
        private double minPlayerLvl;
        private string imagePath;

        public ItemData(string itemName, string damageType, string description, string Lore, string rarity, string imagePath = "", double cooldown = 0, double actualCooldown = 0, double damage = 0, double minPlayerLvl = 0 )
        {
            this.itemName = itemName;
            this.damageType = damageType;
            this.description = description;
            this.lore = Lore;
            this.cooldown = cooldown;
            this.actualCooldown = actualCooldown;
            this.imagePath = imagePath;
            this.damage = damage;
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
        public void SetCooldown(double cooldown)
        {
            this.cooldown = cooldown;
        }
        public double GetCooldown()
        {
            return this.cooldown;
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
    }
}


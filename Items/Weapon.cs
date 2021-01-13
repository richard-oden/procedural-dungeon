using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Weapon : Item, IEquippable
    {
        public EquipmentSlot Slot {get; protected set;}
        public int AttackModifier {get; protected set;}
        public Die[] DamageDice {get; protected set;}
        public int DamageModifier {get; protected set;}
        public int Range {get; protected set;}
        public bool IsThrown {get; protected set;}

        public Weapon(string name, double weight, int value, ItemRarity rarity, string description,
            EquipmentSlot slot, Die[] damageDice, int attackMod = 0, int damageMod = 0, int range = 1, 
            bool isThrown = false) 
            : base(name, weight, value, rarity, description)
        {
            Slot = slot;
            DamageDice = damageDice;
            AttackModifier = attackMod;
            DamageModifier = damageMod;
            Range = range;
            IsThrown = isThrown;
        }

        public Weapon(Weapon weaponToClone)
        {
            Name = weaponToClone.Name;
            Weight = weaponToClone.Weight;
            Value = weaponToClone.Value;
            Description = weaponToClone.Description;
            Rarity = weaponToClone.Rarity;
            Location = weaponToClone.Location;
            Slot = weaponToClone.Slot;
            DamageDice = weaponToClone.DamageDice;
            AttackModifier = weaponToClone.AttackModifier;
            DamageModifier = weaponToClone.DamageModifier;
            Range = weaponToClone.Range;
            IsThrown = weaponToClone.IsThrown;
        }

        public override Item GetClone()
        {
            return new Weapon(this);
        }

        public override string GetSecondaryDetails()
        {
            string attackModString = AttackModifier != 0 ? $"Attack Modifier: {AttackModifier} - " : "";
            string damageModString = DamageModifier != 0 ? $" + {DamageModifier}" : "";
            string thrownString = IsThrown ? " (thrown)" : "";
            return $"{Description}\nSlot:{Slot.ToString().FromTitleOrCamelCase()} - {attackModString}Damage: {DamageDice.DiceToString()}{damageModString} - Range: {Range*5} feet{thrownString}";
        }
    }
}
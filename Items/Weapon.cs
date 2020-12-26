using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class Weapon : Item, IEquippable
    {
        public EquipmentSlot Slot {get; protected set;}
        public int AttackModifier {get; protected set;}
        public Die[] DamageDice {get; protected set;}
        public int DamageModifier {get; protected set;}
        public int Range {get; protected set;}

        public Weapon(string name, double weight, int value, ItemRarity rarity, string description,
            EquipmentSlot slot, Die[] damageDice, int attackMod = 0, int damageMod = 0, int range = 1) 
            : base(name, weight, value, rarity, description)
        {
            Slot = slot;
            DamageDice = damageDice;
            AttackModifier = attackMod;
            DamageModifier = damageMod;
            Range = range;
        }

        public override string GetSecondaryDetails()
        {
            string attackModString = AttackModifier != 0 ? $"Attack Modifier: {AttackModifier} - " : "";
            string damageModString = DamageModifier != 0 ? $" + {DamageModifier}" : "";
            return $"{Description}\nSlot:{Slot.ToString().FromTitleOrCamelCase()} - {attackModString}Damage: {DamageDice.DiceToString()}{damageModString} - Range: {Range*5} feet";
        }
    }
}
namespace ProceduralDungeon
{
    public class Armor : Item, IEquippable
    {
        public EquipmentSlot Slot {get; protected set;}
        public int ArmorClassBonus {get; protected set;}
        public int DamageResistance {get; protected set;}
        public Armor(string name, double weight, int value, ItemRarity rarity, string description,
            EquipmentSlot slot, int acBonus, int damageResistance = 0) 
            : base(name, weight, value, rarity, description)
        {
            Slot = slot;
            ArmorClassBonus = acBonus;
            DamageResistance = damageResistance;
        }

        public override string GetSecondaryDetails()
        {
            string damageResistanceString = DamageResistance != 0 ? $" - Damage Resistance: {DamageResistance}" : "";
            return $"{Description}\nSlot:{Slot.ToString().FromTitleOrCamelCase()} - AC Bonus: {ArmorClassBonus}{damageResistanceString}";
        }
    }
}
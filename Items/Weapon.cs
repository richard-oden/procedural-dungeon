namespace ProceduralDungeon
{
    public class Weapon : Item, IEquippable
    {
        public EquipmentSlot Slot {get; protected set;}
        public int AttackModifier {get; protected set;}
        public Die[] DamageDice {get; protected set;}
        public int DamageModifier {get; protected set;}
        public int Range {get; protected set;}

        public Weapon(string name, double weight, int value, string description,
            EquipmentSlot slot, Die[] damageDice, int attackMod = 0, int damageMod = 0, int range = 1) 
            : base(name, weight, value, description)
        {
            Slot = slot;
            DamageDice = damageDice;
            AttackModifier = attackMod;
            DamageModifier = damageMod;
            Range = range;
        }
    }
}
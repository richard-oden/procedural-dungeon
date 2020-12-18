namespace ProceduralDungeon
{
    public class Weapon : Item, Equippable
    {
        public EquipmentSlot Slot {get; protected set;}
        public int AttackModifier {get; protected set;}
        public Die[] DamageDice {get; protected set;}
        public int DamageModifier {get; protected set;}
        public int Range {get; protected set;}

        public Weapon() : base()
        {

        }
    }
}
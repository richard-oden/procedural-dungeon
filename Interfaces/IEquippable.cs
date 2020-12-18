namespace ProceduralDungeon
{
    public interface Equippable
    {
        EquipmentSlot Slot {get;}
    }

    public enum EquipmentSlot
    {
        Head,
        Chest,
        Hands,
        Legs,
        Feet,
        Neck,
        Ring,
        OneHanded,
        TwoHanded
    }
}
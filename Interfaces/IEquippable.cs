using System;

namespace ProceduralDungeon
{
    public interface IEquippable
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
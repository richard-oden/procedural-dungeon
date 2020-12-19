using static ProceduralDungeon.Dice;

namespace ProceduralDungeon
{
    public static class ItemsRepository
    {
        public static readonly Item[] CommonMisc = new Item[]
        {
            new Item("Tattered Parchment",  weight: 0.5,    value: 0, 
                "Maybe it once held a powerful spell. Or maybe it's a really old piece of toilet paper."),
            new Item("Pottery Shard",       weight: 0.5,    value: 0, 
                "What ancient civilization could have created this?"),
            new Item("Rusty Pickaxe",       weight: 10,     value: 5, 
                "Doesn't seem to be usable anymore, although you could sell it as scrap metal."),
            new Item("Humanoid Skull",      weight: 3,      value: 2, 
                "Hard to tell what race it is. Might be worth a little money for the right buyer."),
            new Item("Leather Scraps",      weight: 0.7,    value: 0, 
                "It may have been part of a fancy purse at some point."),
            new Item("Large Femur",         weight: 4,      value: 2, 
                "This must have belonged to some giant creature."),
            new Item("Dried up inkwell",    weight: 0.1,    value: 0, 
                "What a waste of good ink."),
            new Item("Broken arrow",        weight: 0.1,    value: 0, 
                "It appears to have broken upon impact."),  
        };

        public static readonly Item[] UncommonMisc = new Item[]
        {
            new Item("Silver Ring",         weight: 0.1,    value: 20, 
                "A simple silver band, nothing too fancy."),
            new Item("Ancient Tome",        weight: 8,      value: 22, 
                "It's just a really old book. Has to be worth something, right?"),
            new Item("Wooden Figurine",     weight: 0.5,    value: 8, 
                "It could be a religious icon. Or maybe a children's toy."),
            new Item("Ornate Vase",         weight: 5,      value: 26, 
                "A fancy procelain vase. Nothing inside."),
            new Item("Bundle of leather",   weight: 8,      value: 30, 
                "A bundle of tanned skins bound with hemp rope."),
            new Item("Sack of coal",        weight: 14,     value: 12, 
                "A heavy burlap sack full of coal."),
            new Item("Iron ingot",          weight: 20,     value: 35, 
                "An iron ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Large Cauldron",      weight: 40,     value: 25, 
                "Good luck lugging this thing around.")    
        };

        public static readonly Item[] RareMisc = new Item[]
        {
            new Item("Gold Necklace",      weight: 0.5,     value: 50, 
                "A simple chain necklace. Feels pretty heavy for its size."),
            new Item("Silver ingot",       weight: 23,      value: 55, 
                "A silver ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Fur Rug",            weight: 30,      value: 45, 
                "A large white fur rug. The head of a polar bear is still attached."),
            new Item("Set of tarot cards", weight: 0.1,     value: 30, 
                "They carry intricate designs made of gold and silver ink.")
        };

        public static readonly Item[] VeryRareMisc = new Item[]
        {
            new Item("Jewel-inlaid Skull", weight: 5,       value: 120, 
                "Each eye socket contains a large ruby, and some of the teeth are replaced with gems. Creepy."),
            new Item("Gold Ingot",         weight: 60,      value: 140, 
                "A gold ingot used in blacksmithing."),
            new Item("Arcane Mannacles",   weight: 20,      value: 160, 
                "A set of steel mannacles inscribed with various glowing runes.")
        };

        public static readonly Item[] Utilities = new Item[]
        {
            new Item("Leather Satchel",     weight: 0,      value: 250, 
                "This should help you carry more things. It's also very stylish."),
            new Item("Floor Map",           weight: 0.2,    value: 100, 
                "It contains some notable locations on this floor."),
            new Item("Beast Bane Incense",  weight: 0.4,    value: 80, 
                "It emits a strong musky smell when ignited. Should temporarily deter hostile animals."), 
            new Item("Annointing Oil",      weight: 0.4,    value: 80, 
                "A glass vial filled with an amber-colored oil. Should temporarily deter the undead."),
        };

        public static readonly Item[] CommonWeapons = new Weapon[]
        {
            new Weapon("Worn Iron Waraxe",          weight: 20, value: 25,
                "This old iron axe has seen a lot of battle.",
                EquipmentSlot.OneHanded, new Die[]{D8}),
            new Weapon("Rusted Iron Shortsword",    weight: 20, value: 25,
                "It hasn't rusted all the way through yet.",
                EquipmentSlot.OneHanded, new Die[]{D6}),
            new Weapon("Wooden Maul",               weight: 30, value: 20,
                "It's a big piece of wood. Simple yet effective.",
                EquipmentSlot.TwoHanded, new Die[]{D4, D4}),
            new Weapon("Leather Sling",             weight: 3, value: 15,
                "A long strip of hemp cord with a leather pad in the middle. You can use it to throw stuff.",
                EquipmentSlot.OneHanded, new Die[]{D6}, range: 6),
            new Weapon("Crude Shortbow",            weight: 4, value: 30,
                "It's not the most well-made bow, but should get the job done.",
                EquipmentSlot.TwoHanded, new Die[]{D4, D4}, range: 8),
        };
    }
}
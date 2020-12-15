namespace ProceduralDungeon
{
    public static class ItemsRepository
    {
        public static readonly Item[] CommonMisc = new Item[]
        {
            new Item("Tattered Parchment",  0.5, 0, 
                "Maybe it once held a powerful spell. Or maybe it's a really old piece of toilet paper."),
            new Item("Pottery Shard",       0.5, 0, 
                "What ancient civilization could have created this?"),
            new Item("Rusty Pickaxe",       10, 5, 
                "Doesn't seem to be usable anymore, although you could sell it as scrap metal."),
            new Item("Humanoid Skull",      3, 2, 
                "Hard to tell what race it is. Might be work a little money for the right buyer."),
            new Item("Leather Scraps",      0.7, 0, 
                "It may have been part of a fancy purse at some point."),
            new Item("Large Femur",         4, 2, 
                "This must have belonged to some giant creature."),
            new Item("Dried up inkwell",    0.1, 0, 
                "What a waste of good ink."),
            new Item("Broken arrow",        0.1, 0, 
                "It appears to have broken upon impact."),  
        };

        public static readonly Item[] UncommonMisc = new Item[]
        {
            new Item("Silver Ring",         0.1, 20, 
                "A simple silver band, nothing too fancy."),
            new Item("Ancient Tome",        8, 22, 
                "It's just a really old book. Has to be worth something, right?"),
            new Item("Wooden Figurine",     0.5, 8, 
                "It could be a religious icon. Or maybe a children's toy."),
            new Item("Ornate Vase",         5, 26, 
                "A fancy procelain vase. Nothing inside."),
            new Item("Bundle of leather",   8, 30, 
                "A bundle of tanned skins bound with hemp rope."),
            new Item("Sack of coal",        14, 12, 
                "A heavy burlap sack full of coal."),
            new Item("Iron ingot",          20, 35, 
                "An iron ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Large Cauldron",      40, 25, 
                "Good luck lugging this thing around.")    
        };

        public static readonly Item[] RareMisc = new Item[]
        {
            new Item("Gold Necklace",      0.5, 50, 
                "A simple chain necklace. Feels pretty heavy for its size."),
            new Item("Silver ingot",       23, 55, 
                "A silver ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Fur Rug",            30, 45, 
                "A large white fur rug. The head of a polar bear is still attached."),
            new Item("Set of tarot cards", 0.1, 30, 
                "They carry intricate designs made of gold and silver ink.")
        };

        public static readonly Item[] VeryRareMisc = new Item[]
        {
            new Item("Jewel-inlaid Skull", 5, 120, 
                "Each eye socket contains a large ruby, and some of the teeth are replaced with gems. Creepy."),
            new Item("Gold Ingot",         60, 140, 
                "A gold ingot used in blacksmithing."),
            new Item("Arane Mannacles",    20, 160, 
                "A set of steel mannacles inscribed with various glowing runes.")
        };

        public static readonly Item[] Utilities = new Item[]
        {
            new Item("Leather Satchel",      0, 250, 
                "This should help you carry more things. It's also very stylish."),
            new Item("Floor Map",           0.2, 100, 
                "It contains some notable locations on this floor."),
            new Item("Beast Bane Incense",  0.4, 80, 
                "It emits a strong musky smell when ignited. Should temporarily deter hostile animals."), 
            new Item("Annointing Oil",      0.4, 80, 
                "A glass vial filled with an amber-colored oil. Should temporarily deter the undead."),
        };
    }
}
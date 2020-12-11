namespace ProceduralDungeon
{
    public static class ItemsRepository
    {
        public static readonly Item[] Commons = new Item[]
        {
            new Item("Tattered Parchment",  001, 0.5, 0, 
                "Maybe it once held a powerful spell. Or maybe it's a really old piece of toilet paper."),
            new Item("Pottery Shard",       002, 0.5, 0, 
                "What ancient civilization could have created this?"),
            new Item("Rusty Pickaxe",       003, 10, 5, 
                "Doesn't seem to be usable anymore, although you could sell it as scrap metal."),
            new Item("Humanoid Skull",      004, 3, 2, 
                "Hard to tell what race it is. Might be work a little money for the right buyer."),
            new Item("Leather Scraps",      005, 0.7, 0, 
                "It may have been part of a fancy purse at some point."),
            new Item("Large Femur",         006, 4, 2, 
                "This must have belonged to some giant creature."),
            new Item("Dried up inkwell",    007, 0.1, 0, 
                "What a waste of good ink."),
            new Item("Broken arrow",        008, 0.1, 0, 
                "It appears to have broken upon impact."),  
        };
    }
}
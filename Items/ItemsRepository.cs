using System.Collections.Generic;
using System.Linq;

using static ProceduralDungeon.Dice;

namespace ProceduralDungeon
{
    public static class ItemsRepository
    {
        public static readonly Item[] All = new Item[]
        {
            new Item("Tattered Parchment",  weight: 0.5,    value: 0,   rarity: ItemRarity.Common,
                "Maybe it once held a powerful spell. Or maybe it's a really old piece of toilet paper."),
            new Item("Pottery Shard",       weight: 0.5,    value: 0,   rarity: ItemRarity.Common, 
                "What ancient civilization could have created this?"),
            new Item("Rusty Pickaxe",       weight: 10,     value: 5,   rarity: ItemRarity.Common, 
                "Doesn't seem to be usable anymore, although you could sell it as scrap metal."),
            new Item("Humanoid Skull",      weight: 3,      value: 2,   rarity: ItemRarity.Common, 
                "Hard to tell what race it is. Might be worth a little money for the right buyer."),
            new Item("Leather Scraps",      weight: 0.7,    value: 0,   rarity: ItemRarity.Common, 
                "It may have been part of a fancy purse at some point."),
            new Item("Large Femur",         weight: 4,      value: 2,   rarity: ItemRarity.Common, 
                "This must have belonged to some giant creature."),
            new Item("Dried up inkwell",    weight: 0.1,    value: 0,   rarity: ItemRarity.Common,
                "What a waste of good ink."),
            new Item("Broken arrow",        weight: 0.1,    value: 0,   rarity: ItemRarity.Common,
                "It appears to have broken upon impact."),
            new Item("Silver Ring",         weight: 0.1,    value: 20,    rarity: ItemRarity.Uncommon,
                "A simple silver band, nothing too fancy."),
            new Item("Ancient Tome",        weight: 8,      value: 22,    rarity: ItemRarity.Uncommon,
                "It's just a really old book. Has to be worth something, right?"),
            new Item("Wooden Figurine",     weight: 0.5,    value: 8,     rarity: ItemRarity.Uncommon, 
                "It could be a religious icon. Or maybe a children's toy."),
            new Item("Ornate Vase",         weight: 5,      value: 26,    rarity: ItemRarity.Uncommon, 
                "A fancy procelain vase. Nothing inside."),
            new Item("Bundle of leather",   weight: 8,      value: 30,    rarity: ItemRarity.Uncommon, 
                "A bundle of tanned skins bound with hemp rope."),
            new Item("Sack of coal",        weight: 14,     value: 12,    rarity: ItemRarity.Uncommon, 
                "A heavy burlap sack full of coal."),
            new Item("Iron ingot",          weight: 20,     value: 35,    rarity: ItemRarity.Uncommon, 
                "An iron ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Large Cauldron",      weight: 40,     value: 25,    rarity: ItemRarity.Uncommon, 
                "Good luck lugging this thing around."),
            new Item("Gold Necklace",      weight: 0.5,     value: 50,    rarity: ItemRarity.Rare, 
                "A simple chain necklace. Feels pretty heavy for its size."),
            new Item("Silver ingot",       weight: 23,      value: 55,    rarity: ItemRarity.Rare,
                "A silver ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Fur Rug",            weight: 30,      value: 45,    rarity: ItemRarity.Rare, 
                "A large white fur rug. The head of a polar bear is still attached."),
            new Item("Set of tarot cards", weight: 0.1,     value: 30,    rarity: ItemRarity.Rare, 
                "They carry intricate designs made of gold and silver ink."),
            new Item("Jewel-inlaid Skull", weight: 5,       value: 120,    rarity: ItemRarity.VeryRare, 
                "Each eye socket contains a large ruby, and some of the teeth are replaced with gems. Creepy."),
            new Item("Gold Ingot",         weight: 60,      value: 140,    rarity: ItemRarity.VeryRare, 
                "A gold ingot used in blacksmithing."),
            new Item("Arcane Mannacles",   weight: 20,      value: 160,    rarity: ItemRarity.VeryRare, 
                "A set of steel mannacles inscribed with various glowing runes."),
            new Weapon("Worn Iron Waraxe",          weight: 20, value: 25,   rarity: ItemRarity.Common,
                "This old iron axe has seen a lot of battle.",
                EquipmentSlot.OneHanded, new Die[]{D8}),
            new Weapon("Rusted Iron Shortsword",    weight: 20, value: 25,   rarity: ItemRarity.Common,
                "It hasn't rusted all the way through yet.",
                EquipmentSlot.OneHanded, new Die[]{D6}),
            new Weapon("Wooden Maul",               weight: 30, value: 20,   rarity: ItemRarity.Common,
                "It's a big piece of wood. Simple yet effective.",
                EquipmentSlot.TwoHanded, new Die[]{D4, D4}),
            new Weapon("Decent Iron Pickaxe",       weight: 10, value: 15,   rarity: ItemRarity.Common,
                "It's simple, yet reliable. Good for mining coal and bashing skulls.",
                EquipmentSlot.TwoHanded, new Die[]{D6}),
            new Weapon("Decent Iron Hoe",           weight: 8, value: 12,   rarity: ItemRarity.Common,
                "While it's meant for tilling soil, it's not bad as an improvised weapon.",
                EquipmentSlot.TwoHanded, new Die[]{D6}),
            new Weapon("Leather Sling",             weight: 3, value: 15,   rarity: ItemRarity.Common,
                "A long strip of hemp cord with a leather pad in the middle. You can use it to throw stuff.",
                EquipmentSlot.OneHanded, new Die[]{D6}, range: 6),
            new Weapon("Crude Shortbow",            weight: 4, value: 30,   rarity: ItemRarity.Common,
                "It's not the most well-made bow, but should get the job done.",
                EquipmentSlot.TwoHanded, new Die[]{D4, D4}, range: 8),
            new Weapon("Staff of The Destroyer",    weight: 10, value: 12000,    rarity: ItemRarity.VeryRare,
                "This fabled implement has laid waste to armies and destroyed nations. It also looks pretty.",
                EquipmentSlot.TwoHanded, new Die[]{D12, D12, D12, D12, D8, D8, D4, D4, D4},
                attackMod: 8, damageMod: 8, range: 100),
            new Armor("Shabby Leather Vest",        weight: 20, value: 15,   rarity: ItemRarity.Common,
                "It might stop a few spitballs.",
                EquipmentSlot.Chest, acBonus: 1),
            new Armor("Battered Wooden Shield",     weight: 8, value: 10,   rarity: ItemRarity.Common,
                "This would also make a very rustic serving platter.",
                EquipmentSlot.OneHanded, acBonus: 1),
            new Armor("Mithril Chestplate",        weight: 18, value: 10000,    rarity: ItemRarity.VeryRare,
                "Although lightweight, it will stop all but the mightiest of blows.",
                EquipmentSlot.Chest, acBonus: 10, damageResistance: 10),
            new Repellant("Beast Bane Incense", CreatureCategory.Beast, 30,
                "It emits a strong musky smell when ignited. Should temporarily deter hostile animals."), 
            new Repellant("Annointing Oil", CreatureCategory.Undead, 30, 
                "A glass vial filled with an amber-colored oil. Should temporarily deter the undead."),
            new Potion("Minor Healing", new Die[]{D4, D4}, ItemRarity.Common),
            new Potion("Common Healing", new Die[]{D6, D6}, ItemRarity.Common),
            new Potion("Major Healing", new Die[]{D8, D8}, ItemRarity.Uncommon),
            new Potion("Greater Healing", new Die[]{D12, D12}, ItemRarity.Rare),
            new Potion("Superior Healing", new Die[]{D20, D20}, ItemRarity.Rare),
            new Potion("Wound Regeneration", new Die[]{D20, D20, D20}, ItemRarity.VeryRare),
            new Food("Common Ration", 2, 200, ItemRarity.Common,
                "A selection of smoked meats, cheeses, bread, and dried fruit wrapped in an oilskin."),
            new Food("Hardy Ration", 2, 500, ItemRarity.Uncommon,
                "A selection of jerky, aged cheese, hardtack, and nuts encased in tin. This has a longer shelflife than normal."),
            new Food("Fine Ration", 3, 200, ItemRarity.Uncommon,
                "A selection of artisanal sausages, fancy cheese, bread, and imported preserved fruit wrapped in an oilskin."),
            new Food("Farmer's Meal", 4, 100, ItemRarity.Uncommon,
                "A homecooked meal of fresh meats, breads, fruits, and vegetables. Very nutritous, but doesn't travel well."),
       };

        public static readonly Weapon[] Weapons = All.Where(i => i is Weapon).Cast<Weapon>().ToArray();
        public static readonly Armor[] Armor = All.Where(i => i is Armor).Cast<Armor>().ToArray();
        public static readonly Item[] Commons = All.Where(i => i.Rarity == ItemRarity.Common).ToArray();
        public static readonly Item[] Uncommons = All.Where(i => i.Rarity == ItemRarity.Uncommon).ToArray();
        public static readonly Item[] Rares = All.Where(i => i.Rarity == ItemRarity.Rare).ToArray();
        public static readonly Item[] VeryRares = All.Where(i => i.Rarity == ItemRarity.VeryRare).ToArray();
        public static readonly Item[] Junk = Commons.Where(i => !(i is Weapon) && !(i is Armor)).ToArray();

        public static Item[] GetMapDependantItems(Map map)
        {
            return new Item[]
            {
                new FloorMap(map),
                new Compass((Door)map.Assets.Single(a => a is Door)),
            };
        }

        public static Item GetByName(string name)
        {
            return (Item)All.GetByName(name);
        }
    }
}
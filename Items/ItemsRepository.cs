using System.Collections.Generic;
using System.Linq;

using static ProceduralDungeon.Dice;

namespace ProceduralDungeon
{
    public static class ItemsRepository
    {
        public static readonly Item[] All = new Item[]
        {
            new Item("Ruined Robe",         weight: 0.5,    value: 0,   rarity: ItemRarity.Common, 
                "It's horribly stained, frayed, and ripped in several places."),
            new Item("Tattered Parchment",  weight: 0.1,    value: 0,   rarity: ItemRarity.Common,
                "Maybe it once held a powerful spell. Or maybe it's a really old piece of toilet paper."),
            new Item("Pottery Shard",       weight: 0.1,    value: 0,   rarity: ItemRarity.Common, 
                "What ancient civilization could have created this?"),
            new Item("Wooden Cup",          weight: 0.2,    value: 2,   rarity: ItemRarity.Common, 
                "It's a wooden cup. Pretty exciting, I know."),
            new Item("Empty Vial",          weight: 0.2,    value: 2,   rarity: ItemRarity.Common, 
                "It's made of glass and the stopper is missing. You can see some dried remnants of whatever liquid was inside."),
            new Item("Wooden Plate",        weight: 0.3,    value: 2,   rarity: ItemRarity.Common, 
                "Alternatively, this could be an ancient frisbee."),
            new Item("Rusty Pickaxe",       weight: 8,     value: 5,   rarity: ItemRarity.Common, 
                "Doesn't seem to be usable anymore, although you could sell it as scrap metal."),
            new Item("Humanoid Skull",      weight: 3,      value: 2,   rarity: ItemRarity.Common, 
                "Hard to tell what race it is. Might be worth a little money for the right buyer."),
            new Item("Leather Scraps",      weight: 0.7,    value: 0,   rarity: ItemRarity.Common, 
                "It may have been part of a fancy purse at some point."),
            new Item("Large Femur",         weight: 4,      value: 1,   rarity: ItemRarity.Common, 
                "This must have belonged to some giant creature."),
            new Item("Dried up inkwell",    weight: 0.1,    value: 0,   rarity: ItemRarity.Common,
                "What a waste of good ink."),
            new Item("Broken Arrow",        weight: 0.1,    value: 0,   rarity: ItemRarity.Common,
                "It appears to have broken upon impact."),
            new Item("Rusted Chain",        weight: 6,      value: 1,   rarity: ItemRarity.Common,
                "It's too rusty to use now. Maybe it could be melted down as scrap."),
            new Item("Damaged Tin Pot",     weight: 1.2,    value: 1,   rarity: ItemRarity.Common,
                "It's heavily dented and the bottom is scorched from use."),
            new Item("Tin Fork",            weight: 0.1,    value: 1,   rarity: ItemRarity.Common,
                "It's pretty old, but still perfectly functional. Good for eating spaghetti."),
            new Item("Wooden Spoon",        weight: 0.1,    value: 1,   rarity: ItemRarity.Common,
                "Who knows whose mouth this has been inside? Gross."),
            new Weapon("Butcher's Knife",   weight: 0.1,    value: 3,   rarity: ItemRarity.Common,
                "You could use it as a weapon, but you'd probably be better off using your bare hands",
                EquipmentSlot.OneHanded, new Die[] {Coin}),
            new Item("Broken Glasses",      weight: 0.1,    value: 0,   rarity: ItemRarity.Common,
                "One of the lenses is missing and the frame is bent."),
            new Item("Ivory Chess Piece",   weight: 0.1,    value: 6,   rarity: ItemRarity.Common,
                "It's a bishop. That fact that it's ivory makes it somewhat valuable. Where could the rest be?"),
            new Food("Moldy Bread", 1, 100, ItemRarity.Common,
                "Maybe you could pick off the mold and eat the rest?"),
            new Item("Castiron Lock",        weight: 1,      value: 1,   rarity: ItemRarity.Common,
                "It's locked and the key is nowhere to be found."),

            new Item("Hempen Rope",         weight: 20,     value: 10,   rarity: ItemRarity.Uncommon,
                "You never know when you might need some rope. It's heavy, but appears to be in good condition."),           
            new Item("Silver Ring",         weight: 0.1,    value: 20,    rarity: ItemRarity.Uncommon,
                "A simple silver band, nothing too fancy."),
            new Item("Ancient Tome",        weight: 8,      value: 22,    rarity: ItemRarity.Uncommon,
                "It's just a really old book. Has to be worth something, right?"),
            new Item("Wooden Figurine",     weight: 0.5,    value: 7,     rarity: ItemRarity.Uncommon, 
                "It could be a religious icon. Or maybe a children's toy."),
            new Item("Ornate Vase",         weight: 5,      value: 26,    rarity: ItemRarity.Uncommon, 
                "A fancy procelain vase. Nothing inside."),
            new Item("Bundle of leather",   weight: 8,      value: 21,    rarity: ItemRarity.Uncommon, 
                "A bundle of tanned skins bound with hemp rope."),
            new Item("Sack of coal",        weight: 14,     value: 8,    rarity: ItemRarity.Uncommon, 
                "A heavy burlap sack full of coal."),
            new Item("Iron ingot",          weight: 20,     value: 35,    rarity: ItemRarity.Uncommon, 
                "An iron ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Large Cauldron",      weight: 40,     value: 25,    rarity: ItemRarity.Uncommon,
                "Good luck lugging this thing around."),
            new Food("Bottle of Wine", 2, 800, ItemRarity.Uncommon,
                "The label is worn and it's dark red inside. Uncorking the bottle gives off a strong smell of alcohol and cherries."),
            new Item("Small Leather Pouch", weight: .4,      value: 9,    rarity: ItemRarity.Uncommon, 
                "Useful for holding small items."),
            new Item("Iron Crowbar",        weight: .7,      value: 12,   rarity: ItemRarity.Uncommon, 
                "Useful for prying open boxes."),
            new Item("Tinderbox",           weight: .1,      value: 10,   rarity: ItemRarity.Uncommon, 
                "Good for starting fires in a pinch."),
            new Item("Hourglass",           weight: 0.3,     value: 13,   rarity: ItemRarity.Uncommon, 
                "It's probably more like a minuteglass."),

            new Item("Gold Necklace",      weight: 0.5,     value: 50,    rarity: ItemRarity.Rare, 
                "A simple chain necklace. Feels pretty heavy for its size."),
            new Item("Silver ingot",       weight: 23,      value: 55,    rarity: ItemRarity.Rare,
                "A silver ingot used in blacksmithing. It's pretty tarnished."),
            new Item("Fur Rug",            weight: 30,      value: 45,    rarity: ItemRarity.Rare, 
                "A large white fur rug. The head of a polar bear is still attached."),
            new Item("Set of tarot cards", weight: 0.1,     value: 30,    rarity: ItemRarity.Rare, 
                "They carry intricate designs made of gold and silver ink."),
            new Item("Holy Symbol",        weight: 0.3,     value: 28,    rarity: ItemRarity.Rare, 
                "You don't recognize the deity it's devoted to, but it's very well made."),
            

            new Item("Jewel-inlaid Skull", weight: 5,       value: 120,   rarity: ItemRarity.VeryRare, 
                "Each eye socket contains a large ruby, and some of the teeth are replaced with gems. Creepy."),
            new Item("Gold Ingot",         weight: 60,      value: 140,   rarity: ItemRarity.VeryRare, 
                "A gold ingot used in blacksmithing."),
            new Item("Arcane Mannacles",   weight: 20,      value: 160,   rarity: ItemRarity.VeryRare, 
                "A set of steel mannacles inscribed with various glowing runes."),
            new Item("Mithril Scale",      weight: 4,       value: 80,    rarity: ItemRarity.VeryRare, 
                "It's used to weigh jewels or other small items. The craftsmanship is beautiful."),
            new Item("Crystal Orb",        weight: 14,      value: 96,    rarity: ItemRarity.VeryRare, 
                "The inside pulses with pale blue light. It must be an arcane implement of some type."),
            new Item("Platinum Flask",     weight: 0.3,     value: 74,    rarity: ItemRarity.VeryRare, 
                "While its body is entirely platinum, the cap is made of rose gold. It carries an enscription in a language you don't understand."),
            
            new Weapon("Worn Iron Waraxe",          weight: 20, value: 25,   rarity: ItemRarity.Common,
                "This old iron axe has seen a lot of battle.",
                EquipmentSlot.OneHanded, new Die[]{D8}),
            new Weapon("Rusted Iron Shortsword",    weight: 20, value: 25,   rarity: ItemRarity.Common,
                "It hasn't rusted all the way through yet.",
                EquipmentSlot.OneHanded, new Die[]{D6}),
            new Weapon("Shoddy Bone Dagger",        weight: 2, value: 10,   rarity: ItemRarity.Common,
                "It's very poorly made but still quite sharp.",
                EquipmentSlot.OneHanded, new Die[]{D4}),
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
            new Weapon("Alchemist's Fire",          weight: .5, value: 25,   rarity: ItemRarity.Uncommon,
                "It's a small clay vessel filled with highly flammable liquid.",
                EquipmentSlot.OneHanded, new Die[]{D6}, damageMod: 2, range: 4, isThrown: true),
            new Weapon("Staff of The Destroyer",    weight: 10, value: 12000,    rarity: ItemRarity.VeryRare,
                "This fabled implement has laid waste to armies and destroyed nations. It also looks pretty.",
                EquipmentSlot.TwoHanded, new Die[]{D12, D12, D12, D12, D8, D8, D4, D4, D4},
                attackMod: 8, damageMod: 8, range: 100),
            
            new Armor("Wide Brimmed Hat",           weight: .3, value: 3,    rarity: ItemRarity.Common,
                "Although useful for keeping the sun at bay, it doesn't offer any protection against attacks.",
                EquipmentSlot.Head, acBonus: 0),
            new Armor("Shabby Leather Vest",        weight: 20, value: 15,   rarity: ItemRarity.Common,
                "It might stop a few spitballs.",
                EquipmentSlot.Chest, acBonus: 2),
            new Armor("Damaged Wooden Shield",     weight: 8, value: 9,   rarity: ItemRarity.Common,
                "This would also make a very rustic serving platter.",
                EquipmentSlot.OneHanded, acBonus: 1),
            new Armor("Shoddy Leather Bracers",     weight: 1.3, value: 7,   rarity: ItemRarity.Common,
                "They're not constructed very well, but beggars can't be choosers, right?",
                EquipmentSlot.Hands, acBonus: 1),
            new Armor("Worn Leather Boots",     weight: 1.4, value: 6,   rarity: ItemRarity.Common,
                "There are some holes in the front and the sole is peeling off of one, but they're better than nothing.",
                EquipmentSlot.Feet, acBonus: 1),
            new Armor("Battered Leather Helmet",     weight: 1, value: 8,   rarity: ItemRarity.Common,
                "It's definitely seen some battle.",
                EquipmentSlot.Head, acBonus: 1),
            new Armor("Old Leather Greaves",     weight: 2, value: 10,   rarity: ItemRarity.Common,
                "They cover the lower half of your legs.",
                EquipmentSlot.Legs, acBonus: 1),
            new Armor("Mithril Chestplate",        weight: 18, value: 10000,    rarity: ItemRarity.VeryRare,
                "Although lightweight, it will stop all but the mightiest of blows.",
                EquipmentSlot.Chest, acBonus: 10, damageResistance: 10),

            new Repellant("Beast Bane Incense", CreatureCategory.Beast, 30,
                "It emits a strong musky smell when ignited. Should temporarily deter hostile animals."), 
            new Repellant("Annointing Oil", CreatureCategory.Undead, 30, 
                "A glass vial filled with an amber-colored oil. Should temporarily deter the undead."),
            new Repellant("Anti-Monster Aroma", CreatureCategory.Monstrosity, 30, 
                "A small, pale-green bottle of perfume. The cap is a stylized dragon head. Should temporarily deter monsters."),

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
            return ((Item)All.GetByName(name)).GetClone();
        }
    }
}
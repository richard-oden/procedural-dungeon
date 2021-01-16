using System;
using System.Linq;
using System.Collections.Generic;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public static class ChestsRepository
    {
        public static List<Chest> All => new List<Chest>()
        {
            new Chest("Wooden Chest", averageGold: 30, "A simple wooden chest without much decoration.",
                ItemsRepository.ChestItems.Where(i => i.Rarity == ItemRarity.Common || i.Rarity == ItemRarity.Uncommon).ToList()),
            new Chest("Iron Chest", averageGold: 60, "A sturdy wooden chest reinforced with various iron pieces.",
                ItemsRepository.ChestItems.Where(i => i.Rarity == ItemRarity.Uncommon || i.Rarity == ItemRarity.Rare).ToList()),
            new Chest("Gold Chest", averageGold: 90, "An ornately decorated chest. Much of its design is made of gold.",
                ItemsRepository.ChestItems.Where(i => i.Rarity == ItemRarity.Rare || i.Rarity == ItemRarity.VeryRare).ToList()),
            new Chest("Mithril Chest", averageGold: 120, "A simple, yet beautiful chest made of mithril.",
                ItemsRepository.ChestItems.Where(i => i.Rarity == ItemRarity.VeryRare).ToList()),
            new Chest("Mithril Chest", averageGold: 150, "A simple, yet beautiful chest made of mithril.",
                ItemsRepository.ChestItems.Where(i => i.Rarity == ItemRarity.VeryRare).ToList()),
        };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class PlayerBackground
    {
        public string Name {get; protected set;}
        public int StrengthMod {get; protected set;}
        public int EnduranceMod {get; protected set;}
        public int PerceptionMod {get; protected set;}
        public int CharismaMod {get; protected set;}
        public int StartingGold {get; protected set;}
        public List<Item> Inventory {get; protected set;}

        public PlayerBackground(string name, int startingGold, List<Item> inventory,
            int strengthMod = 0, int enduranceMod = 0, int perceptionMod = 0, int charismaMod = 0)
        {
            Name = name;
            StrengthMod = strengthMod;
            EnduranceMod = enduranceMod;
            PerceptionMod = perceptionMod;
            CharismaMod = charismaMod;
            StartingGold = startingGold;
            Inventory = inventory;
        }
    }

    public static class PlayerBackgrounds
    {
        public static readonly PlayerBackground Noble = new PlayerBackground("Noble", startingGold: 200, 
            inventory: new List<Item>(){ItemsRepository.GetByName("Fine Ration")}); // Fine clothing, 200, but no mods
        
        public static readonly PlayerBackground Miner = new PlayerBackground("Miner", startingGold: 50, strengthMod: 2,
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Decent Iron Pickaxe"), 
                ItemsRepository.GetByName("Hardy Ration")
            }
        ); // Tattered clothing, pickaxe, lantern, little money, increased strength
        
        public static readonly PlayerBackground Farmer = new PlayerBackground("Farmer", startingGold: 50, enduranceMod: 2, 
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Decent Iron Hoe"), 
                ItemsRepository.GetByName("Farmer's Meal")
            }
        ); // Tattered clothing, hoe, food, little money, increased endurance
        
        public static readonly PlayerBackground Hunter = new PlayerBackground("Hunter", startingGold: 50,  perceptionMod: 2,
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Crude Shortbow"), 
                ItemsRepository.GetByName("Beast Bane Incense"), 
                ItemsRepository.GetByName("Common Ration")
            }
        ); // Decent clothing, bow, beast bane, little money, increased perception
        
        public static readonly PlayerBackground Priest = new PlayerBackground("Priest", startingGold: 50, charismaMod: 2, 
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Annointing Oil"), 
                ItemsRepository.GetByName("Common Ration")
            }
        ); // Decent clothing, morning star, annointing oil, little money, increased charisma
    }
}
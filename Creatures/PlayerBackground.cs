using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class PlayerBackground
    {
        public string Name {get; private set;}
        public int StrengthMod {get; private set;}
        public int EnduranceMod {get; private set;}
        public int PerceptionMod {get; private set;}
        public int CharismaMod {get; private set;}
        public int StartingGold {get; private set;}
        public List<Item> Inventory {get; private set;}
        public string Description {get; private set;}

        public PlayerBackground(string name, int startingGold, List<Item> inventory, string description,
            int strengthMod = 0, int enduranceMod = 0, int perceptionMod = 0, int charismaMod = 0)
        {
            Name = name;
            StrengthMod = strengthMod;
            EnduranceMod = enduranceMod;
            PerceptionMod = perceptionMod;
            CharismaMod = charismaMod;
            StartingGold = startingGold;
            Inventory = inventory;
            Description = description;
        }
    }

    public static class PlayerBackgrounds
    {
        public static readonly PlayerBackground Noble = new PlayerBackground("Noble", startingGold: 200,
            description: "You start with 200 gold, a fine ration, but not much else.", 
            inventory: new List<Item>(){ItemsRepository.GetByName("Fine Ration")});
        
        public static readonly PlayerBackground Miner = new PlayerBackground("Miner", startingGold: 50, strengthMod: 2,
            description: "You are stronger than normal due to years of hard labor in the mines. Additionally, you have 50 gold, an iron pickaxe, a hardy ration, and a miner's helmet.",
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Decent Iron Pickaxe"), 
                ItemsRepository.GetByName("Hardy Ration"),
                ItemsRepository.GetByName("Miner's Helmet")
            }
        );
        
        public static readonly PlayerBackground Farmer = new PlayerBackground("Farmer", startingGold: 50, enduranceMod: 2,
            description: "You are more durable than normal due to years of farm work. Additionally, you have 50 gold, an iron hoe, a farmer's meal, and a torch.",
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Decent Iron Hoe"),
                ItemsRepository.GetByName("Torch"),
                ItemsRepository.GetByName("Farmer's Meal")
            }
        );
        
        public static readonly PlayerBackground Hunter = new PlayerBackground("Hunter", startingGold: 50,  perceptionMod: 2,
            description: "You are more perceptive than normal due to your experience as a hunter. Additionally, you have 50 gold, an shortbow, beast bane incense, and a common ration.",
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Crude Shortbow"), 
                ItemsRepository.GetByName("Beast Bane Incense"), 
                ItemsRepository.GetByName("Common Ration")
            }
        );
        
        public static readonly PlayerBackground Priest = new PlayerBackground("Priest", startingGold: 50, charismaMod: 2,
            description: "You are more persuasive than normal due to your status as a priest. Additionally, you have 50 gold, a vessel of alchemist's fire, annointing oil, a candle, and a common ration.",
            inventory: new List<Item>()
            {
                ItemsRepository.GetByName("Alchemist's Fire"), 
                ItemsRepository.GetByName("Annointing Oil"), 
                ItemsRepository.GetByName("Common Ration"),
                ItemsRepository.GetByName("Ceremonial Candle")
            }
        );
    }
}
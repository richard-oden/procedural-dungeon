using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class PlayerBackground
    {
        public int StrengthMod {get; protected set;}
        public int EnduranceMod {get; protected set;}
        public int PerceptionMod {get; protected set;}
        public int CharismaMod {get; protected set;}
        public int StartingGold {get; protected set;}
        public List<Item> Inventory {get; protected set;}

        public PlayerBackground(int startingGold, List<Item> inventory,
            int strengthMod = 0, int enduranceMod = 0, int perceptionMod = 0, int charismaMod = 0)
        {
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
        public static PlayerBackground Noble; // Fine clothing, 200, but no mods
        public static PlayerBackground Miner; // Tattered clothing, pickaxe, lantern, little money, increased strength
        public static PlayerBackground Farmer; // Tattered clothing, hoe, food, little money, increased endurance
        public static PlayerBackground Hunter; // Decent clothing, bow, beast bane, little money, increased perception
        public static PlayerBackground Priest; // Decent clothing, morning star, annointing oil, little money, increased charisma
    }
}
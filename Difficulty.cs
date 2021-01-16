namespace ProceduralDungeon
{
    public class Difficulty
    {
        public string Name {get; private set;}
        public int AverageNpcChallenge {get; private set;}
        public int MaxNpcsPerTile {get; private set;}
        public double NpcToTileRatio {get; private set;}
        public int AverageItemValue {get; private set;}
        public double ItemToTileRatio {get; private set;}
        public int MerchantInventorySize {get; private set;}
        public int AverageMerchantGold {get; private set;}
        public double ChestSpawnChance {get; private set;}
        public int ChestValuePerTile {get; private set;}

        public Difficulty(string name, int averageNpcChallenge, int maxNpcsPerTile, double npcToTileRatio,
            int averageItemValue, double itemToTileRatio, int merchantInventorySize, int averageMerchantGold,
            double chestSpawnChance, int chestValuePerTile)
        {
            Name = name;
            AverageNpcChallenge = averageNpcChallenge;
            MaxNpcsPerTile = maxNpcsPerTile;
            NpcToTileRatio = npcToTileRatio;
            AverageItemValue = averageItemValue;
            ItemToTileRatio = itemToTileRatio;
            MerchantInventorySize = merchantInventorySize;
            AverageMerchantGold = averageMerchantGold;
            ChestSpawnChance = chestSpawnChance;
            ChestValuePerTile = chestValuePerTile;
        }
    }

    public static class Difficulties
    {
        public static readonly Difficulty VeryEasy = 
            new Difficulty(name: "Very Easy", averageNpcChallenge: 1, maxNpcsPerTile: 1, npcToTileRatio: .2,
                averageItemValue: 20, itemToTileRatio: .8, merchantInventorySize: 7, averageMerchantGold: 300,
                chestSpawnChance: .5, chestValuePerTile: 5);

        public static readonly Difficulty Easy = 
            new Difficulty(name: "Easy", averageNpcChallenge: 3, maxNpcsPerTile: 2, npcToTileRatio: .4,
                averageItemValue: 16, itemToTileRatio: .6, merchantInventorySize: 6, averageMerchantGold: 250,
                chestSpawnChance: .4, chestValuePerTile: 4);

        public static readonly Difficulty Medium = 
            new Difficulty(name: "Medium", averageNpcChallenge: 5, maxNpcsPerTile: 3, npcToTileRatio: .6,
                averageItemValue: 8, itemToTileRatio: .4, merchantInventorySize: 6, averageMerchantGold: 200,
                chestSpawnChance: .3, chestValuePerTile: 3);

        public static readonly Difficulty Hard = 
            new Difficulty(name: "Hard", averageNpcChallenge: 7, maxNpcsPerTile: 4, npcToTileRatio: .8,
                averageItemValue: 4, itemToTileRatio: .2, merchantInventorySize: 6, averageMerchantGold: 150,
                chestSpawnChance: .2, chestValuePerTile: 2);

        public static readonly Difficulty VeryHard = 
            new Difficulty(name: "Very Hard", averageNpcChallenge: 9, maxNpcsPerTile: 5, npcToTileRatio: 1,
                averageItemValue: 0, itemToTileRatio: .1, merchantInventorySize: 6, averageMerchantGold: 100,
                chestSpawnChance: .1, chestValuePerTile: 1);
    }
}
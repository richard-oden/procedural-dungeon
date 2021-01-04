namespace ProceduralDungeon
{
    public class Difficulty
    {
        public int AverageNpcChallenge {get; private set;}
        public int MaxNpcsPerTile {get; private set;}
        public double NpcToTileRatio {get; private set;}
        public int AverageItemValue {get; private set;}
        public double ItemToTileRatio {get; private set;}
        public int MerchantInventorySize {get; private set;}
        public int AverageMerchantGold {get; private set;}

        public Difficulty(int averageNpcChallenge, int maxNpcsPerTile, double npcToTileRatio,
            int averageItemValue, double itemToTileRatio, int merchantInventorySize, int averageMerchantGold)
        {
            AverageNpcChallenge = averageNpcChallenge;
            MaxNpcsPerTile = maxNpcsPerTile;
            NpcToTileRatio = npcToTileRatio;
            AverageItemValue = averageItemValue;
            ItemToTileRatio = itemToTileRatio;
            MerchantInventorySize = merchantInventorySize;
            AverageMerchantGold = averageMerchantGold;
        }
    }

    public static class Difficulties
    {
        public static readonly Difficulty VeryEasy = 
            new Difficulty(averageNpcChallenge: 1, maxNpcsPerTile: 1, npcToTileRatio: .2,
                averageItemValue: 20, itemToTileRatio: 1, merchantInventorySize: 7, averageMerchantGold: 300);

        public static readonly Difficulty Easy = 
            new Difficulty(averageNpcChallenge: 3, maxNpcsPerTile: 2, npcToTileRatio: .4,
                averageItemValue: 16, itemToTileRatio: .8, merchantInventorySize: 6, averageMerchantGold: 250);

        public static readonly Difficulty Medium = 
            new Difficulty(averageNpcChallenge: 5, maxNpcsPerTile: 3, npcToTileRatio: .6,
                averageItemValue: 8, itemToTileRatio: .6, merchantInventorySize: 6, averageMerchantGold: 200);

        public static readonly Difficulty Hard = 
            new Difficulty(averageNpcChallenge: 7, maxNpcsPerTile: 4, npcToTileRatio: .8,
                averageItemValue: 4, itemToTileRatio: .4, merchantInventorySize: 6, averageMerchantGold: 150);

        public static readonly Difficulty VeryHard = 
            new Difficulty(averageNpcChallenge: 9, maxNpcsPerTile: 5, npcToTileRatio: 1,
                averageItemValue: 0, itemToTileRatio: .2, merchantInventorySize: 6, averageMerchantGold: 100);
    }
}
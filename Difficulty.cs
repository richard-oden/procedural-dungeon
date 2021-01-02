namespace ProceduralDungeon
{
    public class Difficulty
    {
        public double AverageNpcChallenge {get; private set;}
        public int MaxNpcsPerTile {get; private set;}
        public double NpcToTileRatio {get; private set;}
        public double AverageItemRarity {get; private set;}
        public double ItemToTileRatio {get; private set;}

        public Difficulty(double averageNpcChallenge, int maxNpcsPerTile, double npcToTileRatio,
            double averageItemRarity, double itemToTileRatio)
        {
            AverageNpcChallenge = averageNpcChallenge;
            MaxNpcsPerTile = maxNpcsPerTile;
            NpcToTileRatio = npcToTileRatio;
            AverageItemRarity = averageItemRarity;
            ItemToTileRatio = itemToTileRatio;
        }
    }

    public static class Difficulties
    {
        public static readonly Difficulty VeryEasy = 
            new Difficulty(averageNpcChallenge: 1, maxNpcsPerTile: 1, npcToTileRatio: .2,
                averageItemRarity: 8, itemToTileRatio: 1);

        public static readonly Difficulty Easy = 
            new Difficulty(averageNpcChallenge: 3, maxNpcsPerTile: 2, npcToTileRatio: .4,
                averageItemRarity: 6, itemToTileRatio: .8);

        public static readonly Difficulty Medium = 
            new Difficulty(averageNpcChallenge: 5, maxNpcsPerTile: 3, npcToTileRatio: .6,
                averageItemRarity: 4, itemToTileRatio: .6);

        public static readonly Difficulty Hard = 
            new Difficulty(averageNpcChallenge: 7, maxNpcsPerTile: 4, npcToTileRatio: .8,
                averageItemRarity: 2, itemToTileRatio: .4);

        public static readonly Difficulty VeryHard = 
            new Difficulty(averageNpcChallenge: 9, maxNpcsPerTile: 5, npcToTileRatio: 1,
                averageItemRarity: 0, itemToTileRatio: .2);
    }
}
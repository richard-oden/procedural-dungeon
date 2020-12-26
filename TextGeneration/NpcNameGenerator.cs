namespace ProceduralDungeon.TextGeneration
{
    public static class NpcNameGenerator
    {
        public static readonly string[] FirstNamePrefixes = new string[]
        {
            "Eld", "Del", "Ald", "Grym", "Jil", "Dor", "Dar", "Phyl", "Jul",
            "Cer", "Syr", "Zil", "Nor", "Mue", "Mur", "Gil", "Ral", "Flor",
            "Dryn", "Mal", "Ben", "Ro", "Op", "Quar", "Bel"
        };

        public static readonly string[] FirstNameSuffixes = new string[]
        {
            "far", "ius", "ios", "eus", "ston", "ien", "ian", "ion", "ia", 
            "any", "men", "min", "aria", "ith", "iath", "eth", "phor", "ole",
            "ale", "al", "esh", "ally", "rus", "crest", "ham", "ford", "hed"
        };

        public static readonly string[] LastNamePrefixes = new string[]
        {
            "Hammer", "Spear", "Iron", "White", "Red", "Black", "Silver",
            "War", "Mill", "Well", "Joan", "Storm", "Fire", "Flame", "Frost",
            "Stone", "Boulder", "Good", "Plain", "Warm", "Light", "Wheat"
        };

        public static readonly string[] LastNameSuffixes = new string[]
        {
            "son", "ston", "feld", "field", "hand", "fist", "ford", "hed",
            "dale", "ham", "shield", "banner", "carver", "forger", "fisher",
            "tailor", "smith", "fletcher", "cross", "crow", "lake", "stream",
            "briger", "farmer", "body"
        };

        public static string Generate()
        {
            return $"{FirstNamePrefixes.RandomElement()}{FirstNameSuffixes.RandomElement()} {LastNamePrefixes.RandomElement()}{LastNameSuffixes.RandomElement()}";
        }
    }
}
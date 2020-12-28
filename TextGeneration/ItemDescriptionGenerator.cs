namespace ProceduralDungeon.TextGeneration
{
    public static class ItemDescriptionGenerator
    {
        private static string[] _hardMaterials = new string[]{
            "gold", "silver", "iron", "ruby", "copper", "saphire", "topaz", "stone",
            "obsidian", "mithril", "clay", "glass", "diamond", "bronze", "quartz", "wood"
        };

        private static string[] _softMaterials = new string[]{
            "leather", "canvas", "wool", "cotton", "silk", "rope"
        };

        private static string[] _weaponMaterialAdjectives = new string[]{
            "tipped", "lined", "enscribed", "inlaid", "coated"
        };
        
        private static string[] _qualityAdjectives = new string[]{
            "shoddy", "battered", "worn", "good quality", "well-made", "masterwork",
            "superbly-made", "fine", "decent", "damaged"
        };
        
        private static string[] _weaponTypes = new string[]{
            "shortsword", "greatsword", "longsword", "scimitar", "dagger", "bastard sword",
            "greataxe", "waraxe", "battleaxe", "warhammer", "maul", "spear", "club"
        };

        public static string Generate()
        {
            var line1 = $"It's a {_qualityAdjectives.RandomElement()} {_hardMaterials.RandomElement()} {_weaponTypes.RandomElement()}. ";
            var line2 = $"It's {_weaponMaterialAdjectives.RandomElement()} with {_hardMaterials.RandomElement()}, ";
            var line3 =  $"and the handle is {_weaponMaterialAdjectives.RandomElement()} with {_softMaterials.RandomElement()}.";
            return line1 + line2 + line3;
        }
    }
}
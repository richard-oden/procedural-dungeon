using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Material : INameable
    {
        public string Name {get; protected set;}
        public double Weight {get; protected set;} // per cubic inch
        public int Value {get; protected set;} // per pound
        public MaterialCategory Category {get; protected set;}
        public ItemRarity Rarity {get; protected set;}
    
        public Material(string name, double weight, int value, 
            MaterialCategory category, ItemRarity rarity)
        {
            Name = name;
            Weight = weight;
            Value = value;
            Category = category;
            Rarity = rarity;
        }
    }

    public static class Materials
    {
        public static readonly Material[] All = new Material[] {
            new Material("Iron", .26, 4, MaterialCategory.Metal, ItemRarity.Common),
            new Material("Tin", .26, 2, MaterialCategory.Metal, ItemRarity.Common),
            new Material("Bronze", .28, 5, MaterialCategory.Metal, ItemRarity.Common),
            new Material("Stone", .1, 2, MaterialCategory.OtherMineral, ItemRarity.Common),
            new Material("Bone", .06, 2, MaterialCategory.OrganicHard, ItemRarity.Common),
            new Material("Ivory", .06, 3, MaterialCategory.OrganicHard, ItemRarity.Common),
            new Material("Wood", .1, 1, MaterialCategory.OrganicHard, ItemRarity.Common),
            new Material("Wool", .04, 1, MaterialCategory.Fabric, ItemRarity.Common),
            new Material("Hemp", .05, 1, MaterialCategory.Fabric, ItemRarity.Common),
            new Material("Linen", .05, 1, MaterialCategory.Fabric, ItemRarity.Common),
            new Material("Cotton", .04, 1, MaterialCategory.Fabric, ItemRarity.Common),
            new Material("Clay", .06, 2, MaterialCategory.Fragile, ItemRarity.Common),
            new Material("Glass", .08, 4, MaterialCategory.Fragile, ItemRarity.Common),

            new Material("Brass", .3, 7, MaterialCategory.Metal, ItemRarity.Uncommon),
            new Material("Copper", .32, 6, MaterialCategory.Metal, ItemRarity.Uncommon),
            new Material("Silver", .38, 12, MaterialCategory.Metal, ItemRarity.Uncommon),
            new Material("Cobalt", .32, 8, MaterialCategory.Metal, ItemRarity.Uncommon),
            new Material("Steel", .28, 10, MaterialCategory.Metal, ItemRarity.Uncommon),
            new Material("Quartz", .09, 7, MaterialCategory.Gemstone, ItemRarity.Uncommon),
            new Material("Silk", .03, 3, MaterialCategory.Fabric, ItemRarity.Uncommon),
            new Material("Leather", .09, 4, MaterialCategory.Fabric, ItemRarity.Uncommon),
            new Material("Snakeskin", .08, 5, MaterialCategory.Fabric, ItemRarity.Uncommon),
            new Material("Chambray", .04, 2, MaterialCategory.Fabric, ItemRarity.Uncommon),
            new Material("Porcelain", .08, 5, MaterialCategory.Fragile, ItemRarity.Uncommon),
            new Material("Chitin", .01, 6, MaterialCategory.OrganicHard, ItemRarity.Uncommon),
            new Material("Marble", .1, 4, MaterialCategory.OtherMineral, ItemRarity.Uncommon),

            new Material("Obsidian", .09, 12, MaterialCategory.OtherMineral, ItemRarity.Rare),
            new Material("Gold", .69, 20, MaterialCategory.Metal, ItemRarity.Rare),
            new Material("Jade", .12, 15, MaterialCategory.Gemstone, ItemRarity.Rare),
            new Material("Amethyst", .13, 16, MaterialCategory.Gemstone, ItemRarity.Rare),
            new Material("Topaz", .13, 14, MaterialCategory.Gemstone, ItemRarity.Rare),
            new Material("Oricalcum", .29, 14, MaterialCategory.Gemstone, ItemRarity.Rare),
            new Material("Pearl", .01, 18, MaterialCategory.Gemstone, ItemRarity.Rare),
            new Material("Amber", .04, 16, MaterialCategory.Gemstone, ItemRarity.Rare),
            new Material("Elven fiber", .02, 20, MaterialCategory.Fabric, ItemRarity.Rare),

            new Material("Dragontooth", .07, 22, MaterialCategory.OrganicHard, ItemRarity.VeryRare),
            new Material("Mithril", .2, 30, MaterialCategory.Metal, ItemRarity.VeryRare),
            new Material("Ruby", .15, 24, MaterialCategory.Gemstone, ItemRarity.VeryRare),
            new Material("Saphire", .14, 26, MaterialCategory.Gemstone, ItemRarity.VeryRare),
            new Material("Adamantine", .42, 32, MaterialCategory.Metal, ItemRarity.VeryRare),
            new Material("Aetherweave", .3, 40, MaterialCategory.Fabric, ItemRarity.VeryRare),
            new Material("Dragonhide", .12, 28, MaterialCategory.Fabric, ItemRarity.VeryRare),
            new Material("Pheonix feather", .01, 30, MaterialCategory.Fabric, ItemRarity.VeryRare),
        };

        public static readonly IEnumerable<Material> MeleeWeaponMaterials = All.Where(m => 
            m.Category == MaterialCategory.Metal || m.Category == MaterialCategory.OrganicHard || m.Category == MaterialCategory.OtherMineral);

        public static readonly IEnumerable<Material> ArmorMaterials = All.Where(m => 
            m.Category == MaterialCategory.Metal || m.Category == MaterialCategory.OrganicHard || m.Category == MaterialCategory.Fabric);

        public static readonly IEnumerable<Material> Fabrics = All.Where(m => m.Category == MaterialCategory.Fabric);
    }

    public enum MaterialCategory
    {
        Metal,
        Gemstone,
        OtherMineral,
        OrganicHard,
        Fabric,
        Fragile,
    }
}
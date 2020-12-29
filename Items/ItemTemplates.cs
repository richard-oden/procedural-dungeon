using System;
using System.Collections.Generic;
using System.Linq;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class ItemPart : INameable
    {
        public string Name {get; protected set;}
        public IEnumerable<Material> PossibleMaterials {get; protected set;}
        public Material Material {get; protected set;}
        public double VolumeMin {get; protected set;} // number of cubic inches
        public double VolumeMax {get; protected set;}
        public double Volume {get; protected set;}
        public bool CanOmit {get; protected set;}
        public double TotalWeight => Volume * Material.Weight;
        public double TotalValue => TotalWeight * Material.Value;

        public ItemPart(string name, IEnumerable<Material> possibleMaterials, double volumeMin, double volumeMax, bool canOmit = false)
        {
            Name = name;
            PossibleMaterials = possibleMaterials;
            VolumeMin = volumeMin;
            VolumeMax = volumeMax;
            CanOmit = canOmit;
        }

        public void Generate(Material material = null, ItemRarity? materialRarity = null, double? volume = null)
        {
            if (material != null)
            {
                Material = material;
            }
            else if (materialRarity != null)
            {
                Material = PossibleMaterials.Where(pM => pM.Rarity == materialRarity).RandomElement();
            }
            else
            {
                Material = PossibleMaterials.RandomElement();
            }

            if (volume != null)
            {
                if (volume > VolumeMin && volume < VolumeMax)
                {
                    Volume = (double)volume;
                }
                else
                {
                    throw new Exception($"{volume} is not a valid volume for {Name}. Must be between {VolumeMin} and {VolumeMax}.");
                }
            }
            else
            {
                Volume = RandomDouble(VolumeMin, VolumeMax);
            }
        }
    }

    public static class ItemTemplates
    {
        public static ItemPart[] Dagger => _getSwordTemplate(6, 18, 4, 5);
        public static ItemPart[] Shortsword => _getSwordTemplate(18, 36, 5, 7);
        public static ItemPart[] LongSword => _getSwordTemplate(36, 48, 8, 12);
        public static ItemPart[] GreatSword => _getSwordTemplate(48, 72, 15, 20);
        public static ItemPart[] Scimitar => _getSwordTemplate(30, 36, 8, 12);

        private static ItemPart[] _getSwordTemplate(double bladeLengthMin, double bladeLengthMax, 
            double gripLengthMin, double gripLengthMax)
        {
            return new ItemPart[] 
            {
                new ItemPart("Blade", Materials.MeleeWeaponMaterials, .4*bladeLengthMin, 2.25*bladeLengthMax),
                new ItemPart("Wrapping", Materials.Fabrics, .7*gripLengthMin, 3*gripLengthMax, true),
                new ItemPart("Pommel", Materials.MeleeWeaponMaterials, .4, 8, true),
                new ItemPart("Grip", Materials.MeleeWeaponMaterials, 1.5*gripLengthMin, 6*gripLengthMax),
                new ItemPart("Cross-guard", Materials.MeleeWeaponMaterials, 1.8, 11.5),
                new ItemPart("Hand guard", Materials.MeleeWeaponMaterials, .5, 4)
            };
        }
    }
}
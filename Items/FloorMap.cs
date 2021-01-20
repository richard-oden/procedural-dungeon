using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class FloorMap : Item, IInteractable
    {
        public override string Name {get; protected set;} = "Floor Map";
        public override double Weight {get; protected set;} = 0.1;
        public override int Value {get; protected set;} = 100;
        public override string Description {get; protected set;} = "Although nearly falling apart, this map still shows the floor's general layout and a few highlighted locations.";
        public override ItemRarity Rarity {get; protected set;} = ItemRarity.Rare;
        private Map _map;
        private List<IMappable> _highlightedAssets = new List<IMappable>();
        public override Point Location {get; set;}
        public FloorMap(Map map, Point location = null)
        {
            _map = map;
            Location = location;

            var potentialAssets = new List<IMappable>();
            potentialAssets.Add(map.Assets.Single(a => a is Door));
            potentialAssets.AddRange(map.Items.Where(i => i.Rarity == ItemRarity.Rare || i.Rarity == ItemRarity.VeryRare));
            _highlightedAssets.AddRange(potentialAssets.RandomSample(5));
        }

        public FloorMap(FloorMap floorMapToClone)
        {
            _map = floorMapToClone._map;
            Location = floorMapToClone.Location;
            _highlightedAssets = floorMapToClone._highlightedAssets;
        }

        public override Item GetClone()
        {
            return new FloorMap(this);
        }

        public void Activate(Player player)
        {
            Console.WriteLine("You study the map for a moment. It shows the following locations:");
            _highlightedAssets.ListDistanceAndDirectionFrom(player.Location);
            _map.PrintMapWithOnlyHighlightedAssets(_highlightedAssets);
        }
    }
}
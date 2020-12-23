using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class FloorMap : Item, IInteractable
    {
        private Door _door {get; set;}
        private Player _player {get; set;}
        private Map _map {get; set;}
        private List<IMappable> _highlightedAssets = new List<IMappable>();
        public override Point Location {get; set;}
        public FloorMap(Map map, Player player, Point location = null)
        {
            Name = "Floor map";
            Weight = 0.1;
            Value = 100;
            Description = "Although nearly falling apart, this map still shows the floor's general layout and a few highlighted locations.";
            Rarity = ItemRarity.Rare;
            _map = map;
            _player = player;
            Location = location;

            var potentialAssets = new List<IMappable>();
            potentialAssets.Add(map.Assets.Single(a => a is Door));
            potentialAssets.AddRange(map.Items.Where(i => i.Rarity == ItemRarity.Rare || i.Rarity == ItemRarity.VeryRare));
            _highlightedAssets.AddRange(potentialAssets.RandomSample(5));
        }

        public void Activate()
        {
            System.Console.WriteLine("You study the map for a moment. It shows the following locations:");
            _highlightedAssets.ListDistanceAndDirectionFrom(_player.Location);
            _map.PrintMapHighlightingAssets(_highlightedAssets);
        }
    }
}
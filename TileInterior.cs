using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class TileInterior : Map
    {
        public TileSize TileWidth {get; set;}
        public TileSize TileHeight {get; set;}
        public TileInterior(TileSize width, TileSize height, List<IMappable> assets = null) : base()
        {
            TileWidth = width;
            TileHeight = height;
            Width = (int)width-2;
            Height = (int)height-2;
            if (assets == null) Assets = new List<IMappable>();
            else Assets = assets;
        }

        private List<Barrier> createBarriersFromPreset(InteriorPreset preset)
        {
            var barriers = new List<Barrier>();
            switch (preset)
            {
                case InteriorPreset.Zigzag:
                    
                    break;
                case InteriorPreset.CornerPillars:

                    break;
                case InteriorPreset.CentralPillar:
                    
                    break;
            }
            return barriers;
        }
    }

    public enum InteriorPreset
    {
        Zigzag,
        CornerPillars,
        CentralPillar,
        Random,
    }
}
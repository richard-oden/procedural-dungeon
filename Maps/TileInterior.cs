using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class TileInterior : Map
    {
        public TileSize TileWidth {get; set;}
        public TileSize TileHeight {get; set;}
        public InteriorPreset Preset {get; private set;}
        public TileInterior(TileSize width, TileSize height, List<IMappable> assets = null) : base()
        {
            TileWidth = width;
            TileHeight = height;
            Width = (int)width-2;
            Height = (int)height-2;
            if (assets == null) Assets = new List<IMappable>();
            else Assets = assets;
        }

        public TileInterior(TileSize width, TileSize height, InteriorPreset preset)
        {
            TileWidth = width;
            TileHeight = height;
            Width = (int)width-2;
            Height = (int)height-2;
            Preset = preset;

            switch (preset)
            {
                case InteriorPreset.IndentedCorners:
                    AddAsset(new Wall(new Point(0, 0)));
                    AddAsset(new Wall(new Point(Width-1, 0)));
                    AddAsset(new Wall(new Point(0, Height-1)));
                    AddAsset(new Wall(new Point(Width-1, Height-1)));
                    break;
                case InteriorPreset.CornerPillars:
                    int pillarHeight = TileHeight == TileSize.Large ? 1 : 0;
                    int pillarWidth = TileWidth == TileSize.Large ? 1 : 0;

                    AddAsset(new Barrier(new Rectangle(new Point(1,1), pillarWidth, pillarHeight)));
                    AddAsset(new Barrier(new Rectangle(new Point(Width-(2+pillarWidth),1), pillarWidth, pillarHeight)));
                    AddAsset(new Barrier(new Rectangle(new Point(1,Height-(2+pillarHeight)), pillarWidth, pillarHeight)));
                    AddAsset(new Barrier(new Rectangle(new Point(Width-(2+pillarWidth),Height-(2+pillarHeight)), pillarWidth, pillarHeight)));
                    break;
                case InteriorPreset.CornerPillarsLarge:
                    int pillarHeightLarge = TileHeight == TileSize.Large ? 2 : 1;
                    int pillarWidthLarge = TileWidth == TileSize.Large ? 2 : 1;

                    AddAsset(new Barrier(new Rectangle(new Point(1,1), pillarWidthLarge, pillarHeightLarge)));
                    AddAsset(new Barrier(new Rectangle(new Point(Width-(2+pillarWidthLarge),1), pillarWidthLarge, pillarHeightLarge)));
                    AddAsset(new Barrier(new Rectangle(new Point(1,Height-(2+pillarHeightLarge)), pillarWidthLarge, pillarHeightLarge)));
                    AddAsset(new Barrier(new Rectangle(new Point(Width-(2+pillarWidthLarge),Height-(2+pillarHeightLarge)), pillarWidthLarge, pillarHeightLarge)));
                    break;
            }
        }
    }

    public enum InteriorPreset
    {
        Empty,
        IndentedCorners,
        CornerPillars,
        CornerPillarsLarge,
    }
}
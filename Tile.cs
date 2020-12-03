using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class Tile : Map
    {
        public TileBorder NorthBorder {get; private set;}
        public TileBorder SouthBorder {get; private set;}
        public TileBorder EastBorder {get; private set;}
        public TileBorder WestBorder {get; private set;}
        // public TileInterior Interior {get; private set;}

        public Tile(TileSize width, TileSize height, 
            TileBorder north, TileBorder south, TileBorder east, TileBorder west) : base()
        {
            Width = (int)width;
            Height = (int)height;

            NorthBorder = north;
            AddAssets(NorthBorder.Assets, new Point(0, 0));

            SouthBorder = south;
            AddAssets(SouthBorder.Assets, new Point(0, Height-1));

            EastBorder = east;
            AddAssets(EastBorder.Assets, new Point(Width-1, 0));

            WestBorder = west;
            AddAssets(WestBorder.Assets, new Point(0, 0));
        }

    }
    public enum TileSize
    {
        Tiny = 3,
        Small = 6,
        Medium = 9,
        Large = 12
    }
}
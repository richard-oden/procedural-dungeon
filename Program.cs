using System;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {

            var test = new Tile(TileSize.Large, TileSize.Medium,
            north: new TileBorder(TileSize.Large, Orientation.Horizontal, 1),
            south: new TileBorder(TileSize.Large, Orientation.Horizontal, 2),
            east: new TileBorder(TileSize.Medium, Orientation.Vertical, 0),
            west: new TileBorder(TileSize.Medium, Orientation.Vertical, 1));
            
            test.PrintMap();

            foreach(IMappable x in test.Assets)
            if (x is IRectangular)
            {
                var xr = (IRectangular)x;
                System.Console.WriteLine($"{xr.Location.ToString()} Width: {xr.Rect.Width} Height {xr.Rect.Height}");
            }
        }
    }
}

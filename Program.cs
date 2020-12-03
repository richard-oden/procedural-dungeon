using System;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {

            var test = new Tile(TileSize.Medium, TileSize.Large, 2, 1, 0, 1);
            
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

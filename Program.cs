using System;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var test = new Map(80, 50, 40, 120);

            // var test = new Tile(TileSize.Medium, TileSize.Medium, 1, 1, 1, 1,
            //     new TileInterior(TileSize.Medium, TileSize.Medium, InteriorPreset.IndentedCorners));
            test.PrintMap();
        }
    }
}

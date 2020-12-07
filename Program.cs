using System;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Map(60, 40, 20, 100);
            test.PrintMap();
        }
    }
}

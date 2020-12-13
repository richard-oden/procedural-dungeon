using System;
using System.Collections.Generic;
using System.Linq;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var testMap = new Map(60, 40, 20, 80);
            var testPlayer = new Player(name: "Bill", id: 001, hp: 10, speed: 1);
            var testNpcs = new Npc[]
            {
                new Npc(name: "Giant Rat", id: 002, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 003, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 004, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 005, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 006, hp: 10, speed: 1)
            };
            testMap.AddPlayer(testPlayer);
            foreach (var npc in testNpcs) testMap.AddNpc(npc);


            while (true)
            {
                testMap.PrintMap();
                var moveInput = Console.ReadKey();
                testMap.Move(testPlayer, moveInput);
                foreach (var npc in testNpcs) 
                {
                    if (testMap.GetPathObstructions(npc.Location, testPlayer.Location).Any())
                    {
                        testMap.Wander(npc);
                    }
                    else
                    {
                        System.Console.WriteLine($"{npc.Location} Can see player");
                        testMap.MoveToward(npc, testPlayer.Location);
                    }
                }
                PressAnyKeyToContinue();
                Console.Clear();
            }

            // var m = new Map(30, 10);
            // var r = new Rectangle(new Point(5,5), 10, 0);
            // var p1 = new Point(10, 3);
            // var p2 = new Point(5, 4);
            // m.AddAssets(new List<IMappable>(){new Barrier(r), new Wall(p1), new Wall(p2)});
            // System.Console.WriteLine($"{r.NeCorner}, {r.NwCorner}, {r.SeCorner}, {r.SwCorner}");
            // // System.Console.WriteLine(Point.DoLinesIntersect(r.NwCorner, r.SwCorner, p1, p2));
            // // System.Console.WriteLine(Point.DoLinesIntersect(r.NwCorner, r.NeCorner, p1, p2));
            // // System.Console.WriteLine(Point.DoLinesIntersect(r.SeCorner, r.NeCorner, p1, p2));
            // // System.Console.WriteLine(Point.DoLinesIntersect(r.SeCorner, r.SwCorner, p1, p2));
            // System.Console.WriteLine(m.GetPathObstructions(p1, p2).Select(o => o.Location).ToString("and"));
            // m.PrintMap();
        }
    }
}

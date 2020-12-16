using System;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleKey;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Player : Creature
    {
        public Player(string name, int id, int hp, int speed, Point location = null,
            List<Item> inventory = null, List<INameable> memory = null) :
            base (name, id, hp, speed, location, inventory, memory)
        {
            SearchRange = 8;
        }

        public bool ParseInput(Map map, ConsoleKeyInfo input)
        {
            if (new[]{Q, W, E, A, D, Z, X, C}.Contains(input.Key)) map.Move(this, input);
            else if (input.Key == S) Search(map);
            else if (input.Key == L) Map.ShowLegend();
            else if (input.Key == M) {}// Open menu
            else if (input.Key == I) {}// Open inventory
            else if (input.Key == P) {}// Pick up item
            else if (input.Key == O) {}// Drop item
            else if (input.Key == U) {}// Use item
            else if (input.Key == J) {}// Interact
            else if (input.Key == F) {}// Attack
            else if (input.Key == R) {}// Memory
            else if (input.Key == T) {}// Describe item
            else if (input.Key == Escape) {}// Quit menu
            else System.Console.WriteLine("Command not recognized. Press 'M' to open the menu for a full list of commands.");
            return true;
        }

        public override void Search(Map map)
        {
            var foundAssets = GetVisibleAssets(map).Where(a => a is INameable);
            if (!foundAssets.Any())
            {
                Console.WriteLine($"{Name} searched but couldn't find anything!");
            }
            else
            {
                System.Console.WriteLine($"{Name} searched and found:");
                foreach (var fA in foundAssets) 
                {
                    Console.WriteLine($"- {(fA as INameable).Name} located {Math.Round(Location.DistanceTo(fA.Location)*5)} feet {Location.DirectionTo(fA.Location)}");
                    AddToMemory(fA as INameable);
                }
            }
            PressAnyKeyToContinue();
        }
    }
}
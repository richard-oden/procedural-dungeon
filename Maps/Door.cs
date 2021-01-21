using System;
using System.Linq;

namespace ProceduralDungeon
{
    public class Door : IMappable, IInteractable, INameable
    {
        public string Name {get; private set;} = "Door";
        public Point Location {get; set;}
        private Map _map {get; set;}
        public char Symbol {get; protected set;} = Symbols.Door;
        public virtual ConsoleColor Color {get; protected set;} = ConsoleColor.Black;
        private bool _requiresKey {get; set;}

        public Door(Map map, Point location, bool requiresKey = true)
        {
            _map = map;
            Location = location;
            _requiresKey = requiresKey;
        }

        public void SetLocation(Point location)
        {   
            Location = location;
        }

        public void Activate(Player player)
        {
            if (player.Inventory.Any(i => i is Key) || _requiresKey == false)
            {
                Console.WriteLine("The door slowly creaks open, revealing a staircase descending into darkness.");
                _map.HasPlayerExited = true;
            }
            else
            {
                Console.WriteLine("It's locked. The key should be around here somewhere...");
            }
        }
    }
}
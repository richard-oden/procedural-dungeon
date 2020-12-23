using System.Linq;

namespace ProceduralDungeon
{
    public class Door : IMappable, IInteractable, INameable
    {
        public string Name {get; private set;} = "Door";
        public Point Location {get; set;}
        private Player _player {get; set;}
        public char Symbol {get; protected set;} = Symbols.Door;

        public Door(Point location, Player player)
        {
            Location = location;
            _player = player;
        }

        public void SetLocation(Point location)
        {   
            Location = location;
        }

        public void Activate()
        {
            if (_player.Inventory.Any(i => i is Key))
            {
                System.Console.WriteLine("You insert the key and the door slowly creaks open, revealing a staircase descending into darkness.");
            }
            else
            {
                System.Console.WriteLine("It's locked. The key should be around here somewhere...");
            }
        }
    }
}
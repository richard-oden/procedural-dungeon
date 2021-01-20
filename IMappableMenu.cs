using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class IMappableMenu
    {
        public string Prompt {get; private set;}
        public List<IMappable> Options {get; private set;}
        private Map _map {get; set;}
        private Player _player {get; set;}
    
        public IMappableMenu(string prompt, List<IMappable> options, Player player, Map map = null)
        {
            Prompt = prompt;
            Options = options;
            _map = map;
            _player = player;
        }

        private void listOptions(IMappable highlightedOption)
        {
            foreach (var o in Options)
            {
                if (o == highlightedOption) Console.ForegroundColor = ConsoleColor.DarkYellow;

                Console.Write("- ");
                if (o is INameable) 
                {
                    Console.Write((o as INameable).Name);
                    if (o is Creature && (o as Creature).IsDead) Console.Write(" (dead)");
                }
                else
                {
                    Console.Write(o.GetType().Name);
                }

                if (o.Location != null)
                {
                    Console.Write($" located {Math.Round(_player.Location.DistanceTo(o.Location)*5)} feet {_player.Location.DirectionTo(o.Location)}");
                }
                else if (_player.Inventory.Contains(o))
                {
                    Console.Write(" (in inventory)");
                }
                Console.WriteLine('\n');
                Console.ResetColor();
            }
            if (highlightedOption is IDescribable)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine((highlightedOption as IDescribable).Description);
                Console.ResetColor();
            }
        }

        private int moveCursor(ConsoleKeyInfo input, int cursor)
        {
            int tempCursor = cursor;

            if (input.Key == ConsoleKey.UpArrow) tempCursor--;
            else if (input.Key == ConsoleKey.DownArrow) tempCursor++;

            int cursorMax = Options.Count-1;
            if (tempCursor > cursorMax) tempCursor = cursorMax;
            else if (tempCursor < 0) tempCursor = 0;
            else cursor = tempCursor;

            return cursor;
        }

        public IMappable Open()
        {
            if (Options.Any())
            {
                int cursorPosition = 0;
                bool menuOpen = true;
                while (menuOpen)
                {
                    IMappable highlightedOption = Options[cursorPosition];
                    Console.Clear();
                    Console.WriteLine(Prompt);
                    if (_map != null) _map.PrintMapFromViewport(_player, highlightedOption);
                    Console.WriteLine();
                    listOptions(highlightedOption);

                    var input = Console.ReadKey(true);

                    if (input.Key == ConsoleKey.Enter) return highlightedOption;
                    else if (input.Key == ConsoleKey.Escape) menuOpen = false;
                    else cursorPosition = moveCursor(input, cursorPosition);
                }
            }
            else
            {
                Console.WriteLine("There's nothing here.");
            }
            return null;
        }
    }
}
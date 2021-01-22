using System;

namespace ProceduralDungeon
{
    public class Wall : IMappable
    {
        public Point Location {get; protected set;}
        public char Symbol {get; protected set;} = ' ';
        public virtual ConsoleColor Color {get; protected set;} = ConsoleColor.Black;

        public Wall(Point location)
        {
            Location = location;
        }
        public void SetLocation(Point location)
        {   
            Location = location;
        }
    }
}
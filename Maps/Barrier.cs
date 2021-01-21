using System;

namespace ProceduralDungeon
{
    public class Barrier : IRectangular
    {
        public Point Location => Rect.StartLocation;
        public Rectangle Rect {get; private set;}
        public char Symbol {get; private set;} = Symbols.Barrier;
        public virtual ConsoleColor Color {get; protected set;} = ConsoleColor.Black;

        public Barrier(Rectangle rect)
        {
            Rect = rect;
        }
    }
}
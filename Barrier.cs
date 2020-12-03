namespace ProceduralDungeon
{
    public class Barrier : IRectangular
    {
        public Point Location {get; private set;}
        public Rectangle Rect {get; private set;}
        public char Symbol {get; private set;} = Symbols.Barrier;

        public Barrier(Rectangle rect)
        {
            Rect = rect;
            Location = Rect.StartLocation;
        }
    }
}
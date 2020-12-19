namespace ProceduralDungeon
{
    public class Door : IMappable
    {
        public Point Location {get; set;}
        public char Symbol {get; protected set;} = Symbols.Door;

        public Door(Point location)
        {
            Location = location;
        }
        public void SetLocation(Point location)
        {   
            Location = location;
        }
    }
}
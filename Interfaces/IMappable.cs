using System;

namespace ProceduralDungeon
{
    public interface IMappable
    {
        Point Location {get;}
        char Symbol {get;}
        ConsoleColor Color {get;}
    }
}
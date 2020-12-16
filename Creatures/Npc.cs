using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Npc : Creature
    {
        public override char Symbol => Symbols.Npc;
        public Npc(string name, int id, int hp, int speed, Point location = null,
            List<Item> inventory = null, List<INameable> memory = null) :
            base (name, id, hp, speed, location, inventory, memory)
        {}

        public void Wander(Map map)
        {
            var validDestinations = map.EmptyPoints.Where(eP => Location.InRangeOf(eP, 1));
            Location = validDestinations.RandomElement();
        }

        public void MoveToward(Map map, Point target)
        {
            int getDiff(int coord1, int coord2)
            {
                if (coord1 > coord2)
                {
                    return 1;
                }
                else if (coord1 < coord2)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            
            int xDiff = getDiff(target.X, Location.X);
            int yDiff = getDiff(target.Y, Location.Y);
            
            var potentialDestinations = new List<Point>()
            {
                new Point(Location.X + xDiff, Location.Y + yDiff),
                new Point(Location.X, Location.Y + yDiff),
                new Point(Location.X + xDiff, Location.Y)
            };

            var chosenDestination = potentialDestinations.FirstOrDefault(pD =>
                // Destination cannot be the same as current location:
                (pD.X != Location.X || pD.Y != Location.Y) &&
                // Destination must be an empty point:
                map.EmptyPoints.Any(eP => eP.X == pD.X && eP.Y == pD.Y));
            if (chosenDestination != null) Location = chosenDestination;
        }
    }

    public enum Aggression
    {
        Friendly,
        Passive,
        Hostile
    }
}
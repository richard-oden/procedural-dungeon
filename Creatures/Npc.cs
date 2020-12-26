using System;
using System.Collections.Generic;
using System.Linq;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Npc : Creature
    {
        public override char Symbol => IsDead ? Symbols.Dead : Symbols.Npc;
        public Npc(string name, int id, int hp, Gender gender = Gender.None, Point location = null,
            List<Item> inventory = null, int gold = 0, List<INameable> memory = null, int team = 1) :
            base (name, id, hp, gender, location, inventory, gold, memory)
        {
            Team = team;
            SearchRange = 5;
        }

        public void Wander(Map map)
        {
            var validDestinations = map.EmptyPoints.Where(eP => Location.InRangeOf(eP, 1));
            Location = validDestinations.RandomElement();
        }

        // TODO: Dry up MoveToward and MoveAwayFrom:
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

        public void MoveAwayFrom(Map map, Point target)
        {
            int getDiff(int coord1, int coord2)
            {
                if (coord1 > coord2)
                {
                    return -1;
                }
                else if (coord1 < coord2)
                {
                    return 1;
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
    
        public virtual void Act(Map map)
        {
            if (!IsDead)
            {
                var visibleEnemies = GetVisibleAssets(map).Where(a => a is Creature)
                    .Cast<Creature>().Where(c => c.Team != this.Team);
                if (visibleEnemies.Any())
                {
                    var knownVisibleEnemies = visibleEnemies.Where(vE => _memory.Contains(vE));
                    if (knownVisibleEnemies.Any())
                    {
                        var attackTarget = knownVisibleEnemies.RandomElement();
                        if (Location.InRangeOf(attackTarget.Location, _attackRange))
                        {
                            Attack(map, attackTarget);
                            WaitForInput();
                        }
                        else
                        {
                            MoveToward(map, attackTarget.Location);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine($"{Name} has spotted something. It's searching...");
                        WaitForInput();
                        Search(map);
                    }
                }
                else
                {
                    Wander(map);
                }
            }
        }
    }

    public enum Aggression
    {
        Friendly,
        Passive,
        Hostile
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Npc : Creature, IInteractable
    {
        public int ChallengeLevel {get; protected set;}
        public override char Symbol => IsDead ? Symbols.Dead : Symbols.Npc;
        public Npc(string name, int id, int challengeLevel, int hp, CreatureCategory category, int ac = 8, int dr = 0, 
            int attackMod = 0, Die[] damageDice = null, int damageMod = 0, int attackRange = 1, int searchRange = 5,
            Gender gender = Gender.None, Point location = null, double maxCarryWeight = 100, 
            List<Item> inventory = null, int gold = 0, List<INameable> memory = null, int team = 1, string baseDescription = null) :
            base (name, id, hp, category, gender, location, inventory, gold, memory, baseDescription)
        {
            Category = category;
            ChallengeLevel = challengeLevel;
            _baseArmorClass = ac;
            _baseDamageResistance = dr;
            _attackModifier = attackMod;
            _damageDice = damageDice == null ? new Die[]{Dice.D4} : damageDice;
            _damageModifier = damageMod;
            _attackRange = attackRange;
            SearchRange = searchRange;
            MaxCarryWeight = maxCarryWeight;
            Team = team;
        }

        public Npc GetClone()
        {
            return new Npc(Name, Id, ChallengeLevel, _maxHp, Category, ArmorClass, DamageResistance, 
                _attackModifier, _damageDice, _damageModifier, _attackRange, SearchRange, 
                Gender, Location, MaxCarryWeight, Inventory, Gold, _memory, Team, _baseDescription);
        }

        public void Wander(Map map)
        {
            // move to adjacent point
            var validDestinations = map.EmptyPoints.Where(eP => Location.InRangeOf(eP, 1)).ToList();
            Location = validDestinations.RandomElement();
        }

        private List<Point> getActiveRepellantLocationsInRange(Map map)
        {
            var activeRepellants = map.Repellants.Where(r => 
                r.IsActive && r.TargetCreatureCategory == this.Category);
            var activeRepellantLocationsInRange = new List<Point>();
            // If any active repellants on map are in range:
            if (activeRepellants.Any())
            {
                // Select assets where asset is equal to any active repellant:
                var activeRepellantsOnMap = map.Assets.Where(a => activeRepellants.Any(aR => a == aR));
                // Select containers that contain any active repellants:
                var containersWithActiveRepellants = activeRepellants.Select(aR => 
                    map.Containers.SingleOrDefault(c => c.Inventory.Contains(aR)))
                    // Remove duplicate containers and null values:
                    .Where(c => c != null).Distinct();
                // Select list of locations where repellants are:
                activeRepellantLocationsInRange = activeRepellantsOnMap.Select(aR => aR.Location)
                    .Concat(containersWithActiveRepellants.Select(c => (c as IMappable).Location))
                    .Where(p => p.InRangeOf(this.Location, SearchRange)).ToList();
            }
            return activeRepellantLocationsInRange;
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
                var visibleEnemies = getVisibleAssets(map).Where(a => a is Creature)
                    .Cast<Creature>().Where(c => c.Team != this.Team);
                var activeRepellantLocationsInRange = getActiveRepellantLocationsInRange(map);
                
                if (activeRepellantLocationsInRange.Any()) 
                {
                    MoveAwayFrom(map, activeRepellantLocationsInRange.RandomElement());
                }
                else if (visibleEnemies.Any())
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
                        Console.WriteLine($"{Name} has spotted something. It's searching...");
                        WaitForInput();
                        Search(map);
                    }
                }
                else
                {
                    if (Dice.Coin.Roll() == 1) Wander(map);
                }
            }
        }
    
        public virtual void Activate(Player player)
        {
            if (IsDead)
            {
                Console.WriteLine($"{player.Name} searches the {Name}.");
                WaitForInput();
                Console.Clear();
                (this as IContainer).OpenTradeMenu(player);
            }
            else
            {
                Console.WriteLine("Probably better to attack this!");
            }
        }
    }
}
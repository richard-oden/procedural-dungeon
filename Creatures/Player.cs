using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Player : Creature
    {
        public Player(string name, int id, int hp, int speed, Point location,
            List<IMappable> inventory = null, List<IMappable> memory = null) :
            base (name, id, hp, speed, location, inventory, memory)
        {}
    }
}
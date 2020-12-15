using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Npc : Creature
    {
        public override char Symbol => Symbols.Npc;
        public Npc(string name, int id, int hp, int speed, Point location = null,
            List<Item> inventory = null, List<IMappable> memory = null) :
            base (name, id, hp, speed, location, inventory, memory)
        {}
    }

    public enum Aggression
    {
        Friendly,
        Passive,
        Hostile
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public abstract class Creature : IMappable
    {
        public string Name {get; protected set;}
        public int Id {get; protected set;}
        // public Gender Gender {get; protected set;}
        // public AbilityScores AbilityScores {get; protected set;}
        protected int _hp {get; set;}
        protected int _currentHp {get; set;}
        protected int _speed {get; set;}
        public List<IMappable> Inventory {get; protected set;} = new List<IMappable>();
        protected List<IMappable> _memory {get; set;} = new List<IMappable>();
        public Point Location {get; set;}
        public virtual char Symbol {get; protected set;} = Symbols.Player;

        public Creature(string name, int id, int hp, int speed, Point location = null,
            List<IMappable> inventory = null, List<IMappable> memory = null)
        {
            Name = name;
            Id = id;
            _hp = hp;
            _speed = speed;
            if (location != null) Location = location;
            if (inventory != null) Inventory = inventory;
            if (memory != null) _memory = memory;
        }
    }
}
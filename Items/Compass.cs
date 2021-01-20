using System;

namespace ProceduralDungeon
{
    public class Compass : Item, IInteractable
    {
        public override string Name {get; protected set;} = "Compass";
        public override double Weight {get; protected set;} = 0.1;
        public override int Value {get; protected set;} = 100;
        public override string Description {get; protected set;} = "It's an ornate gold compass. The outside rim is etched with a magic encantation.";
        public override ItemRarity Rarity {get; protected set;} = ItemRarity.Rare;
        private Door _door {get; set;}
        public override Point Location {get; set;}
        public Compass(Door door, Point location = null)
        {
            _door = door;
            Location = location;
        }

        public Compass(Compass compassToClone)
        {
            _door = compassToClone._door;
            Location = compassToClone.Location;
        }

        public override Item GetClone()
        {
            return new Compass(this);
        }

        public void Activate(Player player)
        {
            Console.WriteLine($"You hold the compass level and repeat its encantation. The needle spins for a bit before settling {player.Location.DirectionTo(_door.Location)}.");
        }
    }
}
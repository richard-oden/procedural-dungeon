namespace ProceduralDungeon
{
    public class Compass : Item, IInteractable
    {
        private Door _door {get; set;}
        private Player _player {get; set;}
        public override Point Location {get; set;}
        public Compass(Door door, Player player, Point location = null)
        {
            Name = "Compass";
            Weight = 0.1;
            Value = 100;
            Description = "It's an ornate gold compass. The outside rim is etched with a magic encantation.";
            Rarity = ItemRarity.Rare;
            _door = door;
            _player = player;
            Location = location;
        }

        public void Activate()
        {
            System.Console.WriteLine($"You hold the compass level and repeat its encantation. The needle spins for a bit before settling {_player.Location.DirectionTo(_door.Location)}.");
        }
    }
}
using System;
using System.Collections.Generic;  
using System.Linq;

namespace ProceduralDungeon
{
    public class Repellant : Item, IInteractable
    {
        public CreatureCategory TargetCreatureCategory {get; protected set;}
        public bool IsActive {get; protected set;} = false;
        public int Duration {get; protected set;}
        private Map _map {get; set;}
        public Repellant(string name, CreatureCategory targetCreatureCategory, int duration, Map map, string description)
        {
            Name = name;
            TargetCreatureCategory = targetCreatureCategory;
            Duration = duration;
            Value = duration * 2;
            Weight = duration * .05;
            Description = description;
            _map = map;
        }

        public void DecrementDuration()
        {
            Duration--;
            if (Duration == 0) 
            {
                Console.WriteLine($"The {Name}'s effect has ended. {TargetCreatureCategory.ToString()}s are no longer turned.");
                IsActive = false;
                var containerHoldingThis = _map.Containers.SingleOrDefault(c => c.Inventory.Contains(this));
                if (containerHoldingThis != null)
                {
                    containerHoldingThis.RemoveItemFromInventory(this);
                }
                else if (_map.Assets.Contains(this))
                {
                    _map.RemoveAsset(this);
                }
            }
        }

        public void Activate(Player player)
        {
            if (!IsActive)
            {
                Console.WriteLine($"{player.Name} activates the {Name}. For the next {Duration} steps, all {TargetCreatureCategory.ToString()}s are turned.");
                IsActive = true;
            }
            else
            {
                var endRepellantInput = ExtensionsAndHelpers.PromptKey($"The {Name} is already active. Do you wish to end the effect? (Y/N).");
                if (endRepellantInput == ConsoleKey.Y)
                {
                    Console.WriteLine($"{player.Name} ends the {Name}'s effect early. It still has some use left.");
                    IsActive = false;
                }
                else
                {
                    Console.WriteLine("The effect is not ended.");
                }
            }  
        }
    }
}
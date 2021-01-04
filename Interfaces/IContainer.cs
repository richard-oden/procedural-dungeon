using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public interface IContainer
    { 
        List<Item> Inventory {get;}
        // If set to -1, carry weight is unlimited:
        double MaxCarryWeight {get;}
        double CurrentCarryWeight {get;}
        int Gold {get; set;}
        bool AddItemToInventory(Item itemToAdd, bool cloneItem = false);
        bool RemoveItemFromInventory(Item itemToRemove);
        string GetCarryWeightString();
        
        void TradeItem(Item itemToTrade, IContainer recipient, 
            bool requireGold = false, double discount = 1)
        {
            int price = (int)Math.Round(itemToTrade.Value * discount);
            if (Inventory.Contains(itemToTrade))
            {
                if (recipient.MaxCarryWeight < 0 || 
                    itemToTrade.Weight + recipient.CurrentCarryWeight <= recipient.MaxCarryWeight)
                {
                    if (!requireGold || 
                        price <= recipient.Gold)
                    {
                        RemoveItemFromInventory(itemToTrade);
                        recipient.AddItemToInventory(itemToTrade);
                        if (requireGold)
                        {
                            Gold += price;
                            recipient.Gold -= price;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{recipient.GetName()} does not have {price} gold.");
                    }
                }
                else
                {
                    Console.WriteLine($"{itemToTrade.Name} is too heavy for {recipient.GetName().ToLower()}.");
                }
            }
            else
            {
                Console.WriteLine($"{GetName()} does not currently have {itemToTrade.Name}.");
            }
        }

        Item ListTwoInventoriesAndSelect(IContainer otherContainer, int cursorX, int cursorY)
        {
            Item highlightedItem = cursorX == 0 ? otherContainer.Inventory[cursorY] : Inventory[cursorY];
            int listLength = otherContainer.Inventory.Count() > Inventory.Count() ? otherContainer.Inventory.Count() : Inventory.Count();
            int padSize = otherContainer.Inventory.Any() ? otherContainer.Inventory.Select(i => i.GetBasicDetails().Length).Max() + 5 
                : otherContainer.GetInfoString().Length + 5;
            Console.WriteLine(otherContainer.GetInfoString().PadRight(padSize, ' ') + GetInfoString() + "\n\n");
            for (int y = 0; y < listLength; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    if (cursorX == x && cursorY == y) Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (x == 0)
                    {
                        if (otherContainer.Inventory.Count > y) Console.Write(otherContainer.Inventory[y].GetBasicDetails().PadRight(padSize, ' '));
                        else
                        {
                            Console.Write("".PadRight(padSize, ' '));
                        }
                        Console.ResetColor();
                    }
                    else if (x == 1)
                    {
                        if (Inventory.Count > y) Console.WriteLine(Inventory[y].GetBasicDetails());
                        else
                        {
                            Console.WriteLine();
                        }
                    }
                    Console.ResetColor();
                }
                System.Console.WriteLine("---".PadRight(padSize, ' ') + "---");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine('\n' + highlightedItem.GetSecondaryDetails());
            Console.ResetColor();
            return highlightedItem;
        }
        
        void OpenTradeMenu(Player player)
        {
            int cursorX = player.Inventory.Any() ? 0 : 1;
            int cursorY = 0;
            bool stillTrading = true;

            while (stillTrading)
            {
                Item selectedItem = ListTwoInventoriesAndSelect(player, cursorX, cursorY);
                System.Console.WriteLine("\nUse the arrow keys to navigate, Enter to take/leave items, and Esc to exit.");
                var input = Console.ReadKey();

                int tempCursorX = cursorX;
                int tempCursorY = cursorY;
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        tempCursorY--;
                        break;
                    case ConsoleKey.DownArrow:
                        tempCursorY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        tempCursorX--;
                        break;
                    case ConsoleKey.RightArrow:
                        tempCursorX++;
                        break;
                    case ConsoleKey.Enter:
                        if (player.Inventory.Contains(selectedItem))
                        {
                            (player as IContainer).TradeItem(selectedItem, this);
                        }
                        else if (Inventory.Contains(selectedItem))
                        {
                            this.TradeItem(selectedItem, player);
                        }
                        break;
                    case ConsoleKey.Escape:
                        stillTrading = false;
                        break;
                }

                // Keep cursor X coordinate within menu bounds:
                if (tempCursorX < 0) tempCursorX = 0;
                else if (tempCursorX > 1) tempCursorX = 1;
                // If one inventory is empty, force cursor to other inventory:
                if (tempCursorX == 0 && !player.Inventory.Any()) tempCursorX = 1;
                else if (tempCursorX == 1 && !Inventory.Any()) tempCursorX = 0;
                cursorX = tempCursorX;

                // Keep cursor Y coordinate within menu bounds:
                int cursorYMax = tempCursorX <= 0 ? player.Inventory.Count() : Inventory.Count();
                if (tempCursorY < 0) cursorY = 0;
                else if (tempCursorY >= cursorYMax) cursorY = cursorYMax - 1;
                else cursorY = tempCursorY;
                Console.Clear();
            }
        }
        string GetName()
        {
            return this is INameable ? (this as INameable).Name : "The" + this.GetType().Name;
        }

        string GetInfoString()
        {
            return $"{GetName()} {GetCarryWeightString()} ({Gold} gold):";
        }
    }
}
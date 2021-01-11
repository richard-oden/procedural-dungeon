# Procedural Dungeon

Delve into an endless series of increasingly deadly dungeons, hone your skills, and acquire loot in this (WIP) console-based RPG.

Derived from a D&D-style multiplayer combat game I made for Code Louisville's C# class, this project started out as a system for procedurally generating dungeons, and has since evolved into its own infinite dungeon crawling game.

## How to play
Your goal is to progresss as far as possible. On each floor you'll need to find a key to unlock the door to the next floor. Along the way you'll encounter hostile creatures and valuable treasures.

### Controls:
```
- Movement keys: Q, W, E, A, D, Z, X, C       
- Map legend: L
- Search: S
- Recall memory: R
- Inventory: I
- Pick up item: P
- Drop item: O
- Equip item: U
- Unequip item: Y
- Interact: J
- Attack: F
- Show all hotkeys: H
```
(At the moment, only hotkeys are used to control the game, but I have plans to implement optional command-based controls as well.)

### Mechanics:
- **Strength, Endurance, Perception, and Charisma:** At the moment, these are the core attributes that determine how your character interacts with the game. I might expand on these later.
- **Experience/Leveling:** Experience is awared after killing creatures and for each floor reached. With each level up, a core attribute can be increased.
- **Player backgrounds:** These determine your starting core attributes, gold, and inventory. Think of it like your starting profession in Oregon Trail.
- **Searching/Memory:** In order to do anything in the game, your character must first know about it. Doing this requires that you search your surroundings frequently.
- **Combat:** Combat is very similar to D&D. Each creature involved has an armor class (AC), damage resistance (DR), attack bonus, damage dice, and damage bonus. An attacker first makes an attack roll, adding the attack bonus, against the target's armor class. If the attack roll is higher than the target's AC, the attacker then rolls their damage dice, adding their damage bonus. The target then takes the resulting damage, minus their DR.
- **Trade:** Every few floors, you will encounter a merchant with whom you may exchange goods and currency. This can be a valuable source of resources and equipment, but may also be costly, depending on your bartering skills.
- **Equippables:** Some items, like weapons, armor, shields, goggles, lanterns, and backpacks must be equipped before use.
- **Interactables:** Some objects and creatures may be interacted with. For example, you can consume potions, search dead bodies, and examine maps.

## Final Notes
In addition to being a WIP, this game is also the most ambitious and complex thing I've ever made. If anything, this is more of a learning project than a serious attempt at a well-balanced, heavily polished game. 

That said, if you decide to try this game out, I sincerely appreciate it and hope you enjoy it!
Battle System Game

This project is a Unity-based battle system game that allows players to engage in turn-based combat, featuring character customization, weapon selection, and a dynamic combat environment. The system calculates combat outcomes using character stats derived from equipped gear, providing a strategic layer to the battle experience.

Features

Turn-Based Combat System: The core mechanic of the game involves turn-based battles, where players select actions for their characters each round.

Character Customization: Players can create and manage multiple characters, each with unique stats and equipment.

Dynamic Weapon and Body Selection UI: Players choose weapons and attack points dynamically through a user-friendly button interface.

Gear-Based Stats Calculation: Character stats (life, force, speed, agility, mana, etc.) are calculated based on equipped gear. This draws inspiration from games like World of Warcraft, where gear impacts character performance.

Experience and Gold Management: Characters gain experience and gold from battles. Functions like BetGold and ReceiveGold are used to manage gold, while GiveExperience is used to update character levels.

Background Variation: Different battle arenas are selected dynamically to provide variety to each fight.

Hero Editor Integration: Characters are visually represented using Hero Editor assets, making the battles visually engaging.

Key Components

BattleHandler

Handles the core battle logic, including initiating fights, determining attack sequences, and calculating outcomes based on character stats.

Weapon Selection: Displays available weapons in a 6x6 button grid and allows players to select from different body parts to attack.

Background Management: Randomly changes the battle background between different arenas.

CharacterStats

Defines the attributes of each character, such as life, force, speed, agility, and mana. The stats are inspired by a configuration similar to the one used in the Pokémon series, making battles strategy-driven.

PlayerConfig

Stores player information, including gear like head, weapon, off-hand, and body. This class also includes methods for loading player configuration from a file and displaying it in the UI.

InventoryManager

Manages the dynamic loading and display of equipment slots for characters, making it easy for players to customize their gear.

Hero Editor Integration

Utilizes Hero Editor assets for character models, providing a highly customizable and visually consistent look for characters.

Installation & Setup

Unity Version: Ensure you have Unity version 2021.3 or later installed.

Clone the Repository: Clone this project to your local machine.

git clone <repository_url>

Open in Unity: Open the project in Unity Hub and allow the assets to import.

Dependencies: Make sure to install any required Unity packages, including Hero Editor and relevant UI packages.

How to Play

Create a Character: Start by creating a new character and customizing its stats and equipment.

Select Weapons and Attack: During a battle, use the weapon selection UI to choose your weapon and attack specific body parts of your opponent.

Gain Experience and Gold: Win battles to gain experience and gold, which can be used to level up your character and acquire better gear.

Future Improvements

Multiplayer Functionality: Implement a networked multiplayer mode to allow players to battle each other.

Enhanced AI: Improve the AI to make single-player battles more challenging.

Skill System: Introduce skills and abilities that characters can use during battle for more tactical depth.

Quest System: Add a quest system to give players context for battles and add long-term goals.


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
 
public enum GearType
{
    Weapon,
    Head,
    Armor,
    Legs,
    Feet,
    Accessory,
    Other
}


public class UserObjects
{
    public string name;
        public string description;
    public string iconFile;
    public string path;

    // New attributes for equipment stats
    public int agility;
    public int intelligence;
    public int lifePoints; 

        public GearType Type { get; set; } // New GearType property

    // Constructor to initialize the attributes
    public UserObjects(string name, string description, string iconFile, string path,int agility, int intelligence, int lifePoints,GearType type)
    {
        this.name = name;
        this.description = description;
        this.iconFile = iconFile;
        this.path = path;
        
        this.agility = agility;
        this.intelligence = intelligence;
        this.lifePoints = lifePoints;
    }

    public UserObjects()
    {
      
    }


    
public GearType FindGearTypeByFilename(string input)
{
    // Define the paths to your configuration files
    string weaponConfigPath = "Assets/TurnBattleSystem/weapon_config.txt";
    string armorConfigPath = "Assets/TurnBattleSystem/armor_config.txt";
    string legsConfigPath = "Assets/TurnBattleSystem/legs_config.txt";
    string feetConfigPath = "Assets/TurnBattleSystem/feet_config.txt";
    string accessoryConfigPath = "Assets/TurnBattleSystem/accessory_config.txt";
    string headConfigPath = "Assets/TurnBattleSystem/head_config.txt";

    // Check in each config file and return the corresponding GearType

    if (FileContainsInput(headConfigPath, input))
    {
        return GearType.Head;
    }

    if (FileContainsInput(weaponConfigPath, input))
    {
        return GearType.Weapon;
    }
    if (FileContainsInput(armorConfigPath, input))
    {
        return GearType.Armor;
    }
    if (FileContainsInput(legsConfigPath, input))
    {
        return GearType.Legs;
    }
    if (FileContainsInput(feetConfigPath, input))
    {
        return GearType.Feet;
    }
    if (FileContainsInput(accessoryConfigPath, input))
    {
        return GearType.Accessory;
    }

    // If no match is found, return Other
    return GearType.Other;
}

// Helper method to check if the input string is present in the config file
private bool FileContainsInput(string filePath, string input)
{
    try
    {
        // Read all lines of the file
        string[] lines = File.ReadAllLines(filePath);

        // Check each line for the input string
        foreach (string line in lines)
        {
            if (line.Contains(input))
            {
                return true;
            }
        }
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Error reading file {filePath}: {ex.Message}");
    }

    return false;
}
}

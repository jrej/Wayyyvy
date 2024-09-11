using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Armor : UserObjects
{
    public int defense;
    public int durability;

    // Constructor
    public Armor(string name, string iconFile, string path, int defense, int durability, string description, int agility, int intelligence, int lifePoints)
        : base(name, description, iconFile, path, agility, intelligence, lifePoints, GearType.Armor)
    {
        this.defense = defense;
        this.durability = durability;
    }

    public Armor()
    {
    }

    // Load armors from a single config file
    public static List<Armor> LoadArmorsFromConfig(string configFilePath)
    {
        List<Armor> armors = new List<Armor>();

        if (!File.Exists(configFilePath))
        {
            Debug.LogError("Config file not found: " + configFilePath);
            return armors;
        }

        string[] lines = File.ReadAllLines(configFilePath);
        Armor currentArmor = null;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] split = line.Split('=');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string value = split[1].Trim();

            switch (key)
            {
                case "ArmorName":
                    if (currentArmor != null) armors.Add(currentArmor);
                    currentArmor = new Armor(value, "", "", 0, 0, "", 0, 0, 0);
                    break;
                case "IconFile":
                    if (currentArmor != null) currentArmor.iconFile = value;
                    break;
                case "Defense":
                    if (currentArmor != null) currentArmor.defense = int.Parse(value);
                    break;
                case "Durability":
                    if (currentArmor != null) currentArmor.durability = int.Parse(value);
                    break;
                case "Description":
                    if (currentArmor != null) currentArmor.description = value;
                    break;
                case "Path":
                    if (currentArmor != null) currentArmor.path = value;
                    break;
            }
        }

        if (currentArmor != null) armors.Add(currentArmor);
        return armors;
    }

    // Load armors from multiple config files
    public static List<Armor> LoadArmorsFromConfigs(List<string> configFilePaths)
    {
        List<Armor> allArmors = new List<Armor>();

        foreach (string configFilePath in configFilePaths)
        {
            List<Armor> armorsFromFile = LoadArmorsFromConfig(configFilePath);
            allArmors.AddRange(armorsFromFile);
        }

        return allArmors;
    }

    // Find an armor by its icon file name
    public static Armor FindArmorByIconFile(string iconFileName, List<Armor> armors)
    {
        string fileName = Path.GetFileName(iconFileName);
        return armors.Find(armor => armor.iconFile == fileName);
    }

    // Display armor info
    public void DisplayArmorInfo()
    {
        Debug.Log($"Armor: {name}");
        Debug.Log($"Defense: {defense}, Durability: {durability}");
        Debug.Log($"Description: {description}");
    }
}

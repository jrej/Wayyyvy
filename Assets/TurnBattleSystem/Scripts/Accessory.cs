using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Accessory : UserObjects
{
    public int magicResistance;
    public int criticalChance;

    // Constructor
    public Accessory(string name, string iconFile, string path, int magicResistance, int criticalChance, string description, int agility, int intelligence, int lifePoints)
        : base(name, description, iconFile, path, agility, intelligence, lifePoints, GearType.Accessory)
    {
        this.magicResistance = magicResistance;
        this.criticalChance = criticalChance;
    }

    public Accessory()
    {
    }

    // Load accessories from a config file
    public static List<Accessory> LoadAccessoriesFromConfig(string configFilePath)
    {
        List<Accessory> accessories = new List<Accessory>();

        if (!File.Exists(configFilePath))
        {
            Debug.LogError("Config file not found: " + configFilePath);
            return accessories;
        }

        string[] lines = File.ReadAllLines(configFilePath);
        Accessory currentAccessory = null;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] split = line.Split('=');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string value = split[1].Trim();

            switch (key)
            {
                case "AccessoryName":
                    if (currentAccessory != null) accessories.Add(currentAccessory);
                    currentAccessory = new Accessory(value, "", "", 0, 0, "", 0, 0, 0);
                    break;
                case "IconFile":
                    if (currentAccessory != null) currentAccessory.iconFile = value;
                    break;
                case "MagicResistance":
                    if (currentAccessory != null) currentAccessory.magicResistance = int.Parse(value);
                    break;
                case "CriticalChance":
                    if (currentAccessory != null) currentAccessory.criticalChance = int.Parse(value);
                    break;
                case "Description":
                    if (currentAccessory != null) currentAccessory.description = value;
                    break;
                case "Path":
                    if (currentAccessory != null) currentAccessory.path = value;
                    break;
            }
        }

        if (currentAccessory != null) accessories.Add(currentAccessory);
        return accessories;
    }

    // Find an accessory by its icon file name
    public static Accessory FindAccessoryByIconFile(string iconFileName, List<Accessory> accessories)
    {
        string fileName = Path.GetFileName(iconFileName);
        return accessories.Find(accessory => accessory.iconFile == fileName);
    }

    // Display accessory info
    public void DisplayAccessoryInfo()
    {
        Debug.Log($"Accessory: {name}");
        Debug.Log($"Magic Resistance: {magicResistance}, Critical Chance: {criticalChance}");
        Debug.Log($"Description: {description}");
    }
}

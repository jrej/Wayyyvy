using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]

public class Other : UserObjects
{
    // Constructor
    public Other(string name, string iconFile, string path, string description, int agility, int intelligence, int lifePoints)
        : base(name, description, iconFile, path, agility, intelligence, lifePoints, GearType.Other)
    {
    }

    public Other()
    {
    }

    // Load others from a config file
    public static List<Other> LoadOthersFromConfig(string configFilePath)
    {
        List<Other> others = new List<Other>();

        if (!File.Exists(configFilePath))
        {
            Debug.LogError("Config file not found: " + configFilePath);
            return others;
        }

        string[] lines = File.ReadAllLines(configFilePath);
        Other currentOther = null;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] split = line.Split('=');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string value = split[1].Trim();

            switch (key)
            {
                case "OtherName":
                    if (currentOther != null) others.Add(currentOther);
                    currentOther = new Other(value, "", "", "", 0, 0, 0);
                    break;
                case "IconFile":
                    if (currentOther != null) currentOther.iconFile = value;
                    break;
                case "Description":
                    if (currentOther != null) currentOther.description = value;
                    break;
                case "Path":
                    if (currentOther != null) currentOther.path = value;
                    break;
            }
        }

        if (currentOther != null) others.Add(currentOther);
        return others;
    }

    // Find an item by its icon file name
    public static Other FindOtherByIconFile(string iconFileName, List<Other> others)
    {
        string fileName = Path.GetFileName(iconFileName);
        return others.Find(other => other.iconFile == fileName);
    }

    // Display item info
    public void DisplayOtherInfo()
    {
        Debug.Log($"Item: {name}");
        Debug.Log($"Description: {description}");
    }
}

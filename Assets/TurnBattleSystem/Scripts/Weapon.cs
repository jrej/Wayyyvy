using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
// In Weapon_class.cs
public class Weapon : UserObjects
{
    public int attack;
    public int speed;
    public int maneuverability;

    // Constructor
    public Weapon(string name, string iconFile,string path, int attack, int speed, int maneuverability, string description,int agility , int intelligence, int lifePoints)
        : base(name, description, iconFile,path ,  agility,  intelligence, lifePoints,GearType.Weapon)
    {
        this.attack = attack;
        this.speed = speed;
        this.maneuverability = maneuverability;
    }

    public Weapon()
    {
        
    }

    // Load weapons from a config file
    public static List<Weapon> LoadWeaponsFromConfig(string configFilePath)
    {
        List<Weapon> weapons = new List<Weapon>();

        if (!File.Exists(configFilePath))
        {
            Debug.LogError("Config file not found: " + configFilePath);
            return weapons;
        }

        string[] lines = File.ReadAllLines(configFilePath);
        Weapon currentWeapon = null;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] split = line.Split('=');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string value = split[1].Trim();

            switch (key)
            {
                case "WeaponName":
                    if (currentWeapon != null) weapons.Add(currentWeapon);
                    currentWeapon = new Weapon(value, "","", 0, 0, 0, "",0,0,0);
                    break;
                case "IconFile":
                    if (currentWeapon != null) currentWeapon.iconFile = value;
                    break;
                case "Attack":
                    if (currentWeapon != null) currentWeapon.attack = int.Parse(value);
                    break;
                case "Speed":
                    if (currentWeapon != null) currentWeapon.speed = int.Parse(value);
                    break;
                case "Maneuverability":
                    if (currentWeapon != null) currentWeapon.maneuverability = int.Parse(value);
                    break;
                case "Description":
                    if (currentWeapon != null) currentWeapon.description = value;
                    break;

                case "Path":
                    if (currentWeapon != null) currentWeapon.path = value;
                    break;
            }
        }

        if (currentWeapon != null) weapons.Add(currentWeapon);
        return weapons;
    }

    // Find a weapon by its icon file name
    public static Weapon FindWeaponByIconFile(string iconFileName, List<Weapon> weapons)
{
    // Extract the file name from the full path (without directories)
    string fileName = Path.GetFileName(iconFileName);

    // Find the weapon whose iconFile matches the extracted file name
    return weapons.Find(weapon => weapon.iconFile == fileName);
}


    // Display weapon info
    public void DisplayWeaponInfo()
    {
        Debug.Log($"Weapon: {name}");
        Debug.Log($"Attack: {attack}, Speed: {speed}, Maneuverability: {maneuverability}");
        Debug.Log($"Description: {description}");
    }
}

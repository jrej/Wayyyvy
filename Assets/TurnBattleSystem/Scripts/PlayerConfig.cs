using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]

public class PlayerConfig
{
    public string playerName;

    public string spriteSheetPath;
    public UserObjects playerHead = new UserObjects();
    public UserObjects playerBody = new UserObjects();
    public UserObjects playerWeapon = new UserObjects();
    public UserObjects playerOffHand = new UserObjects();
    public UserObjects playerLegs = new UserObjects();
    public UserObjects playerFeet = new UserObjects();
    public UserObjects playerRelic = new UserObjects();
    public UserObjects playerCloak = new UserObjects();


    // Player's total stats
    public int totalLifePoints;
    public int totalAttack;
    public int totalAgility;
    public int totalIntelligence;
    public int totalSpeed;



    public void SavePlayerConfig(string configFilePath)
{
    List<string> lines = new List<string>();

    lines.Add("PlayerName=" + playerName);
    lines.Add("PlayerHead=" + playerHead.path);
    lines.Add("PlayerBody=" + playerBody.path);
    lines.Add("PlayerWeapon=" + playerWeapon.path);
    lines.Add("PlayerOffHand=" + playerOffHand.path);
    lines.Add("PlayerLegs=" + playerLegs.path);
    lines.Add("PlayerFeet=" + playerFeet.path);
    lines.Add("PlayerRelic=" + playerRelic.path);
    lines.Add("PlayerCloak=" + playerCloak.path);
    lines.Add("SpriteSheetPAth=" + spriteSheetPath);

    try
    {
        // Write all lines to the file
        File.WriteAllLines(configFilePath, lines.ToArray());
        Debug.Log("Player config saved successfully to: " + configFilePath);
    }
    catch (Exception e)
    {
        Debug.LogError("Failed to save player config: " + e.Message);
    }
}


    public PlayerConfig LoadPlayerConfig(string configFilePath)

    {
        PlayerConfig playerConfig = new PlayerConfig();
        Debug.Log("Srting file  : " + configFilePath);

        if (!File.Exists(configFilePath))
        {
            Debug.LogError("Config file not found: " + configFilePath);
            return null;
        }

        string[] lines = File.ReadAllLines(configFilePath);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] split = line.Split('=');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string value = split[1].Trim();

            Debug.Log(" key : " + key +  "   value : " + value);

            switch (key)
            {
                case "PlayerName":
                    playerConfig.playerName = value;
                    break;
                case "PlayerHead":
                    playerConfig.playerHead.path = value;
                    break;
                case "PlayerBody":
                    playerConfig.playerBody.path = value;
                    break;
                case "PlayerWeapon":
                    playerConfig.playerWeapon.path = value;
                    break;
                case "PlayerOffHand":
                    playerConfig.playerOffHand.path = value;
                    break;
                case "PlayerLegs":
                    playerConfig.playerLegs.path = value;
                    break;
                case "PlayerFeet":
                    playerConfig.playerFeet.path = value;
                    break;
                case "PlayerRelic":
                    playerConfig.playerRelic.path = value;
                    break;
                case "PlayerCloak":
                    playerConfig.playerCloak.path = value;
                    break;
                case "SpriteSheetPath":
                    playerConfig.spriteSheetPath =  value ;
                    break;
            }
        }

        playerConfig.CalculateTotalStats();

        return playerConfig;
    }

    public void DisplayPlayerConfig()
    {
        Debug.Log($"Player Name: {playerName}");
        Debug.Log($"Head: {playerHead.path}, Body: {playerBody.path}, Weapon: {playerWeapon.path}");
        Debug.Log($"OffHand: {playerOffHand.path}, Legs: {playerLegs.path}, Feet: {playerFeet.path}");
        Debug.Log($"Relic: {playerRelic.path}, Cloak: {playerCloak.path}");
    }
    
    
    private void AddGearStats(UserObjects gear)
{
    if (gear != null)
    {
        totalLifePoints += gear.lifePoints;
        totalAgility += gear.agility;
        totalIntelligence += gear.intelligence;

        // Check if the gear is of type Weapon
        if (gear is Weapon weapon)
        {
            totalAttack += weapon.attack;
            totalSpeed += weapon.speed;
        }
    }
}




    // Method to calculate total stats based on equipped items
    public void CalculateTotalStats()
    {
        totalLifePoints = 0;
        totalAttack = 0;
        totalAgility = 0;
        totalIntelligence = 0;
        totalSpeed = 0;

        AddGearStats(playerHead);
        AddGearStats(playerBody);
        AddGearStats(playerWeapon);
        AddGearStats(playerOffHand);
        AddGearStats(playerLegs);
        AddGearStats(playerFeet);
        AddGearStats(playerRelic);
        AddGearStats(playerCloak);
        
    }
}


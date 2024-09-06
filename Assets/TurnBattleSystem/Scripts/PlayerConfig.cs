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
    public UserObjects playerHead = new UserObjects();
    public UserObjects playerBody = new UserObjects();
    public UserObjects playerWeapon = new UserObjects();
    public UserObjects playerOffHand = new UserObjects();
    public UserObjects playerLegs = new UserObjects();
    public UserObjects playerFeet = new UserObjects();
    public UserObjects playerRelic = new UserObjects();
    public UserObjects playerCloak = new UserObjects();

    public static PlayerConfig LoadPlayerConfig(string configFilePath)
    {
        PlayerConfig playerConfig = new PlayerConfig();

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
            }
        }

        return playerConfig;
    }

    public void DisplayPlayerConfig()
    {
        Debug.Log($"Player Name: {playerName}");
        Debug.Log($"Head: {playerHead.path}, Body: {playerBody.path}, Weapon: {playerWeapon.path}");
        Debug.Log($"OffHand: {playerOffHand.path}, Legs: {playerLegs.path}, Feet: {playerFeet.path}");
        Debug.Log($"Relic: {playerRelic.path}, Cloak: {playerCloak.path}");
    }
}


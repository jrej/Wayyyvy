using System.Collections.Generic;
using System.IO;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.InventorySystem.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Assets.HeroEditor.InventorySystem.Scripts.Elements;
using UnityEngine.UI;
using System.Collections;
using System;

[System.Serializable]
public class ItemAbility
{
    public string name;
    public string description;
    public List<string> abilities;
    public string image_path;
}

[System.Serializable]
public class ItemAbilityList
{
    public ItemAbility[] items;
}
public class CharacterItemExtractor : MonoBehaviour
{
    public InventoryBase inventory;

    // Initialize inventory
    public void Start()
    {
        // Find the InventoryBase in the scene
        inventory = FindObjectOfType<InventoryBase>();

        if (inventory == null)
        {
            Debug.LogError("InventoryBase not found in the scene.");
            return;
        }

        // Extract and display the item info
        ExtractItemInfo();
    }

    // Function to extract item names and descriptions based on the player's equipped items
    public void ExtractItemInfo()
    {
        List<string> itemInfoList = new List<string>();
       // ItemParams itemparam;
        // Iterate over the equipped items in the inventory
        Equipment equipement = inventory.Equipment;

        equipement.GetEquippedItemsInfo();
    }
}


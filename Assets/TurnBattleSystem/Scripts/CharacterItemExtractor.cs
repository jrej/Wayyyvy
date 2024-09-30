using System.Collections.Generic;
using System.IO;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;

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
    private Dictionary<string, ItemAbility> itemAbilities;

    // Call this method to load JSON data
    public void LoadItemData(string jsonFilePath)
    {
        // Read JSON from the file
        string json = File.ReadAllText(jsonFilePath);
        
        // Create an instance of ItemAbilityList
        ItemAbilityList itemAbilityList = JsonUtility.FromJson<ItemAbilityList>(json);

        // Populate the dictionary from the item list
        itemAbilities = new Dictionary<string, ItemAbility>();
        foreach (var item in itemAbilityList.items)
        {
            // Use only the file name as the key
            itemAbilities[Path.GetFileName(item.image_path)] = item;
        }
    }

    // Function to extract item descriptions and abilities based on the character's sprites
    public void ExtractItemInfo(Character character)
    {
        // Load item data from JSON
        LoadItemData("items_data.json"); // Update with your actual path

        List<ItemAbility> extractedAbilities = new List<ItemAbility>();

        // Check for each sprite in the character
        if (character.HelmetRenderer.sprite != null)
        {
            string spriteName = Path.GetFileName(character.HelmetRenderer.sprite.name);
            if (itemAbilities.TryGetValue(spriteName, out ItemAbility ability))
            {
                extractedAbilities.Add(ability);
            }
        }

        if (character.ArmorRenderers != null)
        {
            foreach (var armorRenderer in character.ArmorRenderers)
            {
                if (armorRenderer.sprite != null)
                {
                    string spriteName = Path.GetFileName(armorRenderer.sprite.name);
                    if (itemAbilities.TryGetValue(spriteName, out ItemAbility ability))
                    {
                        extractedAbilities.Add(ability);
                    }
                }
            }
        }

        if (character.PrimaryMeleeWeaponRenderer.sprite != null)
        {
            string spriteName = Path.GetFileName(character.PrimaryMeleeWeaponRenderer.sprite.name);
            if (itemAbilities.TryGetValue(spriteName, out ItemAbility ability))
            {
                extractedAbilities.Add(ability);
            }
        }

        // Example for Shield
        if (character.ShieldRenderer.sprite != null)
        {
            string spriteName = Path.GetFileName(character.ShieldRenderer.sprite.name);
            if (itemAbilities.TryGetValue(spriteName, out ItemAbility ability))
            {
                extractedAbilities.Add(ability);
            }
        }

        // Log or use the extracted abilities
        foreach (var item in extractedAbilities)
        {
            Debug.Log($"Item: {item.name}, Description: {item.description}, Abilities: {string.Join(", ", item.abilities)}");
        }
    }
}

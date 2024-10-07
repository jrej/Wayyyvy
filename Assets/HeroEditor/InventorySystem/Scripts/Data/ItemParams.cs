using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Scripts.Common;
using Assets.HeroEditor.InventorySystem.Scripts.Enums;
using UnityEngine;
namespace Assets.HeroEditor.InventorySystem.Scripts.Data
{
    /// <summary>
    /// Represents generic item params (common for all items).
    /// </summary>
    [Serializable]
    public class ItemParams
    {
	    public string Id;
        public int Level;
        public ItemRarity Rarity;
        public ItemType Type;
        public ItemClass Class;
        public List<ItemTag> Tags = new List<ItemTag>();
        public List<Property> Properties = new List<Property>();
        public int Price;
        public int Weight;
        public string IconId;
        public string SpriteId;
        public string Meta;
        public List<LocalizedValue> Localization = new List<LocalizedValue>();

        public Property FindProperty(PropertyId id)
        {
            var target = Properties.SingleOrDefault(i => i.Id == id);

            return target;
        }

        public Property FindProperty(PropertyId id, ElementId element)
        {
            var target = Properties.SingleOrDefault(i => i.Id == id);

            return target;
        }

        public string GetLocalizedName(string language)
        {
            var localized = Localization.SingleOrDefault(i => i.Language == language) ?? Localization.SingleOrDefault(i => i.Language == "English");
            
            return localized == null ? Id : localized.Value;
        }

        public List<string> MetaToList()
        {
            return Meta.IsEmpty() ? new List<string>() : Serializer.DeserializeList(Meta);
        }


        public void DebugDisplay()
        {
            // Display basic item information
            Debug.Log($"Item ID: {Id}");
            Debug.Log($"Level: {Level}");
            Debug.Log($"Rarity: {Rarity}");
            Debug.Log($"Type: {Type}");
            Debug.Log($"Class: {Class}");
            Debug.Log($"Price: {Price}");
            Debug.Log($"Weight: {Weight}");

            // Display tags
            if (Tags.Any())
            {
                Debug.Log($"Tags: {string.Join(", ", Tags.Select(t => t.ToString()))}");
            }
            else
            {
                Debug.Log("Tags: None");
            }

            // Display properties
            if (Properties.Any())
            {
                Debug.Log("Properties:");
                foreach (var property in Properties)
                {
                    Debug.Log($"- {property.Id}: {property.Value}"); // Assuming Property has an Id and Value property
                }
            }
            else
            {
                Debug.Log("Properties: None");
            }

            // Display localized names
            var localizedName = GetLocalizedName("English");
            Debug.Log($"Localized Name: {localizedName}");
        }


    }


    
}
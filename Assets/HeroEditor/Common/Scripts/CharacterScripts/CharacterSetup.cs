using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Scripts.Data;
using Assets.HeroEditor.Common.Scripts.CharacterScripts.Firearms.Enums;
using Assets.HeroEditor.InventorySystem.Scripts;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Assets.HeroEditor.InventorySystem.Scripts.Enums;
using HeroEditor.Common.Data;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.Scripts.CharacterScripts
{
    public partial class Character
    {
        /// <summary>
        /// Set character's body parts.
        /// </summary>
		public override void SetBody(ItemSprite item, BodyPart part, Color? color)
        {
            switch (part)
            {
                case BodyPart.Body:
                    Body = item?.Sprites;
					break;
                case BodyPart.Head:
                    Head = item?.Sprite;
					break;
                case BodyPart.Hair:
                    Hair = item?.Sprite;
                    FullHair = Helmet == null || FullHair;
                    HideEars = item != null && item.Tags.Contains("HideEars");
                    HairRenderer.color = color ?? HairRenderer.color;
                    if (item != null && item.Tags.Contains("NoPaint")) HairRenderer.color = Color.white;
                    break;
                case BodyPart.Ears:
                    Ears = item?.Sprite;
                    break;
                case BodyPart.Eyebrows:
                    Expressions[0].Eyebrows = item?.Sprite;
                    break;
                case BodyPart.Eyes:
                    Expressions[0].Eyes = item?.Sprite;
                    Expressions.Where(i => i.Name != "Dead").ToList().ForEach(i => i.EyesColor = color ?? EyesRenderer.color);
                    EyesRenderer.color = color ?? EyesRenderer.color;
                    break;
                case BodyPart.Mouth:
                    Expressions[0].Mouth = item?.Sprite;
                    break;
                case BodyPart.Beard:
                    Beard = item?.Sprite;
                    BeardRenderer.color = color ?? BeardRenderer.color;
                    break;
                default: throw new NotImplementedException($"Unsupported part: {part}.");
            }

            Initialize();
		}

        public override void SetBody(ItemSprite item, BodyPart part)
        {
            SetBody(item, part, null);
        }

        /// <summary>
        /// Set character's expression.
        /// </summary>
        public override void SetExpression(string expression)
        {
            if (Expressions.Count < 3) throw new Exception("Character must have at least 3 basic expressions: Default, Angry and Dead.");

            var e = Expressions.Single(i => i.Name == expression);

            Expression = expression;
            EyebrowsRenderer.sprite = e.Eyebrows;
            EyesRenderer.sprite = e.Eyes;
            EyesRenderer.color = e.EyesColor;
            MouthRenderer.sprite = e.Mouth;

            if (EyebrowsRenderer.sprite == null) EyebrowsRenderer.sprite = Expressions[0].Eyebrows;
            if (EyesRenderer.sprite == null) EyesRenderer.sprite = Expressions[0].Eyes;
            if (MouthRenderer.sprite == null) MouthRenderer.sprite = Expressions[0].Mouth;
        }

		/// <summary>
		/// Equip something from SpriteCollection.
		/// </summary>
		public override void Equip(ItemSprite item, EquipmentPart part, Color? color)
        {
            switch (part)
            {
                case EquipmentPart.MeleeWeapon1H:
                case EquipmentPart.MeleeWeapon2H:
                case EquipmentPart.MeleeWeaponPaired:
                case EquipmentPart.Bow:
                    Firearms = null;
                    FirearmsRenderers.ForEach(i => i.sprite = null);
                    break;
                case EquipmentPart.Firearm1H:
                case EquipmentPart.Firearm2H:
                    PrimaryMeleeWeapon = SecondaryMeleeWeapon = null;
                    PrimaryMeleeWeaponRenderer.sprite = SecondaryMeleeWeaponRenderer.sprite = null;
                    break;
            }

            switch (part)
            {
                case EquipmentPart.Helmet:
                    Helmet = item?.Sprite;
                    HelmetRenderer.color = color ?? HelmetRenderer.color;
                    FullHair = item == null || item.Tags.Contains("FullHair");
                    break;
                case EquipmentPart.Armor:
                    Armor = item?.Sprites.ToList();
                    ArmorRenderers.ForEach(i => i.color = color ?? i.color);
                    break;
                case EquipmentPart.Pauldrons:
                case EquipmentPart.Vest:
                case EquipmentPart.Gloves:
                case EquipmentPart.Belt:
                case EquipmentPart.Boots:
                    foreach (var p in GetEquipmentSubPartNames(part))
                    {
                        SetArmorParts(p, item?.Sprites);
                    }
                    break;
                case EquipmentPart.MeleeWeapon1H:
                    PrimaryMeleeWeapon = item?.Sprite;
                    PrimaryMeleeWeaponRenderer.color = color ?? (item != null && item.Tags.Contains("Paint") ? PrimaryMeleeWeaponRenderer.color : Color.white);
                    if (WeaponType != WeaponType.MeleePaired) WeaponType = WeaponType.Melee1H;
					break;
                case EquipmentPart.MeleeWeapon2H:
                    PrimaryMeleeWeapon = item?.Sprite;
                    PrimaryMeleeWeaponRenderer.color = color ?? (item != null && item.Tags.Contains("Paint") ? PrimaryMeleeWeaponRenderer.color : Color.white);
                    WeaponType = WeaponType.Melee2H;
					break;
                case EquipmentPart.MeleeWeaponPaired:
                    if (WeaponType == WeaponType.Melee2H) PrimaryMeleeWeapon = null;
                    SecondaryMeleeWeapon = item?.Sprite;
                    WeaponType = WeaponType.MeleePaired;
					break;
                case EquipmentPart.Bow:
                    Bow = item?.Sprites.ToList();
                    WeaponType = WeaponType.Bow;
					break;
                case EquipmentPart.Firearm1H:
                    Firearms = item?.Sprites.ToList();
                    WeaponType = WeaponType.Firearm1H;
					break;
                case EquipmentPart.Firearm2H:
                    Firearms = item?.Sprites.ToList();
                    WeaponType = WeaponType.Firearm2H;
					break;
                case EquipmentPart.Shield:
                    Shield = item?.Sprite;
                    WeaponType = WeaponType.Melee1H;
					break;
                case EquipmentPart.Cape:
                    Cape = item?.Sprite;
                    CapeRenderer.color = color ?? CapeRenderer.color;
                    break;
                case EquipmentPart.Back:
                    Back = item?.Sprite;
                    BackRenderer.color = color ?? BackRenderer.color;
					break;
                case EquipmentPart.Earrings:
                    Earrings = item?.Sprite;
                    EarringsRenderer.color = color ?? EarringsRenderer.color;
                    break;
                case EquipmentPart.Glasses:
                    Glasses = item?.Sprite;
                    GlassesRenderer.color = color ?? GlassesRenderer.color;
                    break;
                case EquipmentPart.Mask:
                    Mask = item?.Sprite;
                    MaskRenderer.color = color ?? MaskRenderer.color;
                    break;
                default: throw new NotImplementedException($"Unsupported part: {part}.");
            }

            Initialize();
		}

        public override void Equip(ItemSprite item, EquipmentPart part)
        {
            Equip(item, part, null);
        }

        /// <summary>
        /// Remove equipment partially.
        /// </summary>
        public override void UnEquip(EquipmentPart part)
        {
			Equip(null, part);
        }

        /// <summary>
        /// Equip item.
        /// </summary>
        /// <param name="item"></param>
        public void Equip(Item item)
        {
            var itemParams = ItemCollection.Active.GetItemParams(item);

            switch (itemParams.Type)
            {
                case ItemType.Helmet: Equip(item.Sprite, EquipmentPart.Helmet); break;
                case ItemType.Armor: Equip(item.Sprite, EquipmentPart.Armor); break;
                case ItemType.Shield: Equip(item.Sprite, EquipmentPart.Shield); break;
                case ItemType.Weapon:
                {
                    switch (itemParams.Class)
                    {
                        case ItemClass.Bow: Equip(item.Sprite, EquipmentPart.Bow); break;
                        case ItemClass.Firearm:
                            Equip(item.Sprite, itemParams.Tags.Contains(ItemTag.TwoHanded)
                                    ? EquipmentPart.Firearm2H
                                    : EquipmentPart.Firearm1H);
                            break;
                        default:
                            Equip(item.Sprite, itemParams.Tags.Contains(ItemTag.TwoHanded)
                                    ? EquipmentPart.MeleeWeapon2H
                                    : EquipmentPart.MeleeWeapon1H);
                            break;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Remove all equipment.
        /// </summary>
        public override void ResetEquipment()
        {
            Armor = null;
            Bow = null;
            Firearms = null;
            Helmet = Cape = Back = PrimaryMeleeWeapon = SecondaryMeleeWeapon = Shield = Mask = Glasses = Earrings = null;
            WeaponType = WeaponType.Melee1H;
            HideEars = false;
            FullHair = true;
            Initialize();
        }
        /// <summary>
/// Display currently equipped equipment for the character.
/// </summary>
public void DisplayEquipment()
{
    Debug.Log($"Equipment for {this}:");

    // Display armor
    if (Armor != null && Armor.Count > 0)
    {
        Debug.Log("Armor:");
        foreach (var armorPiece in Armor)
        {
            Debug.Log($" - {armorPiece.name}");
        }
    }
    else
    {
        Debug.Log("No armor equipped.");
    }

    // Display weapons
    if (PrimaryMeleeWeapon != null)
    {
        Debug.Log($"Primary Melee Weapon: {PrimaryMeleeWeapon.name}");
    }
    else
    {
        Debug.Log("No primary melee weapon equipped.");
    }

    if (SecondaryMeleeWeapon != null)
    {
        Debug.Log($"Secondary Melee Weapon: {SecondaryMeleeWeapon.name}");
    }
    else
    {
        Debug.Log("No secondary melee weapon equipped.");
    }

    if (Bow != null)
    {
        Debug.Log($"Bow: {Bow.First().name}");
    }
    else
    {
        Debug.Log("No bow equipped.");
    }

    if (Firearms != null && Firearms.Count > 0)
    {
        Debug.Log("Firearms:");
        foreach (var firearm in Firearms)
        {
            Debug.Log($" - {firearm.name}");
        }
    }
    else
    {
        Debug.Log("No firearms equipped.");
    }

    // Display head equipment
    if (Helmet != null)
    {
        Debug.Log($"Helmet: {Helmet.name}");
    }
    else
    {
        Debug.Log("No helmet equipped.");
    }

    // Display other equipment
    if (Cape != null)
    {
        Debug.Log($"Cape: {Cape.name}");
    }
    if (Back != null)
    {
        Debug.Log($"Back: {Back.name}");
    }
    if (Mask != null)
    {
        Debug.Log($"Mask: {Mask.name}");
    }
    if (Glasses != null)
    {
        Debug.Log($"Glasses: {Glasses.name}");
    }
    if (Earrings != null)
    {
        Debug.Log($"Earrings: {Earrings.name}");
    }

    // Add more equipment displays as necessary
}


		private void SetArmorParts(string part, List<Sprite> armor)
	    {
		    var sprite = armor?.SingleOrDefault(j => j.name == part);

            Armor?.RemoveAll(i => i == null || i.name == part);
            
            if (sprite != null)
            {
                if (Armor == null) Armor = new List<Sprite>();

                Armor.Add(sprite);
            }
	    }

		private void BuildFirearms(FirearmParams firearmParams)
        {
            if (firearmParams == null) return;

            Firearm.Params = firearmParams; // TODO:
		    Firearm.SlideTransform.localPosition = firearmParams.SlidePosition;
		    Firearm.MagazineTransform.localPosition = firearmParams.MagazinePosition;
		    Firearm.FireTransform.localPosition = firearmParams.FireMuzzlePosition;
		    Firearm.AmmoShooted = 0;
            Firearm.Fire.SetLamp(Firearm.Params.LoadType == FirearmLoadType.Lamp ? firearmParams.GetColorFromMeta("LampReady") : Color.white);
        }

		private static void MapSprites(List<SpriteRenderer> spriteRenderers, List<Sprite> sprites)
        {
			foreach (var part in spriteRenderers)
            {
                part.sprite = sprites?.SingleOrDefault(i => i != null && i.name == part.name.Split('[')[0]);
            }
        }
    }
}
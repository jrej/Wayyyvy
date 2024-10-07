using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Scripts.Common;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Assets.HeroEditor.InventorySystem.Scripts.Elements;
using Assets.HeroEditor.InventorySystem.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.InventorySystem.Scripts
{
    public class InventoryBase : ItemWorkspace
    {
        public Equipment Equipment;
        public ScrollInventory PlayerInventory;
        public ScrollInventory Materials;
        public Button EquipButton;
        public Button RemoveButton;
        public Button CraftButton;
        public Button LearnButton;
        public Button UseButton;
        public AudioClip EquipSound;
        public AudioClip CraftSound;
        public AudioClip UseSound;
        public AudioSource AudioSource;
        public bool InitializeExample;

        public Func<Item, bool> CanEquip = i => true;
        public Action<Item> OnEquip;

        public void Awake()
        {
            Debug.Log("Awake called - Initializing ItemCollection");
            ItemCollection.Active = ItemCollection;
        }

        public void Start()
        {
            Debug.Log("Start called - InitializeExample: " + InitializeExample);
            if (InitializeExample)
            {
                TestInitialize();
            }
        }

        public void TestInitialize()
        {
            Debug.Log("TestInitialize called");
            var inventory = ItemCollection.Active.Items.Select(i => new Item(i.Id)).ToList();
            var equipped = new List<Item>();

            RegisterCallbacks();
            PlayerInventory.Initialize(ref inventory);
            Equipment.Initialize(ref equipped);
            Debug.Log("Player inventory and equipment initialized in TestInitialize");
        }

        public void Initialize(ref List<Item> playerItems, ref List<Item> equippedItems, int bagSize, Action onRefresh)
        {
            Debug.Log("Initialize called with playerItems: " + playerItems.Count + ", equippedItems: " + equippedItems.Count);
            RegisterCallbacks();
            PlayerInventory.Initialize(ref playerItems);
            Equipment.SetBagSize(bagSize);
            Equipment.Initialize(ref equippedItems);
            Equipment.OnRefresh = onRefresh;

            Debug.Log("Equipment and player inventory initialized.");

            if (!Equipment.SelectAny() && !PlayerInventory.SelectAny())
            {
                ItemInfo.Reset();
                Debug.Log("No item selected. Resetting ItemInfo.");
            }
        }

        public void RegisterCallbacks()
        {
            Debug.Log("RegisterCallbacks called");
            InventoryItem.OnLeftClick = SelectItem;
            InventoryItem.OnRightClick = InventoryItem.OnDoubleClick = QuickAction;
        }

        public void SelectItem(Item item)
        {
            Debug.Log("SelectItem called for item: " + item.Id);
            SelectedItem = item;
            ItemInfo.Initialize(SelectedItem, SelectedItem.Params.Price);
            Refresh();
        }

        private void QuickAction(Item item)
        {
            Debug.Log("QuickAction called for item: " + item.Id);
            SelectItem(item);

            if (Equipment.Items.Contains(item))
            {
                Debug.Log("Item is already equipped. Removing it.");
                Remove();
            }
            else if (CanEquipSelectedItem())
            {
                Debug.Log("Equipping selected item.");
                Equip();
            }
        }

        public void Equip()
        {
            Debug.Log("Equip called for item: " + SelectedItem.Id);

            if (!CanEquip(SelectedItem))
            {
                Debug.Log("Cannot equip item: " + SelectedItem.Id);
                return;
            }

            var equipped = SelectedItem.IsFirearm
                ? Equipment.Items.Where(i => i.IsFirearm).ToList()
                : Equipment.Items.Where(i => i.Params.Type == SelectedItem.Params.Type && !i.IsFirearm).ToList();

            if (equipped.Any())
            {
                Debug.Log("Item(s) already equipped in the same slot. Auto-removing them.");
                AutoRemove(equipped, Equipment.Slots.Count(i => i.Supports(SelectedItem)));
            }

            if (SelectedItem.IsTwoHanded) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            if (SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());

            if (SelectedItem.IsFirearm)
            {
                AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
                AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());
            }

            if (SelectedItem.IsTwoHanded || SelectedItem.IsShield)
            {
                AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsFirearm).ToList());
            }

            MoveItem(SelectedItem, PlayerInventory, Equipment);
            Debug.Log("Item moved from PlayerInventory to Equipment.");

            OnEquip?.Invoke(SelectedItem);
        }

        public void Remove()
        {
            Debug.Log("Remove called for item: " + SelectedItem.Id);
            MoveItem(SelectedItem, Equipment, PlayerInventory);
            SelectItem(SelectedItem);
        }

        public void Craft()
        {
            Debug.Log("Craft called");
            var materials = MaterialList;

            if (CanCraft(materials))
            {
                Debug.Log("Materials found for crafting.");
                materials.ForEach(i => PlayerInventory.Items.Single(j => j.Hash == i.Hash).Count -= i.Count);
                PlayerInventory.Items.RemoveAll(i => i.Count == 0);

                var itemId = SelectedItem.Params.FindProperty(PropertyId.Craft).Value;
                var existed = PlayerInventory.Items.SingleOrDefault(i => i.Id == itemId && i.Modifier == null);

                if (existed == null)
                {
                    PlayerInventory.Items.Add(new Item(itemId));
                }
                else
                {
                    existed.Count++;
                }

                PlayerInventory.Refresh(SelectedItem);
                CraftButton.interactable = CanCraft(materials);
                AudioSource.PlayOneShot(CraftSound, SfxVolume);
            }
            else
            {
                Debug.Log("No materials available for crafting.");
            }
        }

        public void Use()
        {
            Debug.Log("Use called for item: " + SelectedItem.Id);
            var sound = SelectedItem.Params.Type == ItemType.Coupon ? EquipSound : UseSound;

            if (SelectedItem.Count == 1)
            {
                PlayerInventory.Items.Remove(SelectedItem);
                SelectedItem = PlayerInventory.Items.FirstOrDefault();

                if (SelectedItem == null)
                {
                    PlayerInventory.Refresh(null);
                    SelectedItem = Equipment.Items.FirstOrDefault();

                    if (SelectedItem != null)
                    {
                        Equipment.Refresh(SelectedItem);
                    }
                }
                else
                {
                    PlayerInventory.Refresh(SelectedItem);
                }
            }
            else
            {
                SelectedItem.Count--;
                PlayerInventory.Refresh(SelectedItem);
            }

            Equipment.OnRefresh?.Invoke();
            AudioSource.PlayOneShot(sound, SfxVolume);
        }

        public override void Refresh()
        {
            Debug.Log("Refresh called.");
            if (SelectedItem == null)
            {
                Debug.Log("No item selected. Resetting ItemInfo.");
                ItemInfo.Reset();
                EquipButton.SetActive(false);
                RemoveButton.SetActive(false);
            }
            else
            {
                var equipped = Equipment.Items.Contains(SelectedItem);
                Debug.Log("SelectedItem is equipped: " + equipped);

                EquipButton.SetActive(!equipped && CanEquipSelectedItem());
                RemoveButton.SetActive(equipped);
                UseButton.SetActive(CanUse());
            }

            var receipt = SelectedItem != null && SelectedItem.Params.Type == ItemType.Recipe;
            if (CraftButton != null) CraftButton.SetActive(receipt);
            if (LearnButton != null) LearnButton.SetActive(receipt);
        }

        private List<Item> MaterialList => SelectedItem.Params.FindProperty(PropertyId.Materials).Value
            .Split(',')
            .Select(i => i.Split(':'))
            .Select(i => new Item(i[0], int.Parse(i[1])))
            .ToList();

        private bool CanEquipSelectedItem()
        {
            Debug.Log("CanEquipSelectedItem called for item: " + SelectedItem.Id);
            return PlayerInventory.Items.Contains(SelectedItem) && Equipment.Slots.Any(i => i.Supports(SelectedItem)) && SelectedItem.Params.Class != ItemClass.Booster;
        }

        private bool CanUse()
        {
            Debug.Log("CanUse called for item: " + SelectedItem.Id);
            return SelectedItem.Params.Class == ItemClass.Booster || SelectedItem.Params.Type == ItemType.Coupon;
        }

        private bool CanCraft(List<Item> materials)
        {
            Debug.Log("CanCraft called. Checking materials...");
            return materials.All(i => PlayerInventory.Items.Any(j => j.Hash == i.Hash && j.Count >= i.Count));
        }

        private void AutoRemove(List<Item> items, int max = 1)
        {
            Debug.Log("AutoRemove called for items.");
            long sum = 0;

            foreach (var p in items)
            {
                sum += p.Count;
            }

            if (sum == max)
            {
                Debug.Log("AutoRemoving extra item.");
                MoveItemSilent(items.LastOrDefault(i => i.Id != SelectedItem.Id) ?? items.Last(), Equipment, PlayerInventory);
            }
        }
    }
}

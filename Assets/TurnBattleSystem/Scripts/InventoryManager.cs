using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    public GameObject DescritionSlot;
    private bool active;
    private string weaponsFolderPath = "Assets/TurnBattleSystem/Textures/Weapons";
    private string bodiesFolderPath = "Assets/TurnBattleSystem/Textures/Bodies2";

    private string PlayerSpritePath = "Assets/TurnBattleSystem/Textures/PlayerSprite.png";

    private string PlayerHeadPath = "Assets/TurnBattleSystem/Textures/head.png";

    private string PlayerWeaponHandPath = "Assets/TurnBattleSystem/Textures/weapon.png";

    private string PlayerOffHandPath = "Assets/TurnBattleSystem/Textures/offhand.png";
    
    private string PlayerRelicPath = "Assets/TurnBattleSystem/Textures/relic.png";
    
    private string PlayerFeetPath = "Assets/TurnBattleSystem/Textures/feet.png";

    private string PlayerCloakPath = "Assets/TurnBattleSystem/Textures/cloak.png";

    private string PlayerBodyPath = "Assets/TurnBattleSystem/Textures/body.png";

    private string PlayerLegsPath = "Assets/TurnBattleSystem/Textures/legs.png";


    private BattleHandler battleHandler;

    private List<Weapon> loadedWeapons;

public GameObject[] ItemSlots;
     private Image playerSpriteImage;
     private Image playerSpriteGearImage;
private Image playerSlotsImage;

     private Image playerSpriteHeadImage;
     private Image playerSpriteWeaponHandImage;

     private Image playerSpriteOffHandImage ; 

     private Image playerSpriteRelicImage ; 

     private Image playerSpriteFeetImage ; 

     private Image playerSpriteCloakImage ; 

     private Image playerSpriteBodyImage ; 

     private Image playerSpriteLegsImage ; 


void LoadPlayerSprite()
    {
        // Find the PlayerSprite GameObject
        GameObject playerSpriteObject = GameObject.Find("PlayerSprite");


        if (playerSpriteObject != null )
        {
            // Get the Image component
            playerSpriteImage = playerSpriteObject.GetComponent<Image>();

            if (playerSpriteImage != null)
            {
                LoadPlayerSpriteImage(PlayerSpritePath);
            }
            else
            {
                Debug.LogError("Image component not found on PlayerSprite!");
            }
        }
        else
        {
            Debug.LogError("PlayerSprite GameObject not found!");
        }




    }


void ModifyPlayerGear(UserObjects obj, string name)
{
    // Find the InventoryMenu object first
    Transform inventoryMenuTransform = transform.Find("InventoryMenu");

    if (inventoryMenuTransform != null)
    {
        // Get all children of InventoryMenu
        Transform[] allChildren = inventoryMenuTransform.GetComponentsInChildren<Transform>();
        // Get all children of InventoryMenu (direct children)
        foreach (Transform child in inventoryMenuTransform)
        {
            // Check if the child's name matches
            if (child.name == name)
            {
                Image playerSpriteHeadImage = child.GetComponent<Image>();

                if (playerSpriteHeadImage != null)
                {
                    string fullPath = System.IO.Path.Combine(obj.path, obj.iconFile);
                    LoadPlayerSpriteGearImage(playerSpriteHeadImage, fullPath);
                }
                else
                {
                    Debug.LogError("Image component not found on " + name + "!");
                }

                // Exit loop once the correct child is found
                break;
            }
             Debug.LogError("Child not fpound " + name + "!");

        }
    }
    else
    {
        Debug.LogError("InventoryMenu GameObject not found!");
    }
}


    void LoadPlayerSpriteGear()
{
    // Find the InventoryMenu object first
    Transform inventoryMenuTransform = transform.Find("InventoryMenu");

    if (inventoryMenuTransform != null)
    {
        // Get all children of InventoryMenu
        Transform[] allChildren = inventoryMenuTransform.GetComponentsInChildren<Transform>();

        // Find the PlayerSpriteGear object by name within the children
        foreach (Transform child in allChildren)
        {
            if (child.name == "PlayerSpriteGear")
            {
                playerSpriteGearImage = child.GetComponent<Image>();

                if (playerSpriteGearImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteGearImage,PlayerSpritePath);
                }
                else
                {
                    Debug.LogError("Image component not found on PlayerSpriteGear!");
                }

            }

            if (child.name == "Head")
            {
                playerSpriteHeadImage = child.GetComponent<Image>();

                if (playerSpriteHeadImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteHeadImage,PlayerHeadPath);
                }
                else
                {
                    Debug.LogError("Image component not found on PlayerSpriteGear!");
                }

            }

            if (child.name == "Weapon")
            {
                playerSpriteWeaponHandImage = child.GetComponent<Image>();

                if (playerSpriteWeaponHandImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteWeaponHandImage,PlayerWeaponHandPath);
                }
                else
                {
                    Debug.LogError("Image component not found on WeaponHand!");
                }
            }

            if (child.name == "Offhand")
            {
                playerSpriteOffHandImage = child.GetComponent<Image>();

                if (playerSpriteOffHandImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteOffHandImage,PlayerOffHandPath);
                }
                else
                {
                    Debug.LogError("Image component not found on OffHand!");
                }
            }

            if (child.name == "Relic")
            {
                playerSpriteRelicImage = child.GetComponent<Image>();

                if (playerSpriteRelicImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteRelicImage,PlayerRelicPath);
                }
                else
                {
                    Debug.LogError("Image component not found on OffHand!");
                }
            }

            if (child.name == "Feet")
            {
                playerSpriteFeetImage = child.GetComponent<Image>();

                if (playerSpriteFeetImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteFeetImage,PlayerFeetPath);
                }
                else
                {
                    Debug.LogError("Image component not found on OffHand!");
                }
            }

            if (child.name == "Cloak")
            {
                playerSpriteCloakImage = child.GetComponent<Image>();

                if (playerSpriteCloakImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteCloakImage,PlayerCloakPath);
                }
                else
                {
                    Debug.LogError("Image component not found on OffHand!");
                }
            }

            if (child.name == "Body")
            {
                playerSpriteBodyImage = child.GetComponent<Image>();

                if (playerSpriteBodyImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteBodyImage,PlayerBodyPath);
                }
                else
                {
                    Debug.LogError("Image component not found on OffHand!");
                }
            }

            if (child.name == "Legs")
            {
                playerSpriteLegsImage = child.GetComponent<Image>();

                if (playerSpriteLegsImage != null)
                {
                    LoadPlayerSpriteGearImage(playerSpriteLegsImage,PlayerLegsPath);
                }
                else
                {
                    Debug.LogError("Image component not found on OffHand!");
                }
            }



        }

    }
    else
    {
        Debug.LogError("InventoryMenu GameObject not found!");
    }
}


    void LoadPlayerSpriteImage(string imagePath)
    {
        if (File.Exists(imagePath))
        {
            // Load the texture from the file
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            // Create a sprite from the texture
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            playerSpriteImage.sprite = newSprite;
        }
        else
        {
            Debug.LogError($"Image file not found at path: {imagePath}");
        }
    }

    void LoadPlayerSpriteGearImage(Image img, string imagePath)
    {
        if (File.Exists(imagePath))
        {
            // Load the texture from the file
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            // Create a sprite from the texture
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            img.sprite = newSprite;
        }
        else
        {
            Debug.LogError($"Image file not found at path: {imagePath}");
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        active = false;
        InventoryMenu.SetActive(false);
        
        battleHandler = FindObjectOfType<BattleHandler>();
        //itemSlots = InventoryMenu.GetComponentsInChildren<Transform>();
        LoadPlayerSprite();
        LoadPlayerSpriteGear();
        LoadWeaponImages();
        // Load all weapons from the  config file at the start
        loadedWeapons = Weapon.LoadWeaponsFromConfig("Assets/TurnBattleSystem/weapons_config.txt");
    
       // LoadBodyImages();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (active)
            {
                Debug.Log("Deactivating inventory menu");
                Time.timeScale = 1;
                InventoryMenu.SetActive(false);
                active = false;
            }
            else
            {
                Debug.Log("Activating inventory menu");
                Time.timeScale = 0;
                InventoryMenu.SetActive(true);
                active = true;
            }
        }
    }



void ModifyDescription(string name, string description)
{
    // Find the InventoryMenu object first
    Transform inventoryMenuTransform = transform.Find("InventoryMenu");

    // Validate that the InventoryMenu object was found
    if (inventoryMenuTransform == null)
    {
        Debug.LogError("InventoryMenu object not found.");
        return;
    }

    // Find the DescritptionSlot (1) object under InventoryMenu
    Transform descriptionSlotTransform = inventoryMenuTransform.Find("DescritptionSlot (1)");

    // Validate that the DescritptionSlot (1) object was found
    if (descriptionSlotTransform == null)
    {
        Debug.LogError("DescritptionSlot (1) object not found under InventoryMenu.");
        return;
    }

    // Find the ItemnameDescription child object under DescritptionSlot (1)
    Transform itemnameDescriptionTransform = descriptionSlotTransform.Find("ItemnameDescription");

    // Validate that the ItemnameDescription object was found
    if (itemnameDescriptionTransform == null)
    {
        Debug.LogError("ItemnameDescription object not found under InventoryMenu.");
        return;
    }

    // Find the ItemName child object under ItemnameDescription
    Transform itemnameTransform = itemnameDescriptionTransform.Find("ItemName");
    if (itemnameTransform == null)
    {
        Debug.LogError("ItemName object not found under InventoryMenu.");
        return;
    }

    // Find the ItemDescription child object under ItemnameDescription
    Transform itemdescriptionTransform = itemnameDescriptionTransform.Find("Itemdescription");
    if (itemdescriptionTransform == null)
    {
        Debug.LogError("ItemDescription object not found under InventoryMenu.");
        return;
    }

    // Get the TextMeshProUGUI component of ItemName and set the new name
    TextMeshProUGUI itemNameText = itemnameTransform.GetComponent<TextMeshProUGUI>();
    if (itemNameText != null)
    {
        itemNameText.text = name; // Modify ItemName with the new name
    }
    else
    {
        Debug.LogError("TextMeshProUGUI component not found on ItemName.");
    }

    // Get the TextMeshProUGUI component of ItemDescription and set the new description
    TextMeshProUGUI itemDescriptionText = itemdescriptionTransform.GetComponent<TextMeshProUGUI>();
    if (itemDescriptionText != null)
    {
        itemDescriptionText.text = description; // Modify ItemDescription with the new description
    }
    else
    {
        Debug.LogError("TextMeshProUGUI component not found on ItemDescription.");
    }
}




void LoadWeaponImages()
{
    // Find the InventoryMenu object first
    Transform inventoryMenuTransform = transform.Find("InventoryMenu");

    // Validate that the InventoryMenu object was found
    if (inventoryMenuTransform == null)
    {
        Debug.LogError("InventoryMenu object not found.");
        return;
    }

    // Find the DescritptionSlot (1) object under InventoryMenu
    Transform descriptionSlotTransform = inventoryMenuTransform.Find("DescritptionSlot (1)");

    // Validate that the DescritptionSlot (1) object was found
    if (descriptionSlotTransform == null)
    {
        Debug.LogError("DescritptionSlot (1) object not found under InventoryMenu.");
        return;
    }

    // Find the ItemSlots child object under DescritptionSlot (1)
    Transform itemSlotsTransform = descriptionSlotTransform.Find("ItemSlots");

    // Validate that the ItemSlots object was found
    if (itemSlotsTransform == null)
    {
        Debug.LogError("ItemSlots object not found under DescritptionSlot (1).");
        return;
    }

    // Find all child objects (slots) under ItemSlots
    Image[] itemSlots = itemSlotsTransform.GetComponentsInChildren<Image>();

    // Define the path to the weapon images folder
    string weaponImagesPath = "Assets/TurnBattleSystem/Textures/Weapons";

    // Iterate through each slot and load the corresponding weapon image
    for (int i = 1; i < itemSlots.Length; i++)
    {
        // Ensure we're only modifying slot images, not the ItemSlots panel itself
        if (itemSlots[i].transform != itemSlotsTransform) 
        {
            string imageFile;

            // Handle the case for slot index 0-9 and 10+
            if (i < 10)
                imageFile = $"Icon28_0{i}.png"; // For indices below 10
            else
                imageFile = $"Icon28_{i}.png"; // For indices 10 and above

            // Load the image only for the slot images
            Image slotImage = itemSlots[i].GetComponent<Image>();
            if (slotImage != null)
            {
                LoadPlayerSpriteGearImage(slotImage, $"{weaponImagesPath}/{imageFile}");
            }

            // Check if the slot already has a Button component, if not add it
            Button slotButton = itemSlots[i].GetComponent<Button>();
            if (slotButton == null)
            {
                // Add a Button component dynamically if it doesn't exist
                slotButton = itemSlots[i].gameObject.AddComponent<Button>();
            }

            // Set up the button to be clickable and pass the image file name to OnSlotClicked
            string currentImageFile = imageFile; // Capture the image file name for closure
            slotButton.onClick.RemoveAllListeners(); // Clear any existing listeners
            slotButton.onClick.AddListener(() => OnSlotClicked(currentImageFile));
        }
    }
}


// Method to handle what happens when an ItemSlot is clicked
void OnSlotClicked(string imageFileName)
{
    // Find the weapon based on the image file name
    Weapon clickedWeapon = Weapon.FindWeaponByIconFile(imageFileName, loadedWeapons);

    if (clickedWeapon != null)
    {
        // Display the weapon's information in the console
        clickedWeapon.DisplayWeaponInfo();

        // Modify the UI description with the weapon's name and description
        ModifyDescription(clickedWeapon.name, clickedWeapon.description);
        ModifyPlayerGear(clickedWeapon,"Weapon");
    }
    else
    {
        Debug.LogError($"Weapon not found for image file: {imageFileName}");
    }
}










    void LoadBodyImages()
    {
        int maxBodies = 4;
        int startIndex = 4; // Starting index for body images (after the first 4 weapon slots)

        // Get all body files from the folder
        string[] bodyFiles = Directory.GetFiles(bodiesFolderPath, "*.png");

        for (int i = 0; i < maxBodies; i++)
        {
            int slotIndex = startIndex + i;
            if (i >= bodyFiles.Length || slotIndex >= ItemSlots.Length)
                break;

            string bodyFile = bodyFiles[i];
            GameObject slotTransform = ItemSlots[slotIndex + 1]; // itemSlots[0] is the parent, so start at index 1
            Image slotImage = slotTransform.GetComponent<Image>();

            if (slotImage != null)
            {
                // Load the texture from the file
                byte[] fileData = File.ReadAllBytes(bodyFile);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);

                // Create a sprite from the texture
                Sprite bodySprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                slotImage.sprite = bodySprite;

            }
        }
    }


    void ModifyWeapon(string weaponName) 
    {
        Debug.Log("Item selected: " + weaponName);

        if (battleHandler != null)
        {
            // Use the ModifyPlayerSpriteSheet method from the BattleHandler to modify the configuration file
            battleHandler.ModifyPlayerWeaponSpritesheet(weaponName);
            Debug.LogError("player spritesheetmodified");
        }
        else
        {
            Debug.LogError("BattleHandler not found");
        }
    }

    void ModifyBody(string bodyName) 
    {
        Debug.Log("Item selected: " + bodyName);

        if (battleHandler != null)
        {
            // Use the ModifyPlayerSpriteSheet method from the BattleHandler to modify the configuration file
            battleHandler.ModifyPlayerWeaponSpritesheet(bodyName);
            Debug.LogError("player spritesheetmodified");
        }
        else
        {
            Debug.LogError("BattleHandler not found");
        }
    }
}


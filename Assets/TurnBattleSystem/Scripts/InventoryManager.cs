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

    public string PlayerSpritePath = "Assets/TurnBattleSystem/Textures/PlayerSprite.png";

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
    private List<Armor> loadedArmor;
    private List<Accessory> loadedAccessory;

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
                    Debug.LogError("path  " + fullPath + "!");
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
    Debug.Log("LoadP;layerSpriteGear");
    battleHandler.playerConfig.DisplayPlayerConfig();
    // Find the InventoryMenu object first
    //Transform inventoryMenuTransform = transform.Find("InventoryMenu");
    GameObject inventoryMenuTransform = battleHandler.inventoryMenu;
    

    if (inventoryMenuTransform != null)
    {
        // Get all children of InventoryMenu
        Transform[] allChildren = inventoryMenuTransform.GetComponentsInChildren<Transform>();

        // Find the PlayerSpriteGear object by name within the children
        foreach (Transform child in allChildren)
        {

//                Debug.Log(child.name);

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
                    LoadPlayerSpriteGearImage(playerSpriteHeadImage,battleHandler.playerConfig.playerHead.iconFile);
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
                    LoadPlayerSpriteGearImage(playerSpriteWeaponHandImage, $"{battleHandler.playerConfig.playerWeapon.path}/{battleHandler.playerConfig.playerWeapon.iconFile}");
                    //    System.Diagnostics.Process.Start("Assets/TurnBattleSystemTextures/PlayerSpritesheet.png");
                    battleHandler.ModifyPlayerWeaponSpritesheet($"{battleHandler.playerConfig.playerWeapon.path}/{battleHandler.playerConfig.playerWeapon.iconFile}",true);
                    
                    
//                                        battleHandler.LoadAndDisplayCharacterSprites();
        //                                Debug.Log("LoadAndDisplayCharacterSpritesr");

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
                    LoadPlayerSpriteGearImage(playerSpriteOffHandImage,battleHandler.playerConfig.playerOffHand.path);
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
                    LoadPlayerSpriteGearImage(playerSpriteRelicImage,battleHandler.playerConfig.playerRelic.path);
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
                    LoadPlayerSpriteGearImage(playerSpriteFeetImage,battleHandler.playerConfig.playerFeet.path);
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
                    LoadPlayerSpriteGearImage(playerSpriteCloakImage,battleHandler.playerConfig.playerCloak.path);
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
                    LoadPlayerSpriteGearImage(playerSpriteBodyImage,battleHandler.playerConfig.playerBody.path);
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
                    LoadPlayerSpriteGearImage(playerSpriteLegsImage,battleHandler.playerConfig.playerLegs.path);
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

public List<string> ArmorFilePaths;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        InventoryMenu.SetActive(false);
        
        battleHandler = FindObjectOfType<BattleHandler>();

        //itemSlots = InventoryMenu.GetComponentsInChildren<Transform>();
        //LoadPlayerSprite();
        LoadPlayerSpriteGear();
        LoadWeaponImages();
        // Load all weapons from the  config file at the start
        loadedWeapons = Weapon.LoadWeaponsFromConfig("Assets/TurnBattleSystem/weapons_config.txt");
        loadedAccessory =  Accessory.LoadAccessoriesFromConfig("Assets/TurnBattleSystem/accessory_config.txt");
        ArmorFilePaths = new List<string>
    {
        "Assets/TurnBattleSystem/armor_config.txt",
        "Assets/TurnBattleSystem/feet_config",
        "Assets/TurnBattleSystem/head_config"
    };
        loadedArmor = Armor.LoadArmorsFromConfigs(ArmorFilePaths);
    
       // LoadBodyImages();
    }

    // Update is called once per frame
    void Update()
    {

        LoadWeaponImages();
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

GearType gear ;

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
    string weaponImagesPath = "Assets/TurnBattleSystem/Textures/bag";

    // Get all the image files from the folder
    string[] imageFiles = Directory.GetFiles(weaponImagesPath, "*.png");

    // Iterate through the slots and assign the images
    for (int i = 0; i < Mathf.Min(itemSlots.Length, imageFiles.Length); i++)
    {
        // Get the current image file
        string imageFile = imageFiles[i];

        // Load the image into a Texture2D
        byte[] fileData = File.ReadAllBytes(imageFile);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Assign the sprite to the slot image
        Image slotImage = itemSlots[i].GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = sprite;
        }

        // Check if the slot already has a Button component, if not add it
        Button slotButton = itemSlots[i].GetComponent<Button>();
        if (slotButton == null)
        {
            // Add a Button component dynamically if it doesn't exist
            slotButton = itemSlots[i].gameObject.AddComponent<Button>();
        }
        
         // Set up the button to be clickable and pass the image file name to OnSlotClicked
        string currentImageFile = imageFile; // Capture the image file path for closure
        slotButton.onClick.RemoveAllListeners(); // Clear any existing listeners
        slotButton.onClick.AddListener(() => OnSlotClicked(imageFile));
    
    }
}


// Method to handle what happens when an ItemSlot is clicked
void OnSlotClicked(string imageFileName)
{
    if (imageFileName == null )
        {
        Debug.LogError("Icon imageFileName NULL");
        return ;
    }
        // Extract the file name from the full path
        string fileName = Path.GetFileName(imageFileName);

        Debug.Log(">>>>>>>>>>>>>>>Icon imageFileName : " + imageFileName + fileName);

    UserObjects objects = new UserObjects();


    GearType gear = objects.FindGearTypeByFilename(fileName);
    Weapon clickedWeapon = new Weapon();
    Armor clickedArmor = new Armor();
    Accessory clickedAccesory = new Accessory();
    

    switch (gear)
    {
        case GearType.Weapon:
                clickedWeapon = Weapon.FindWeaponByIconFile(imageFileName,loadedWeapons);
                clickedWeapon.DisplayWeaponInfo();
                battleHandler.playerConfig.playerWeapon = clickedWeapon;
                battleHandler.playerConfig.SavePlayerConfig("configPlayer.txt");

                ModifyDescription(clickedWeapon.name, clickedWeapon.description);
                battleHandler.ModifyPlayerWeaponSpritesheet($"{battleHandler.playerConfig.playerWeapon.path}/{battleHandler.playerConfig.playerWeapon.iconFile}",true);

            break;

        case GearType.Legs:
            Armor clickedLegArmor = Armor.FindArmorByIconFile(imageFileName, loadedArmor);
            if (clickedLegArmor != null)
            {
                clickedLegArmor.DisplayArmorInfo();
                battleHandler.playerConfig.playerLegs = clickedLegArmor;
                battleHandler.playerConfig.SavePlayerConfig("configPlayer.txt");

                ModifyDescription(clickedLegArmor.name, clickedLegArmor.description);
                battleHandler.ModifyPlayerLegsSpritesheet($"{battleHandler.playerConfig.playerLegs.path}/{battleHandler.playerConfig.playerLegs.iconFile}",true);
            }
            break;

        case GearType.Feet:
            Armor clickedFeetArmor = Armor.FindArmorByIconFile(imageFileName, loadedArmor);
            if (clickedFeetArmor != null)
            {
                clickedFeetArmor.DisplayArmorInfo();
                battleHandler.playerConfig.playerFeet = clickedFeetArmor;
                battleHandler.playerConfig.SavePlayerConfig("configPlayer.txt");

                ModifyDescription(clickedFeetArmor.name, clickedFeetArmor.description);
                battleHandler.ModifyPlayerFeetSpritesheet($"{battleHandler.playerConfig.playerFeet.path}/{battleHandler.playerConfig.playerFeet.iconFile}",true);
            }
            break;

        case GearType.Head:
            Armor clickedHeadArmor = Armor.FindArmorByIconFile(imageFileName, loadedArmor);
            if (clickedHeadArmor != null)
            {
                clickedHeadArmor.DisplayArmorInfo();
                battleHandler.playerConfig.playerHead = clickedHeadArmor;
                battleHandler.playerConfig.SavePlayerConfig("configPlayer.txt");

                ModifyDescription(clickedHeadArmor.name, clickedHeadArmor.description);
                battleHandler.ModifyPlayerFeetSpritesheet($"{battleHandler.playerConfig.playerFeet.path}/{battleHandler.playerConfig.playerFeet.iconFile}",true);
            }
            break;


            case GearType.Armor:
            clickedArmor = Armor.FindArmorByIconFile(imageFileName, loadedArmor);
            if (clickedArmor != null)
            {
                clickedArmor.DisplayArmorInfo();
                battleHandler.playerConfig.playerOffHand = clickedArmor;
                battleHandler.playerConfig.SavePlayerConfig("configPlayer.txt");

                ModifyDescription(clickedArmor.name, clickedArmor.description);
                battleHandler.ModifyPlayerFeetSpritesheet($"{battleHandler.playerConfig.playerFeet.path}/{battleHandler.playerConfig.playerFeet.iconFile}",true);
            }
            break;

        default:
            Debug.LogError($"Item not found for image file: {imageFileName}");
            break;
    }
    LoadPlayerSpriteGear();



    // Add handling for other item types here as needed...
    Debug.LogError($"ImageChanged");
}

}


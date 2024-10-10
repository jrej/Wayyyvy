using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using TMPro;
using Assets.HeroEditor.Common.Scripts.EditorScripts;
using HeroEditor.Common;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
//using UnityEditor.Build.Reporting;
using Assets.HeroEditor.Common.Scripts.CharacterScripts.Firearms;

using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Assets.HeroEditor.InventorySystem.Scripts;
using Assets.HeroEditor.InventorySystem.Scripts.Elements;
using System.Linq;
using Assets.HeroEditor.Common.Scripts.Collections;
using static Assets.HeroEditor.Common.Scripts.EditorScripts.CharacterEditor;

//using Unity.EditorCoroutines.Editor;


public class Dialogue {
    public List<string> lines;

    public Dialogue() {
        lines = new List<string>();
    }

    public Dialogue(List<string> lines) {
        this.lines = lines;
    }
}


[System.Serializable]
public class CharacterPosition
{
     public string characterName;
    public Vector3 initialPosition;
    public Vector3 initialSize; // Store the size of the GameObject
    public Quaternion initialRotation; // Store the rotation of the GameObject

    public CharacterPosition(string name, Vector3 position, Vector3 size, Quaternion rotation)
    {
        characterName = name;
        initialPosition = position;
        initialSize = size;
        initialRotation = rotation;
    }
}


public class BattleHandler : MonoBehaviour {


public int enemyIndexnum;
private Character[] characters;
Character humanCharacter = null;
public Image Lifebar1;

public Image Lifebar2;
public GameObject backg;
public GameObject arena1;
public GameObject arena2;
public GameObject arena3;
public GameObject arena4;

 public List<CollectionBackground> CollectionBackgrounds;


public AudioSource audioSource;
public AudioSource backgroundAudioSource; // New for background music
    public AudioClip backgroundMusicClip;   // Background music clip
    public AudioClip jabClip;
    public AudioClip shootClip;
    public AudioClip runClip;

    public AudioClip buttonClip;

    public AudioClip dodgeClip;
    public AudioClip blockClip;
    public AudioClip slashClip;
      

 
public Button CreateCharacterbutton;

public TextMeshProUGUI textlife1 ;

public TextMeshProUGUI statsText; 
public TextMeshProUGUI enemystatsText; 

public TextMeshProUGUI CombatstatsText; 

     public TextMeshProUGUI textlife2 ;
public Button buttonPrefab;

        public KeyCode ReloadButton;
    private static BattleHandler instance;
    private Dictionary<string, string> config;

    private CharacterCustomizationUI characterUi;

    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    public Texture2D copSpritesheet;
    private BattleManager battleManager;
    public GameObject lifebarCanvas;
    //public GameObject ChooseCharacterMEnu;

    //private CharacterBattle playerCharacterBattle;
   // private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    private State state;
    private Text dialogueText;
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
        public TMP_InputField passwordInput;


private CharacterEditor editor;

    private Dialogue preFightDialogue;

   // public AudioClip musicClip;  // Assign this in the Inspector
  // private AudioSource audioSource;

    private GameObject canvas;
    private GameObject textObj;

    private GameObject playerSpriteObj;
    private GameObject enemySpriteObj;

    GameObject customizationUIObject;

    private GameObject backgroundObj;
string enemyConfigPath;
string playerConfigPath;
 public GameObject ChooseCharacterMenu;

public Button CreateAccountButton;
    private GameObject quitButtonObj;

    public PlayerConfig playerConfig;
        public PlayerConfig enemyConfig;
public  Camera PlayerCamera;

        public Character Character;
        public Character Enemy;

        public Button ShopButton;
// Instantiate HeroEditor character prefab

        public Firearm Firearm;
        public Transform ArmL;
        public Transform ArmR;
        public KeyCode FireButton;
        [Header("Check to disable arm auto rotation.")]
	    public bool FixedArm;
        // Find the Character Panel and the corresponding Button
           public     GameObject dropdowncCharaterPanel;

    public GameObject inventoryMenu;
        public GameObject SelectionMenu;
        public GameObject LoginMenu;

                public GameObject SignInMenu;

            public GameObject startCanvas; // Drag the StartCanvas into this field in the Inspector
            public GameObject shopCanvas;
public ConnectionManager connectionManager;

    public List<CharacterPosition> characterPositions = new List<CharacterPosition>(); // Store character positions

 public InventoryBase playerInventory;
    public Equipment equipment;

    public Button toggleInventoryButton;
    public Button createButton ; 
    public Button StartButton;
         public Button toggleInInventoryButton;
           public Button ConnectionButton;
           
           //public Button FightButton;

        public Button Enemy1;
        public Button Enemy2;
        public Button Enemy3;
        public Button Enemy4;

        public Button Enemy5;

        public Button Enemy6;

        public Button Enemy7;

        public Button Enemy8;

        public Button Enemy9;
    public Character CharacterPrefab;
    public Character EnemyPrefab;
    public Button quitButton;
        //public GameObject buttonCanvas;


        private Image playerSpriteGearImage;
private Image Enemy1Image;
 Animator animator ;

private Image Enemy2Image;
private Image Enemy3Image;

private Image Enemy4Image;
   /// <summary>
    private string PlayerSpritePath = "Assets/TurnBattleSystem/Textures/PlayerSprite.png";
        private string EnemySpritePath = "Assets/TurnBattleSystem/Textures/EnemySprite.png";



    public static BattleHandler GetInstance() {
        return instance;
    }

    private enum State {
        WaitingForPlayer,
        Busy,
    }

        private string[] arenaImages = { "arena1", "arena2", "arena3", "arena4", "arena5" };  // Array of arena image names
 private SpriteRenderer spriteRenderer;


    public void ChangeBackground()
{
    // Randomly select between arena1, arena2, arena3, arena4
    int randomIndex = UnityEngine.Random.Range(1, 5); // 1 to 4 inclusive
    Debug.Log("Randomly selected arena: " + randomIndex);

    // Ensure the map background is hidden before changing
    backg.SetActive(false);

    // Deactivate all arenas first
    arena1.SetActive(false);
    arena2.SetActive(false);
    arena3.SetActive(false);
    arena4.SetActive(false);

    // Activate the randomly selected arena
    switch (randomIndex)
    {
        case 1:
            arena1.SetActive(true);
            break;
        case 2:
            arena2.SetActive(true);
            break;
        case 3:
            arena3.SetActive(true);
            break;
        case 4:
            arena4.SetActive(true);
            break;
    }
}



void AdjustCameraForAspectRatio()
{
    float targetAspect = 16f / 9f;  // Target aspect ratio (adjust based on your design)
    float windowAspect = (float)Screen.width / (float)Screen.height;
    float scaleHeight = windowAspect / targetAspect;

    Camera cam = Camera.main;

    if (scaleHeight < 1.0f)
    {
        // Add letterbox
        Rect rect = cam.rect;
        rect.width = 1.0f;
        rect.height = scaleHeight;
        rect.x = 0;
        rect.y = (1.0f - scaleHeight) / 2.0f;
        cam.rect = rect;
    }
    else
    {
        // Add pillarbox
        float scaleWidth = 1.0f / scaleHeight;
        Rect rect = cam.rect;
        rect.width = scaleWidth;
        rect.height = 1.0f;
        rect.x = (1.0f - scaleWidth) / 2.0f;
        rect.y = 0;
        cam.rect = rect;
    }
}


private IEnumerator AnimateBlock(GameObject character)
{
    float stutterDistance = 20f;
    Vector3 originalPosition = character.transform.position;

    // Move the character backward and forward slightly
    character.transform.position += new Vector3(stutterDistance, 0, 0);
    yield return new WaitForSeconds(1f);
    character.transform.position = originalPosition;
    yield return new WaitForSeconds(1f);

    Debug.Log(character.name + " blocked the attack!");
    
    // Pause to allow the animation to fully display
    yield return new WaitForSeconds(5f);
}

private IEnumerator AnimateDodge(GameObject character)
{
    float dodgeDistance = 20f;
    Vector3 originalPosition = character.transform.position;

    // Slide the character to the side
    character.transform.position += new Vector3(dodgeDistance, 0, 0);
    yield return new WaitForSeconds(1f);

    // Lean character by rotating slightly (optional, adjust angle as needed)
    character.transform.rotation = Quaternion.Euler(0, 0, 10);
    yield return new WaitForSeconds(1f);

    // Return to the original position and reset rotation
    character.transform.position = originalPosition;
    character.transform.rotation = Quaternion.identity;
    yield return new WaitForSeconds(1f);

    Debug.Log(character.name + " dodged the attack!");
    
    // Pause to allow the animation to fully display
    yield return new WaitForSeconds(5f);
}




public void Mapbackground()
{
    // Deactivate all arenas and show only the map background
    arena1.SetActive(false);
    arena2.SetActive(false);
    arena3.SetActive(false);
    arena4.SetActive(false);

    // Assuming backg is the map background
    backg.SetActive(true);
    Debug.Log("Map background activated.");
}

private void PlayAttackSound(AudioClip clip) {
    if (clip != null && audioSource != null) {
        audioSource.clip = clip;
        audioSource.Play();
    }
}


  private void LoadUi()
{
    // Try to find the SelectionCanvas in the scene
    Transform selectionCanvasTransform = GameObject.Find("SelectionCanvas")?.transform;

    if (selectionCanvasTransform == null)
    {
        //Debug.LogError("SelectionCanvas not found.");
        return;
    }

    // Show the SelectionCanvas initially
    selectionCanvasTransform.gameObject.SetActive(true);

    // Get all children of SelectionCanvas
    Transform[] allChildren = selectionCanvasTransform.GetComponentsInChildren<Transform>();

    // Define paths to enemy images
    string enemy1ImagePath = "Assets/TurnBattleSystem/Textures/EnemySprite.png";
    string enemy2ImagePath = "Assets/TurnBattleSystem/Textures/Enemy2Sprite.png";
    string enemy3ImagePath = "Assets/TurnBattleSystem/Textures/EnemySprite.png";
    string enemy4ImagePath = "Assets/TurnBattleSystem/Textures/Enemy2Sprite.png";

    // Iterate through all the children of SelectionCanvas
    foreach (Transform child in allChildren)
    {
        if (child.name == "PlayerSpriteGearSelect")
        {
            playerSpriteGearImage = child.GetComponent<Image>();
            if (playerSpriteGearImage != null)
            {
                LoadPlayerSpriteGearImage(playerSpriteGearImage, PlayerSpritePath);
                //Debug.Log("Player sprite gear loaded successfully.");
            }
            else
            {
                //Debug.LogError("Image component not found on PlayerSpriteGearSelect.");
            }
        }

        else if (child.name == "Enemy1")
        {
            LoadEnemyImage(child, enemy1ImagePath);
        }

        else if (child.name == "Enemy2")
        {
            LoadEnemyImage(child, enemy2ImagePath);
        }

        else if (child.name == "Enemy3")
        {
            LoadEnemyImage(child, enemy3ImagePath);
        }

        else if (child.name == "Enemy4")
        {
            LoadEnemyImage(child, enemy4ImagePath);
        }
    }
}

// Method to update the stats display on the UI
public void UpdateStatsDisplay()
{
    if (statsText != null)
    {
        statsText.text = "Name: " + battleManager.playerStats.Name + "\n" +
                        "Level: " + battleManager.playerStats.Level + "\n" +
                        "HP: " + battleManager.playerStats.HP + "\n" +
                        "Attack: " + battleManager.playerStats.Attack + "\n" +
                        "Defense: " + battleManager.playerStats.Defense + "\n" +
                        "Special Attack: " + battleManager.playerStats.SpecialAttack + "\n" +
                        "Special Defense: " + battleManager.playerStats.SpecialDefense + "\n" +
                        "Speed: " + battleManager.playerStats.Speed + "\n" +
                        "Luck: " + battleManager.playerStats.Luck + "%\n" +
                        "Experience: " + battleManager.playerStats.Experience + " / " + battleManager.playerStats.ExperienceToNextLevel;
    }
    if (enemystatsText!= null)
    {
        enemystatsText.text = "Name: " + battleManager.enemyStats.Name + "\n" +
                        "Level: " + battleManager.enemyStats.Level + "\n" +
                        "HP: " + battleManager.enemyStats.HP + "\n" +
                        "Attack: " + battleManager.enemyStats.Attack + "\n" +
                        "Defense: " + battleManager.enemyStats.Defense + "\n" +
                        "Special Attack: " + battleManager.enemyStats.SpecialAttack + "\n" +
                        "Special Defense: " + battleManager.enemyStats.SpecialDefense + "\n" +
                        "Speed: " + battleManager.enemyStats.Speed + "\n" +
                        "Luck: " + battleManager.enemyStats.Luck + "%\n" +
                        "Experience: " + battleManager.enemyStats.Experience + " / " + battleManager.enemyStats.ExperienceToNextLevel;
    }
}



// Helper function to load and assign enemy image
private void LoadEnemyImage(Transform enemyTransform, string imagePath)
{
    // Load the image into a Texture2D
    byte[] fileData = File.ReadAllBytes(imagePath);
    Texture2D texture = new Texture2D(2, 2);
    texture.LoadImage(fileData);

    // Create a sprite from the texture
    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

    // Assign the sprite to the enemy's image component
    Image enemyImage = enemyTransform.GetComponent<Image>();
    if (enemyImage != null)
    {
        enemyImage.sprite = sprite;
        //Debug.Log($"Loaded enemy image from {imagePath}");
    }
    else
    {
        //Debug.LogError("Image component not found on " + enemyTransform.name);
    }
}


    private InventoryBase inventoryBase;

private void Awake()
{
    // Existing Awake logic
    instance = this;
    canvas = GameObject.Find("Canvas2");
    characters = FindObjectsOfType<Character>();

    // Initialize InventoryBase to ensure ItemCollection.Active is properly set up
    inventoryBase = FindObjectOfType<InventoryBase>();
    if (inventoryBase == null)
    {
        Debug.LogError("InventoryBase not found in the scene. Please ensure it is present.");
        return;
    }

    inventoryBase.Awake(); // This will initialize ItemCollection.Active

    // Continue with existing Awake logic
    foreach (Character character in characters)
    {
        string characterName = character.gameObject.name;
        Vector3 initialPosition = character.transform.position;
        Vector3 initialSize = character.transform.localScale;
        Quaternion initialRotation = character.transform.rotation;

        characterPositions.Add(new CharacterPosition(characterName, initialPosition, initialSize, initialRotation));
    }

    inventoryMenu.SetActive(false);
    SelectionMenu.SetActive(false);
    HideCharacters();
    state = State.WaitingForPlayer;
}






// Function to reset all characters to their initial positions
   public void ResetToInitialPositions()
{
            lifebarCanvas.SetActive(false);

    foreach (Character character in characters)
    {
        // Set expression and state back to default/idle
        character.SetExpression("Default"); // Reset to default or ready expression
        character.SetState(CharacterState.Relax); // Ensure the state is Idle after resetting

        // Reset animation manually
        Animator animator = character.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("IdleAnimation", -1, 0f); // Force the character back to Idle animation
        }

        // Reset position, scale, and importantly the rotation
        foreach (CharacterPosition cp in characterPositions)
        {   
            if (character.gameObject.name == "Human")
            {
                character.transform.position = new Vector3(-73.9036255f,-9.31552124f,0);// cp.initialPosition;
                character.transform.localScale = cp.initialSize;
                character.transform.rotation = new Quaternion(0, 0, 0, 1); // Reset to default upright position
            }
            else if (character.gameObject.name == cp.characterName)
            {
                character.transform.position = cp.initialPosition;
                character.transform.localScale = cp.initialSize;
                character.transform.rotation = cp.initialRotation; // Reset rotation as well
            }
            
            Debug.Log($"Character {cp.characterName} reset to initial position: {cp.initialPosition}, rotation: {cp.initialRotation}");
        }

        // Ensure character is visible and active
        character.gameObject.SetActive(true);
    }

    Debug.Log("All characters have been reset to their initial positions, sizes, rotations, and animations.");
}






// Method to activate all children of a given Transform
    void ActivateAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(true);
            // Recursively activate children of child elements if needed
            if (child.childCount > 0)
            {
                ActivateAllChildren(child);
            }
        }
    }


private void LoadEnemyAndStartBattle(int enemyIndex) {
   // editor = new CharacterEditor();
           PlayAttackSound(buttonClip);

   ChangeBackground();
    SelectionMenu.SetActive(false);
    
    //buttonCanvas.SetActive(true);
   // buttonCanvas.SetActive(false);
    enemyIndexnum = enemyIndex;
    enemyConfigPath = $"configEnemy{enemyIndex}.json";
    playerConfigPath = "configPlayer.json" ;
   //         SetupPlayer("configPlayer.json");
  //  editor.LoadFromJson("configPlayer.json");
  //  editor.LoadFromJson(enemyConfigPath);
    //SetupEnemy(enemyConfigPath); // Load the selected enemy config file
    StartNewGame(); // Start the battle with the selected enemy
    SelectionMenu.SetActive(false);
}


private void StartNewGame() {
    // Launch coroutines to spawn player and enemy characters concurrently
    state = State.WaitingForPlayer;
    StartCoroutine(SpawnCharacterCoroutine());  // Spawn player character
   
}

// Coroutine to spawn characters asynchronously
private IEnumerator SpawnCharacterCoroutine() {
    // Simulate a delay for loading, customization, or other setup if necessary
    yield return new WaitForSeconds(0.5f); // Adjust or remove the delay as needed

    // Call your SpawnCharacter method
    SpawnCharacter();
   // FightButton.gameObject.SetActive(true);

    

    //Debug.Log(  "Player character spawned.Enemy character spawned.");
}

// Updated SpawnCharacter method
private void SpawnCharacter() {
    // Find all objects of type 'Character' in the scene
// Loop through and find the one named 'Human'
        
         // Loop through and find the one named 'Human'
        Character EnemyCharacter = null;

        string enemyname = $"character{enemyIndexnum}";
        //Debug.Log("Looking for : : " + enemyname);

        foreach (Character character in characters)
        {
            if (character.gameObject.name == "Human")
            {
                 humanCharacter = character;
                 //connectionManager.characterEditor.Character =  humanCharacter ;

                
            }
            else if (character.gameObject.name == enemyname)
            {   
                EnemyCharacter = character;
                
            }
            else{
                character.gameObject.SetActive(false);
            }
            
        }
  
   
        Character = humanCharacter;  // Ensure CharacterPrefab is assigned in the Inspector
        Enemy = EnemyCharacter;  // Ensure EnemyPrefab is assigned in the Inspector
    
      // Character = Instantiate((Character)); 
        // Set character position and scale
        Character.transform.position = new Vector3(-50, -20);
        Character.transform.localScale = new Vector3(5f, 5f, 5f);
        //Character.transform.rotation = Quaternion.identity; // Or any rotation you'd like to apply
        Character.gameObject.SetActive(true);

        //Debug.Log("Setting up player character with sprite: " + playerConfigPath);


        // Set enemy position, rotation, and scale
        Enemy.transform.position = new Vector3(+50, -20);
        Enemy.transform.Rotate(0, 180, 0);
        Enemy.transform.localScale = new Vector3(5f, 5f, 5f);
        Character.gameObject.SetActive(true);
        battleManager = new BattleManager(Character,Enemy);
        battleManager.ExtractCharacterItems();
        lifebarCanvas.SetActive(true);
        Lifebar1.fillAmount = 1; 
        Lifebar2.fillAmount = 1; 

        battleManager.playerStats.HP = battleManager.playerMaxHP;
                battleManager.enemyStats.HP = battleManager.enemyMaxHP;
                FaceEachOther(Character.gameObject, Enemy.gameObject);
        //FightButton.SetActive(true);
        

      
    
}




public void UpdateConfigWithSelectedWeapon(string weaponPath) {
    // Example implementation for updating config
    // This should be replaced with your actual config update logic
    //Debug.Log("Updating config with weapon: " + weaponPath);
    // Example code to update the config file
     config["PlayerWeapon"] = weaponPath;
     ModifyPlayerBodySpritesheet(weaponPath,true);
     
}


public void LoadAndDisplayCharacterSprites() {
    // Load player sprite
    Texture2D playerTexture = LoadTextureFromFile(playerConfig.playerHead.path);

    // Check if the player sprite object already exists
    GameObject playerSpriteObj = GameObject.Find("PlayerSprite");
    if (playerSpriteObj == null) {
        playerSpriteObj = new GameObject("PlayerSprite");
        playerSpriteObj.transform.SetParent(canvas.transform);

        // Add Image component and set sprite
        Image playerImage = playerSpriteObj.AddComponent<Image>();
        playerImage.sprite = Sprite.Create(playerTexture, new Rect(0, 0, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f));

        // Adjust position and size
        RectTransform playerRect = playerSpriteObj.GetComponent<RectTransform>();
        playerRect.sizeDelta = new Vector2(50, 50); // Adjust size as necessary
        playerRect.anchoredPosition = new Vector2(-110, -10); // Adjust position as necessary
        playerRect.anchorMin = new Vector2(0.5f, 0.5f);
        playerRect.anchorMax = new Vector2(0.5f, 0.5f);
        playerRect.pivot = new Vector2(0.5f, 0.5f);
    } else {
        // Optionally update the existing object, e.g., changing the sprite
        Image playerImage = playerSpriteObj.GetComponent<Image>();
        if (playerImage != null) {
            playerImage.sprite = Sprite.Create(playerTexture, new Rect(0, 0, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    // Load enemy sprite
    Texture2D enemyTexture = LoadTextureFromFile(playerConfig.playerHead.path);

    // Check if the enemy sprite object already exists
    GameObject enemySpriteObj = GameObject.Find("EnemySprite");
    if (enemySpriteObj == null) {
        enemySpriteObj = new GameObject("EnemySprite");
        enemySpriteObj.transform.SetParent(canvas.transform);

        // Add Image component and set sprite
        Image enemyImage = enemySpriteObj.AddComponent<Image>();
        enemyImage.sprite = Sprite.Create(enemyTexture, new Rect(0, 0, enemyTexture.width, enemyTexture.height), new Vector2(0.5f, 0.5f));

        // Adjust position and size
        RectTransform enemyRect = enemySpriteObj.GetComponent<RectTransform>();
        enemyRect.sizeDelta = new Vector2(50, 50); // Adjust size as necessary
        enemyRect.anchoredPosition = new Vector2(110, -10); // Adjust position as necessary
        enemyRect.anchorMin = new Vector2(0.5f, 0.5f);
        enemyRect.anchorMax = new Vector2(0.5f, 0.5f);
        enemyRect.pivot = new Vector2(0.5f, 0.5f);
    } else {
        // Optionally update the existing object, e.g., changing the sprite
        Image enemyImage = enemySpriteObj.GetComponent<Image>();
        if (enemyImage != null) {
            enemyImage.sprite = Sprite.Create(enemyTexture, new Rect(0, 0, enemyTexture.width, enemyTexture.height), new Vector2(0.5f, 0.5f));
        }
    }
}


   /// </summary>



    private void Start() {
        
        // Get the SpriteRenderer component from the backg GameObject
        spriteRenderer = backg.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the backg GameObject.");
            return;
        }
        StartButton.onClick.AddListener(StartGame);
        ShopButton.onClick.AddListener(ShopDisplay);
                CreateAccountButton.onClick.AddListener(Login);
                CreateCharacterbutton.onClick.AddListener(createCaracter);

        createButton.onClick.AddListener(SignIn);
        ConnectionButton.onClick.AddListener(Connect);
       // FightButton.onClick.AddListener(Battle);

        

       // toggleInInventoryButton.onClick.AddListener(ToggleInventoryMenu);
        toggleInventoryButton.onClick.AddListener(ToggleInventoryMenu);
        quitButton.onClick.AddListener(QuitGame);
        // Add listeners for enemy buttons using lambda expressions
//        Enemy1.onClick.AddListener(() => LoadEnemyAndStartBattle(1));
        Enemy2.onClick.AddListener(() => LoadEnemyAndStartBattle(2));
        Enemy3.onClick.AddListener(() => LoadEnemyAndStartBattle(3));
        Enemy4.onClick.AddListener(() => LoadEnemyAndStartBattle(4));
        Enemy5.onClick.AddListener(() => LoadEnemyAndStartBattle(5));
        Enemy6.onClick.AddListener(() => LoadEnemyAndStartBattle(6));
        Enemy7.onClick.AddListener(() => LoadEnemyAndStartBattle(7));
        Enemy8.onClick.AddListener(() => LoadEnemyAndStartBattle(8));
        Enemy9.onClick.AddListener(() => LoadEnemyAndStartBattle(9));

        backgroundAudioSource.clip = backgroundMusicClip;
    backgroundAudioSource.loop = true;     // Loop the background music
    //backgroundAudioSource.volume = 0.5f;   // Set the volume (adjust as needed)
    backgroundAudioSource.Play();

    Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false;         // Disable portrait
        Screen.autorotateToPortraitUpsideDown = false; // Disable upside-down portrait
        Screen.autorotateToLandscapeLeft = true;     // Enable landscape left
        Screen.autorotateToLandscapeRight = true;

        shopCanvas.SetActive(false);

            //();
      

    }
        public void LoadPlayerSpriteGearImage(Image img, string imagePath)
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
            //Debug.LogError($"Image file not found at path: {imagePath}");
        }
    }

public void UpdateCharacterInDatabase()
{
    List<string> characterFilePathList = connectionManager.PlayerDatadata.characterJsonPaths;

    bool characterFound = false;

    foreach (var characterPath in characterFilePathList)
    {
        string characterName = Path.GetFileNameWithoutExtension(characterPath);
        if (connectionManager.characterEditor.Character.name == characterName)
        {
            //connectionManager.UpdatePlayerCharacter(connectionManager.emailInput, humanCharacter, characterName);
            Debug.Log("Character " + characterName + " has been updated in the database.");
            characterFound = true;
            break;
        }
    }

    if (!characterFound)
    {
        string newCharacterName = connectionManager.characterEditor.Character.name;
        string directoryPath = Path.Combine("Assets", "Account", connectionManager.emailInput, "Characters");
        string characterFilePath = Path.Combine(directoryPath, newCharacterName + ".json");

        connectionManager.PlayerDatadata.characterJsonPaths.Add(characterFilePath);
        connectionManager.UpdatePlayerCharacter(connectionManager.emailInput, humanCharacter, newCharacterName);

        Debug.Log("New character " + newCharacterName + " added and saved to the database.");
    }

    connectionManager.SavePlayerDatabase();
}

    private void EquipHumanWithRandomEquipment()
{
    if (humanCharacter == null)
    {
        Debug.LogError("Human character not found!");
        return;
    }

    if (ItemCollection.Active == null || ItemCollection.Active.Items == null)
    {
        Debug.LogError("ItemCollection.Active or Items is not initialized properly.");
        return;
    }

    // Load available items from inventory and convert ItemParams to Items
    List<Item> availableItems = ItemCollection.Active.Items
        .Select(itemParams => new Item(itemParams.Id))
        .ToList();

    // Randomly pick a few items to equip the character
    List<Item> randomItems = PickRandomItems(availableItems, 3);

    foreach (Item item in randomItems)
    {
        humanCharacter.Equip(item);
    }

    Debug.Log("Human character equipped with random items.");

    // Update character in the database
    UpdateCharacterInDatabase();
}


private List<Item> PickRandomItems(List<Item> availableItems, int count)
{
    List<Item> selectedItems = new List<Item>();

    for (int i = 0; i < count; i++)
    {
        int randomIndex = Random.Range(0, availableItems.Count);
        selectedItems.Add(availableItems[randomIndex]);
    }

    return selectedItems;
}


     // Method to toggle the inventory menu
    private void StartGame() {
        //Debug.Log("Starting Game ");
        PlayAttackSound(buttonClip);
        startCanvas.SetActive(false);
        LoginMenu.SetActive(true);
        
        
    }

    private void ShopDisplay(){
                PlayAttackSound(buttonClip);

        shopCanvas.SetActive(true);
        inventoryMenu.SetActive(false);

    }
    public void Connect()
{   
            PlayAttackSound(buttonClip);

    connectionManager = new ConnectionManager();
connectionManager.Initialize();
    RetrieveCharacter();
    connectionManager.emailInput = emailInput.text;
    connectionManager.passwordInput = passwordInput.text;
   // EquipHumanWithRandomEquipment();

    // Use a coroutine to load characters to avoid UI freezing
    StartCoroutine(LoadCharactersCoroutine());
}

private IEnumerator LoadCharactersCoroutine()
{
    connectionManager.Connect(Character);
    Debug.Log("Connected");
    Mapbackground();

    List<string> characterFilePathList = connectionManager.PlayerDatadata.characterJsonPaths;
    
    if (characterFilePathList.Count > 0)
    {
        foreach (string characterPath in characterFilePathList)
        {
            Debug.Log("Creating character panel for: " + characterPath);
            CreateCharacterPanel(characterPath);

            // Optionally yield to allow UI updates between character loads
            yield return null;
        }

      //  ChooseCharacterMenu.SetActive(true);
    }
    else
    {
      //  ChooseCharacterMenu.SetActive(true);  // Show empty state if no characters exist
    }

    Debug.Log("Character loading complete.");
    dropdowncCharaterPanel.SetActive(false);
        //ChooseCharacterMenu.SetActive(false);
        LoginMenu.SetActive(false);

        SelectionMenu.SetActive(true);
        DisplayCharactersOnScreen();  // Display character on screen

}


private void CreateCharacterPanel(string characterPath) {
    if (File.Exists(characterPath)) {
        string characterName = Path.GetFileNameWithoutExtension(characterPath);

        // Instantiate the button and set it inside the panel
        Button newButton = Instantiate(buttonPrefab);
        newButton.transform.SetParent(dropdowncCharaterPanel.transform, false);

        // Update the TextMeshProUGUI instead of standard Text component
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = characterName;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in button prefab!");
        }

        // Set button click listener
        //newButton.onClick.AddListener(() => SelectCharacter(characterPath));
        Debug.Log($"AAAAAAAAAAAAAAAAAAButton clicked for character: {characterPath}");
        newButton.onClick.AddListener(() =>
{
    Debug.Log($"Button clicked for character: {characterPath}");
    SelectCharacter(characterPath);
});
        Debug.Log($"BBBBBBBBbuton clicked for character: {characterPath}");


        // Configure the RawImage component
       // GameObject rawImageObject = new GameObject("CharacterImage");
      //  RawImage characterImage = rawImageObject.AddComponent<RawImage>();
       // characterImage.transform.SetParent(newPanel.transform, false);

        // Use placeholder texture or actual character image
       ////// Texture2D placeholderTexture = Resources.Load<Texture2D>("Textures/Placeholder");
      //  characterImage.texture = placeholderTexture;

      //  RectTransform imageRect = characterImage.GetComponent<RectTransform>();
      //  imageRect.sizeDelta = new Vector2(400, 300);
    } else {
        Debug.LogError("Character file not found: " + characterPath);
    }
}




    
public void createCaracter() {
            PlayAttackSound(buttonClip);

    string text = nameInput.text ;
    connectionManager.CreateCharacter( text,characters);
    Connect();
}

private void DebugTiming(string message, System.Action action)
{
    var start = Time.realtimeSinceStartup;
    action();
    var end = Time.realtimeSinceStartup;
    Debug.Log($"{message} took {end - start} seconds");
}
    // A method to select the character (you can define what happens when a character is selected)
    public void SelectCharacter(string selectedCharacterPath)
{
    Debug.Log("Selected character path OOOOOOOOOOOOOOOO: " + selectedCharacterPath);

    if (File.Exists(selectedCharacterPath))
    {
        
        string jsonContent = File.ReadAllText(selectedCharacterPath);
        Debug.Log("Character JSON loaded: " + jsonContent);

        connectionManager.characterEditor.LoadFromJson(selectedCharacterPath); // Load character from JSON
        Character = (Character)connectionManager.characterEditor.Character;

        dropdowncCharaterPanel.SetActive(false);
        //ChooseCharacterMenu.SetActive(false);

        SelectionMenu.SetActive(true);
        DisplayCharactersOnScreen();  // Display character on screen
    }
    else
    {
        Debug.LogError("Character JSON file not found: " + selectedCharacterPath);
    }
}


private void FaceEachOther(GameObject character, GameObject enemy)
    {
        // Check the positions of the character and enemy
        if (character.transform.position.x < enemy.transform.position.x)
        {
            // Character is on the left, facing right
            character.transform.localScale = new Vector3(Mathf.Abs(character.transform.localScale.x), character.transform.localScale.y, character.transform.localScale.z);
            
            // Enemy is on the right, facing left
            enemy.transform.localScale = new Vector3(-Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
        }
        else
        {
            // Character is on the right, facing left
            character.transform.localScale = new Vector3(-Mathf.Abs(character.transform.localScale.x), character.transform.localScale.y, character.transform.localScale.z);

            // Enemy is on the left, facing right
            enemy.transform.localScale = new Vector3(Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
        }

        Debug.Log("Character and enemy are now facing each other.");
    }

    private void Battle() {
        Debug.Log("Fight button pressed: state : " + state);
        
    if (state != State.WaitingForPlayer) {
        return; // Exit early if the game is busy (e.g., moving or fighting)
    }

    FaceEachOther(Character.gameObject, Enemy.gameObject);

    if (battleManager.playerStats.HP > 0 && battleManager.enemyStats.HP > 0) {
                SelectionMenu.SetActive(false);

        StartCoroutine(HandleCombat());
    } else {

        HandleCombatEnd();

    }
    UpdateStatsDisplay();
}

private void HandleCombatEnd()
{
    StartCoroutine(HandleCombatEndWithDelay());
    
    
}

private IEnumerator HandleCombatEndWithDelay()
{
    state = State.Busy;

    if (battleManager.playerStats.HP <= 0)
    {
        // Player died
        Character.SetState(CharacterState.DeathB);
        Character.SetExpression("Dead");
    }
    else
    {
        // Enemy died
        Enemy.SetState(CharacterState.DeathB);
        Enemy.SetExpression("Dead");

        // Grant experience for winning
        GrantExperienceToWinner();
    }
     // Wait for 2 seconds (adjust the delay as needed)
    yield return new WaitForSeconds(2f);
            Character.SetState(CharacterState.Idle);
            Enemy.SetState(CharacterState.Idle);
    yield return new WaitForSeconds(2f);



       // Reset characters and UI after the delay
    //ResetCharactersToDefaultState();

    


    SelectionMenu.SetActive(true);
    ResetToInitialPositions();
    HideCharacters();
    DisplayCharactersOnScreen();
    Mapbackground();
    state = State.Busy;
    //StartCoroutine(ResetCharactersToDefaultStateCoroutine());

}

private IEnumerator ResetCharactersToDefaultStateCoroutine()
{
    Debug.Log("ResetCharactersToDefaultStateCoroutine ");
    // Reset Player
    Character.ResetAnimation();
    // Wait for one frame to ensure that changes take effect
    yield return null;
    Character.SetState(CharacterState.Relax);
    // Wait for one frame to ensure that changes take effect
    yield return null;
    Character.SetExpression("Default");
    Character.GetReady();
    Character.UpdateAnimation();


    // Wait for one frame to ensure that changes take effect
    yield return null;

    // Reset the rotation to identity (no rotation)
    //Character.transform.rotation = Quaternion.identity;

  
    // Reset Enemy
    Enemy.ResetAnimation();
    Enemy.GetReady();
    // Wait for one frame to ensure that changes take effect
    yield return null;
    Enemy.SetState(CharacterState.Relax);
    // Wait for one frame to ensure that changes take effect
    yield return null;
    Enemy.SetExpression("Default");
    Enemy.UpdateAnimation();

    // Wait for one frame to ensure that changes take effect
    yield return null;

    // Reset the rotation to identity (no rotation)
   // Enemy.transform.rotation = Quaternion.identity;

    // Wait for another frame to ensure all changes are applied
    yield return null;
}


private void ResetCharactersToDefaultState() {
    Debug.Log("Reset character to Default state");
    // Reset Player
    Character.ResetAnimation();
    Character.SetState(CharacterState.Relax);
    Character.SetExpression("Default");
    Character.transform.rotation = Quaternion.identity;  // Reset rotation to default (no rotation)
    Character.transform.localScale = new Vector3(+Mathf.Abs(Enemy.transform.localScale.x), Enemy.transform.localScale.y, Enemy.transform.localScale.z);


    // Reset Enemy
    Enemy.ResetAnimation();
    Enemy.SetState(CharacterState.Relax);
    Enemy.SetExpression("Default");
    Enemy.transform.rotation = Quaternion.identity;  // Reset rotation to default (no rotation)
    Enemy.transform.localScale = new Vector3(-Mathf.Abs(Enemy.transform.localScale.x), Enemy.transform.localScale.y, Enemy.transform.localScale.z);

}


      private void SignIn() {
                PlayAttackSound(buttonClip);

        Debug.Log("SignIn  ");
        SignInMenu.SetActive(true);
        LoginMenu.SetActive(false);
      //  buttonCanvas.SetActive(true);
        DisplayCharactersOnScreen();
    }

      private void Login() {
                PlayAttackSound(buttonClip);

        Debug.Log("Login ");
        SignInMenu.SetActive(false);
        LoginMenu.SetActive(true);
        
    }

 

private void SimulateCharacterRetrievalTest()
{
    Debug.Log("=== Simulating Character Retrieval Test ===");
    
    // Re-load the database to simulate restarting the application
    connectionManager.LoadPlayerDatabase();
    
    // Check if the newly added character can be retrieved
    bool characterFoundAgain = false;

    foreach (var characterPath in connectionManager.PlayerDatadata.characterJsonPaths)
    {
        string characterName = Path.GetFileNameWithoutExtension(characterPath);

        if (connectionManager.characterEditor.Character.name == characterName)
        {
            Debug.Log("Character successfully retrieved from database: " + characterName);
            characterFoundAgain = true;
            break;
        }
    }

    if (!characterFoundAgain)
    {
        Debug.LogError("Character was not found when re-loading from the database.");
    }
    else
    {
        Debug.Log("Character retrieval test passed.");
    }
}






    // Method to toggle the inventory menu
    // Toggle inventory menu on/off
    public void ToggleInventoryMenu()
    {
                PlayAttackSound(buttonClip);

        bool isActive = inventoryMenu.activeSelf;
        inventoryMenu.SetActive(!isActive);
        

        if (isActive)
        {
            // Closing inventory, save character to database
            SaveCharacter();
            shopCanvas.SetActive(false);
        }
        else
        {
            // Opening inventory, retrieve character from database
            RetrieveCharacter();
        }
    }

    // Save the character's current state using the connection manager
    private void SaveCharacter()
    {
        Debug.Log("Saving character...");
        connectionManager.UpdatePlayerCharacter(connectionManager.emailInput, Character, Character.name);
    }

    // Retrieve the character's saved state from the database
    private void RetrieveCharacter()
    {
        Debug.Log("Retrieving character...");
        string characterPath = connectionManager.GetCharacterPath(connectionManager.emailInput, Character.name);
        if (!string.IsNullOrEmpty(characterPath))
        {
            connectionManager.characterEditor.LoadFromJson(characterPath);
            Debug.Log($"Character {Character.name} retrieved from path: {characterPath}");
        }
        else
        {
            Debug.LogError("Failed to retrieve character path from database.");
        }
    }
private void HideCharacters(){
    Debug.Log("Hidin characters");
    foreach (Character character in characters)
        {
            character.gameObject.SetActive(false);
        }
}
private void DisplayCharactersOnScreen(){
        Debug.Log("display characters");

    foreach (Character character in characters)
        {
            character.gameObject.SetActive(true);
            ResetCharactersToDefaultState();
        }
}
   private void QuitGame() {
            PlayAttackSound(buttonClip);

    Debug.Log("Quitting game...");
    
    
    string currentSceneName = SceneManager.GetActiveScene().name;
SceneManager.LoadScene(currentSceneName);
    

}




private bool isMoving = false;
private Vector3 originalPosition;
private Vector3 targetPosition;
private float moveSpeed = 200f; // Speed of movement
private float waitTime = 0.4f; // Time to wait between actions


private IEnumerator MoveToEnemyAndBack() {
   
        
       
    //    state = State.Busy;
        if(battleManager.playerStats.HP <= 0 ){
            yield return null;
        }
        else{
    // state = State.Busy;
        isMoving = true;
      //  Character.SetState(CharacterState.Run);
        Character.SetExpression("Default");
        originalPosition = Character.transform.position; // Save original position
        targetPosition = Enemy.transform.position - new Vector3(+10f, 0f, 0f); // Target position near the enemy
        
          // Check for failed attack
    if (Random.Range(0f, 100f) < battleManager.playerStats.Luck)
    {
        Debug.Log("Player attack failed!");
        CombatstatsText.text = "Player attack failed!";

        yield return StartCoroutine(RotateAndCrouch(Character.gameObject));
        Character.transform.position = new Vector3(-50, -20);
    //Character.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Rotate to lie flat

   // Character.transform.position += new Vector3(0, -0.5f, 0); // Adjust position slightly downward
       // yield return new WaitForSeconds(2);
        yield break;  // Attack fails, no damage dealt
    }

        // Perform attacks after reaching the enemy
        Character.SetState(CharacterState.Run);

        
        

        
        // Move toward the enemy
        yield return StartCoroutine(MoveCharacter(Character.transform, targetPosition, moveSpeed));

        
        // Perform attack sequence
        yield return StartCoroutine(EnemyBlock());
        yield return new WaitForSeconds(waitTime);
        Character.Jab();
        
        PlayAttackSound(jabClip);
        

        
        yield return StartCoroutine(EnemyBlock());
        yield return new WaitForSeconds(waitTime);
        Character.Shoot();
        PlayAttackSound(jabClip);
        
        yield return StartCoroutine(EnemyBlock());
        yield return new WaitForSeconds(waitTime);
        Character.Slash();
        PlayAttackSound(jabClip);
        



        // Move back to original position
        yield return StartCoroutine(MoveCharacter(Character.transform, originalPosition, moveSpeed));
        



        isMoving = false;
        //state = State.WaitingForPlayer; // Set back to waiting after moving back
                    Character.SetState(CharacterState.Relax);
                    // Handle attack logic
    // StartCoroutine(HandlePlayerAttack(Enemy));
    
   

   }

}


private IEnumerator EnemyBlock(){
    float ran = Random.Range(0f, 100f);
    Debug.Log("Enemy Random value " + ran );
    if(battleManager.enemyStats.Defense > battleManager.enemyStats.Speed){
    // Check if the player blocks or dodges
        if (Random.Range(0f, 100f) < battleManager.playerStats.Defense /2 + battleManager.playerStats.Luck)
        {
            Debug.Log("Enemy blocks the attack!");
            CombatstatsText.text = "Enemy hit block the attack";
            
            yield return StartCoroutine(Block(Enemy.gameObject));
                        yield return new WaitForSeconds(0.2f);
                                Enemy.transform.position = new Vector3(50, -20);


         yield break;  // Blocked, no damage dealt
        }
    }
    else{
        if (Random.Range(0f, 100f) < battleManager.playerStats.Speed/2 + battleManager.playerStats.Luck)
        {   
            Enemy.SetState(CharacterState.Jump);
            Debug.Log("Enemy dodges the attack!");
            CombatstatsText.text = "Enemy dodges the attack!";
            yield return StartCoroutine(DodgeJump(Enemy.gameObject));
                        yield return new WaitForSeconds(0.2f);
                                Enemy.transform.position = new Vector3(50, -20);


         yield break;  // Dodged, no damage dealt
        }
    }
            float damage =  battleManager.PerformAttack(battleManager.playerStats,battleManager.enemyStats);
            
            CombatstatsText.text = "Player hit " + (int)damage ;
            yield return new WaitForSeconds(0.2f);

}

private IEnumerator playerBlock(){

    float ran = Random.Range(0f, 100f);
    Debug.Log("Player Random value " + ran );

    if (battleManager.playerStats.Defense > battleManager.playerStats.Speed){

        // Check if the player blocks or dodges
        if (ran < battleManager.playerStats.Defense/2 + battleManager.playerStats.Luck)
        {
            Debug.Log("Player blocks the attack!");
            CombatstatsText.text = "Player block the attack!";
            yield return StartCoroutine(Block(Character.gameObject));
            yield return new WaitForSeconds(0.2f);
                    Character.transform.position = new Vector3(-50, -20);
                    
                    




            yield break;  // Blocked, no damage dealt
        }
    }
    else {

        if (ran< battleManager.playerStats.Speed/2 + battleManager.playerStats.Luck)
        {
            Character.SetState(CharacterState.Jump);
            Debug.Log("Player dodges the attack!");
            CombatstatsText.text = "Player dodges the attack!";
            yield return StartCoroutine(DodgeJump(Character.gameObject));
            yield return new WaitForSeconds(0.2f);
            Character.transform.position = new Vector3(-50, -20);


            yield break;  // Dodged, no damage dealt
        }
    }
                
                float damage =  battleManager.PerformAttack(battleManager.enemyStats,battleManager.playerStats);
            
            CombatstatsText.text = "Enemy hit " + (int)damage ;
            yield return new WaitForSeconds(0.2f);


}
private IEnumerator MoveToPlayerAndBack() {

    
        if(battleManager.enemyStats.HP <= 0 ){
            yield return null;
        }
        else{
        
        isMoving = true;
        Enemy.SetState(CharacterState.Run);
        originalPosition = Enemy.transform.position; // Save original position
        targetPosition = Character.transform.position - new Vector3(-10f, 0f, 0f); // Target position near the enemy

      // Check for failed attack
    if (Random.Range(0f, 100f) < battleManager.enemyStats.Luck)
    {
        Debug.Log("Enemy attack failed!");
         CombatstatsText.text = "Enemy attack failed!";
        // Rotate and crouch sequence
        yield return StartCoroutine(RotateAndCrouch(Enemy.gameObject));
                Enemy.transform.position = new Vector3(+50, -20);

       
            
       // yield return new WaitForSeconds(2);
        yield break;  // Attack fails, no damage dealt
    }

    // Perform attacks after reaching the enemy
        Enemy.SetState(CharacterState.Run);
    
        
        // Move toward the enemy
        yield return StartCoroutine(MoveCharacter(Enemy.transform, targetPosition, moveSpeed));

        yield return StartCoroutine(playerBlock());
        
        yield return new WaitForSeconds(waitTime);
        Enemy.Jab();
        PlayAttackSound(jabClip);
        

        yield return StartCoroutine(playerBlock());
        yield return new WaitForSeconds(waitTime);
        Enemy.Shoot();
        PlayAttackSound(jabClip);
        
        
        
        yield return StartCoroutine(playerBlock());
        yield return new WaitForSeconds(waitTime);
        PlayAttackSound(jabClip);
        Enemy.Slash();
        
        

        // Handle attack logic
        //StartCoroutine(HandlePlayerAttack(Enemy));
       // battleManager.PerformAttack(battleManager.enemyStats,battleManager.playerStats);
        


        // Move back to original position
        yield return StartCoroutine(MoveCharacter(Enemy.transform, originalPosition, moveSpeed));

        isMoving = false;

                    Enemy.SetState(CharacterState.Relax);
                    }
        
    
}

private IEnumerator Block(GameObject character)
{
    // Save original position
    Vector3 originalPosition = character.transform.position;

    // Set the character to block state
    character.GetComponent<Character>().SetState(CharacterState.Crouch);
    
    // Shake parameters
    float shakeDuration = 0.2f; // Total shake time
    float shakeMagnitude = 5f; // Magnitude of the shake (how far it shakes)
    float shakeSpeed = 0.1f; // Speed of the shake
    float elapsedTime = 0;

    // Shake the character by moving back and forth quickly
    while (elapsedTime < shakeDuration)
    {
        float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
        character.transform.position = originalPosition + new Vector3(offsetX, 0, 0);

        yield return new WaitForSeconds(shakeSpeed);
        elapsedTime += shakeSpeed;
    }
    PlayAttackSound(blockClip);

    // Return to the original position after shaking
    character.transform.position = originalPosition;

    // Set back to relax state
    character.GetComponent<Character>().SetState(CharacterState.Relax);
}


private IEnumerator DodgeJump(GameObject character)
{
    // Save original position
    Vector3 originalPosition = character.transform.position;

     // Randomly choose to dodge left or right (+5f or -5f)
    float dodgeDirection = Random.Range(0, 2) == 0 ? -5f : 5f;

    // Jump position (move horizontally by dodgeDirection and vertically by 10f)
    Vector3 dodgePosition = originalPosition + new Vector3(dodgeDirection, 10f, 0f);
    float dodgeTime = 0.1f;
    float elapsedTime = 0;

    // Move character up to simulate the jump
    while (elapsedTime < dodgeTime)
    {
        character.transform.position = Vector3.Lerp(originalPosition, dodgePosition, elapsedTime / dodgeTime);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    PlayAttackSound(dodgeClip);
    // Wait briefly at the peak of the dodge
    yield return new WaitForSeconds(0.1f);

    // Return character back to the original position
    elapsedTime = 0;
    while (elapsedTime < dodgeTime)
    {
        character.transform.position = Vector3.Lerp(dodgePosition, originalPosition, elapsedTime / dodgeTime);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Set the character back to the default state after the dodge
    character.GetComponent<Character>().SetState(CharacterState.Relax);
}


private IEnumerator RotateAndCrouch(GameObject character)
{
    // Rotate the character slightly (e.g., to 45 degrees on the X-axis)
    Quaternion originalRotation = character.transform.rotation;
     // Randomly choose to dodge left or right (+5f or -5f)
    float Direction = Random.Range(0, 2) == 0 ? -45f : 45f;

    // Jump position (move horizontally by dodgeDirection and vertically by 10f)
   // Vector3 dodgePosition = originalPosition + new Vector3(dodgeDirection, 10f, 0f);
    Quaternion crouchRotation = Quaternion.Euler(0f, 0f, Direction);
    
    // Rotate to the crouch position
    float rotationTime = 0.1f;
    float elapsedTime = 0;
    while (elapsedTime < rotationTime)
    {
        character.transform.rotation = Quaternion.Slerp(originalRotation, crouchRotation, elapsedTime / rotationTime);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    
    // Set to crouch state and wait
    character.transform.rotation = crouchRotation;
    character.GetComponent<Character>().SetState(CharacterState.Crouch);
    yield return new WaitForSeconds(0.5f);
    
    // Return to original rotation
    elapsedTime = 0;
    while (elapsedTime < rotationTime)
    {
        character.transform.rotation = Quaternion.Slerp(crouchRotation, originalRotation, elapsedTime / rotationTime);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    
    // Return to default state
    character.GetComponent<Character>().SetState(CharacterState.Relax);
     character.transform.rotation = new Quaternion(0,0,0,1);
}

private IEnumerator MoveCharacter(Transform character, Vector3 destination, float speed)
{
    // Total distance to cover
    float totalDistance = Vector3.Distance(character.position, destination);
    
    // Divide the distance into four segments
    float distancePerSound = totalDistance / 4f;
    float distanceCovered = 0f;

    // Play the initial sound once
    PlayAttackSound(runClip);

    while (Vector3.Distance(character.position, destination) > 0.01f)
    {
        // Calculate the distance moved in this frame
        Vector3 previousPosition = character.position;
        character.position = Vector3.MoveTowards(character.position, destination, speed * Time.deltaTime);
        float distanceMoved = Vector3.Distance(previousPosition, character.position);
        distanceCovered += distanceMoved;

        // Play the sound each time the character covers the distance of one segment
        if (distanceCovered >= distancePerSound)
        {
            PlayAttackSound(runClip);
            distanceCovered = 0f; // Reset the covered distance after each sound
        }

        yield return null; // Wait for the next frame
    }
}


private Vector3 cameraOffset = new Vector3(0, 7, -190);  // Customize your offset as needed

// LateUpdate ensures the camera follows after all other updates
    private void LateUpdate()
    {
        FollowCharacterWithCamera();
    }

    private void FollowCharacterWithCamera()
    {
        if (Character != null && PlayerCamera != null)
        {
            // Update the camera position based on the character's position with the offset
            PlayerCamera.transform.position = Character.transform.position + cameraOffset;

            // Set the camera's orthographic size (if using an orthographic camera)
            PlayerCamera.orthographicSize = 8;  // Adjust size if needed
        }
        else
        {
            Debug.LogError("PlayerCamera or Character is not assigned.");
        }
    }


private float battleTimer = 0f;
private float battleInterval = 1f; // Interval of 1 second
private void Update() {
   
         FollowCharacterWithCamera();
         UpdateLifebar();
         // After battle ends

// Launch battle every second

         // Lock input if the state is not waiting for player input
    if (state != State.WaitingForPlayer)
    {
        return; // Exit early if the game is busy (e.g., moving or fighting)
    }
    #if UNITY_ANDROID
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log($"[TRACKING TOUCH] Android touch detected at position: {touch.position} | Time: {System.DateTime.Now}");
            //if (state == State.WaitingForPlayer) {
                    Battle();
                    
                
        }
    }
    #endif
        // For touch input (Android)
        if (Input.touchCount > 0) {
            
            Touch touch = Input.GetTouch(0);

            // Check if the touch is a "Tap"
            if (touch.phase == TouchPhase.Began) {
                if (state == State.WaitingForPlayer) {
                    Battle();
                    
                }

 
            }
        }



                // For spacebar input (Desktop testing)
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (state == State.WaitingForPlayer) {
                    Battle();
                }
            }
                                        
                

                //Character.transform.position = new Vector3(-50, -20);
                // Enemy.transform.position = new Vector3(+50, -20);

        
    }

    // Method to calculate experience based on luck after winning a fight
private void GrantExperienceToWinner()
{
    // Only grant experience to the player if they won the battle
    if (battleManager.playerStats.HP > 0 && battleManager.enemyStats.HP <= 0)
    {
        // Calculate experience based on player's luck
        float baseExperience = 50;  // Example base experience
        float luckFactor = battleManager.playerStats.Luck / 100f;  // Convert luck to a factor
        float experienceGained = baseExperience * (1 + luckFactor);  // Experience influenced by luck

        // Grant the experience to the player
        battleManager.playerStats.GainExperience(experienceGained);

        Debug.Log("Player won the battle and gained " + experienceGained + " experience.");
        CombatstatsText.text  = "Player won the battle and gained " + experienceGained + " experience.";
    }
}



    private IEnumerator HandleCombat() {
    // Lock the state to prevent additional input while combat is ongoing
    state = State.Busy;

    if (battleManager.playerStats.Speed < battleManager.enemyStats.Speed) {
        // Enemy is faster, attacks first
        yield return StartCoroutine(MoveToPlayerAndBack());

        // If the player is still alive, allow them to attack
        if (battleManager.playerStats.HP > 0) {
            yield return new WaitForSeconds(0.5f); // Optional delay between turns
            yield return StartCoroutine(MoveToEnemyAndBack());
        }
    } else {
        // Player is faster, attacks first
        yield return StartCoroutine(MoveToEnemyAndBack());

        // If the enemy is still alive, allow them to attack
        if (battleManager.enemyStats.HP > 0) {
            yield return new WaitForSeconds(0.5f); // Optional delay between turns
            yield return StartCoroutine(MoveToPlayerAndBack());
        }
    }

    // Once both turns are complete, unlock the state and wait for the next player input
    state = State.WaitingForPlayer;
}



private void UpdateLifebar(){
                // Assuming Lifebar1 and Lifebar2 are Image components
Lifebar1.fillAmount = battleManager.playerStats.HP / battleManager.playerMaxHP;
textlife1.SetText(((int)(Lifebar1.fillAmount*100)).ToString() + "%");
//Debug.Log("Lifebar1 = " + Lifebar1.fillAmount );
Lifebar2.fillAmount = battleManager.enemyStats.HP / battleManager.enemyMaxHP;
textlife2.SetText(((int)(Lifebar2.fillAmount*100)).ToString()+ "%");

//Debug.Log("Lifebar2 = " + Lifebar1.fillAmount );
}


private void LoadCharacterConfig(string path,bool player ) {
    ////Debug.Log("PATH " + path);
    if (!File.Exists(path)) {
        //Debug.LogError("Config file not found: " + path);
        return;
    }
    if(player){
        
        playerConfig = playerConfig.LoadPlayerConfig(path);

        // Optional: Log player config for //Debugging purposes
        playerConfig.DisplayPlayerConfig();

    }
    else {
        enemyConfig = enemyConfig.LoadPlayerConfig(path);

        enemyConfig.DisplayPlayerConfig();
    }
    
}


    public void SetupPlayer(string configPath) {

    
    playerConfig = new PlayerConfig();  // Ensure playerConfig is initialized

    // Load the player configuration from the file
    LoadCharacterConfig(configPath,true);

    // Apply the loaded player sprites based on the configuration
    LoadCharacterSprites(true); 
    //Debug.Log("Player is loaded");

}

    public void SetupEnemy(string configPath) {
        enemyConfig = new PlayerConfig();  // Ensure playerConfig is initialized

    // Load the player configuration from the file
    LoadCharacterConfig(configPath,false);

    // Apply the loaded player sprites based on the configuration
    LoadCharacterSprites(false); 
    //Debug.Log("Enemy is loaded");
    }

    public void LoadCharacterSprites(bool isPlayer) {
     

    if (isPlayer) {
        // Use PlayerConfig for the player character
                   //Debug.Log(" LoadCharacterSprites player ") ;
        string headFile = playerConfig.playerHead.path;
        string bodyFile = playerConfig.playerBody.path;
        string weaponFile = playerConfig.playerWeapon.path;
        string spritepath = playerConfig.spriteSheetPath;
        //Debug.Log(" HEAD : " + headFile +" bodyFile : " + bodyFile+ " weaponFile : " + weaponFile+ "Spritsheet at : "+ spritepath ) ;
        // Apply the player-specific textures
        ModifyPlayerBodySpritesheet(bodyFile,true);
        ModifyPlayerWeaponSpritesheet(weaponFile,true);
        ModifyPlayerHeadSpritesheet(headFile,true);
    } else {
                   //Debug.Log(" LoadCharacterSprites enemy ") ;
        string headFile = enemyConfig.playerHead.path;
        string bodyFile = enemyConfig.playerBody.path;
        string weaponFile = enemyConfig.playerWeapon.path; 
        string spritepath = enemyConfig.spriteSheetPath;
        //Debug.Log(" HEAD : " + headFile +" bodyFile : " + bodyFile+ " weaponFile : " + weaponFile+ "Spritsheet at : "+ spritepath ) ;
        


        ModifyPlayerBodySpritesheet(bodyFile,false);
         //Debug.Log(" Enemy body done : ");
        ModifyPlayerWeaponSpritesheet(weaponFile,false);
         //Debug.Log(" Enemy weapon done : ");
        ModifyPlayerHeadSpritesheet(headFile,false);
         //Debug.Log(" Enemy head done : ");
    }
}

    string[] nameSlots = {};

    private void ModifyPlayerBodySpritesheet(string bodyFile,bool player ) {
        //Setup as enemy for default

        string baseImagePath = enemyConfig.spriteSheetPath;
        string outputImagePath = enemyConfig.spriteSheetPath;

        if (player){
            // Paths to files and folders
         baseImagePath = playerConfig.spriteSheetPath;
         outputImagePath = playerConfig.spriteSheetPath;

        }
        

        // Load the base spritesheet
        Texture2D baseSpritesheet = LoadTextureFromFile(baseImagePath);

        if (baseSpritesheet == null) {
            //Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected body image
        Texture2D selectedBodies = LoadTextureFromFile(bodyFile);

        if (selectedBodies == null) {
            //Debug.LogError("Failed to load selected bodies.");
            return;
        }

        // Resize the loaded bodies image if necessary
        selectedBodies = ResizeTexture(selectedBodies, selectedBodies.width, selectedBodies.height);

        // Define coordinates for the body (adjust based on your spritesheet)
        Rect body1Rect = new Rect(20, 300, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body1Rect);

        Rect body2Rect = new Rect(180-30, 300, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body2Rect);

        Rect body3Rect = new Rect(300-30, 300, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body3Rect);

        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        //Debug.Log($"Modified spritesheet saved to {outputImagePath}");
    }

    private void ModifyPlayerHeadSpritesheet(string bodyFile,bool player) {
        // Paths to files and folders
        string baseImagePath = enemyConfig.spriteSheetPath;
        string outputImagePath = enemyConfig.spriteSheetPath;

        if (player){
            // Paths to files and folders
         baseImagePath = playerConfig.spriteSheetPath;
         outputImagePath = playerConfig.spriteSheetPath;

        }

        // Load the base spritesheet
        Texture2D baseSpritesheet = LoadTextureFromFile(baseImagePath);

        if (baseSpritesheet == null) {
            //Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected body image
        Texture2D selectedHead = LoadTextureFromFile("Assets/TurnBattleSystem/Textures/head.png");

        if (selectedHead == null) {
            //Debug.LogError("Failed to load selected bodies.");
            return;
        }

        // Resize the loaded bodies image if necessary
        selectedHead = ResizeTexture(selectedHead, selectedHead.width, selectedHead.height);

        // Define coordinates for the body (adjust based on your spritesheet)
        Rect body1Rect = new Rect(25, 432, selectedHead.width, selectedHead.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedHead, body1Rect);

        Rect body2Rect = new Rect(180-30, 432, selectedHead.width, selectedHead.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedHead, body2Rect);

        Rect body3Rect = new Rect(300-30, 432, selectedHead.width, selectedHead.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedHead, body3Rect);

        

        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        //Debug.Log($"Modified spritesheet saved to {outputImagePath}");
    }


    public void ModifyPlayerWeaponSpritesheet(string weaponFile,bool player) {
        //Debug.Log("Modify playterSpriteshete");
        // Paths to files and folders
        string baseImagePath = enemyConfig.spriteSheetPath;
        string outputImagePath = enemyConfig.spriteSheetPath;

        if (player){
            // Paths to files and folders
         baseImagePath = playerConfig.spriteSheetPath;
         outputImagePath = playerConfig.spriteSheetPath;

        }

        // Load the base spritesheet
        Texture2D baseSpritesheet = LoadTextureFromFile(baseImagePath);

        if (baseSpritesheet == null) {
            //Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected weapon image
        Texture2D selectedWeapons = LoadTextureFromFile(weaponFile);

        if (selectedWeapons == null) {
            //Debug.LogError("Failed to load selected weapons.");
            return;
        }

        // Resize the loaded weapon image if necessary
        selectedWeapons = ResizeTexture(selectedWeapons, selectedWeapons.width, selectedWeapons.height);

        // Define coordinates for the wepons (adjust based on your spritesheet)
        Rect weapon1Rect = new Rect(50, 180, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon1Rect);


        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        //Debug.Log($"Modified spritesheet saved to {outputImagePath}");
       // SetupPlayer("configPlayer.txt");
    }
    

public void ModifyPlayerArmorSpritesheet(string armorFile, bool player) {
    //Debug.Log("Modify PlayerArmorSpritesheet");

    // Paths to files and folders
    string baseImagePath = enemyConfig.spriteSheetPath;
        string outputImagePath = enemyConfig.spriteSheetPath;

        if (player){
            // Paths to files and folders
         baseImagePath = playerConfig.spriteSheetPath;
         outputImagePath = playerConfig.spriteSheetPath;

        }

    // Load the base spritesheet
    Texture2D baseSpritesheet = LoadTextureFromFile(baseImagePath);

    if (baseSpritesheet == null) {
        //Debug.LogError("Failed to load base spritesheet.");
        return;
    }

    // Load the selected armor image
    Texture2D selectedArmor = LoadTextureFromFile(armorFile);

    if (selectedArmor == null) {
        //Debug.LogError("Failed to load selected armor.");
        return;
    }

    // Resize the loaded armor image if necessary
    selectedArmor = ResizeTexture(selectedArmor, selectedArmor.width, selectedArmor.height);

    // Define coordinates for the armor (adjust based on your spritesheet)
    Rect armorRect = new Rect(baseSpritesheet.width - selectedArmor.width, baseSpritesheet.height - selectedArmor.height, selectedArmor.width, selectedArmor.height); // Top-right position

    // Replace the specific section of the spritesheet with the armor image
    ReplaceTextureSection(baseSpritesheet, selectedArmor, armorRect);

    // Save the modified spritesheet
    SaveTextureToFile(baseSpritesheet, outputImagePath);
    //Debug.Log($"Modified spritesheet saved to {outputImagePath}");

    // Optionally, you can trigger player setup here if needed.
    // SetupPlayer("configPlayer.txt");
}

public void ModifyPlayerFeetSpritesheet(string weaponFile, bool player) {
        //Debug.Log("Modify playterSpriteshete");
        string baseImagePath = enemyConfig.spriteSheetPath;
        string outputImagePath = enemyConfig.spriteSheetPath;

        if (player){
            // Paths to files and folders
         baseImagePath = playerConfig.spriteSheetPath;
         outputImagePath = playerConfig.spriteSheetPath;

        }  

        // Load the base spritesheet
        Texture2D baseSpritesheet = LoadTextureFromFile(baseImagePath);

        if (baseSpritesheet == null) {
            //Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected weapon image
        Texture2D selectedWeapons = LoadTextureFromFile(weaponFile);

        if (selectedWeapons == null) {
            //Debug.LogError("Failed to load selected weapons.");
            return;
        }

        // Resize the loaded weapon image if necessary
        selectedWeapons = ResizeTexture(selectedWeapons, selectedWeapons.width, selectedWeapons.height);

        // Define coordinates for the wepons (adjust based on your spritesheet)
        Rect weapon1Rect = new Rect(50, 160, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon1Rect);


        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        //Debug.Log($"Modified spritesheet saved to {outputImagePath}");
       // SetupPlayer("configPlayer.txt");
    }


    public void ModifyPlayerLegsSpritesheet(string weaponFile, bool player) {
        //Debug.Log("Modify playterSpriteshete");
        // Paths to files and folders
        string baseImagePath = enemyConfig.spriteSheetPath;
        string outputImagePath = enemyConfig.spriteSheetPath;

        if (player){
            // Paths to files and folders
         baseImagePath = playerConfig.spriteSheetPath;
         outputImagePath = playerConfig.spriteSheetPath;

        }


        // Load the base spritesheet
        Texture2D baseSpritesheet = LoadTextureFromFile(baseImagePath);

        if (baseSpritesheet == null) {
            //Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected weapon image
        Texture2D selectedWeapons = LoadTextureFromFile(weaponFile);

        if (selectedWeapons == null) {
            //Debug.LogError("Failed to load selected weapons.");
            return;
        }

        // Resize the loaded weapon image if necessary
        selectedWeapons = ResizeTexture(selectedWeapons, selectedWeapons.width, selectedWeapons.height);

        // Define coordinates for the wepons (adjust based on your spritesheet)
        Rect weapon1Rect = new Rect(50, 230, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon1Rect);


        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        //Debug.Log($"Modified spritesheet saved to {outputImagePath}");
       // SetupPlayer("configPlayer.txt");
    }

private Texture2D LoadTextureFromFile(string filePath)
{
    // Check if the filePath is valid
    if (string.IsNullOrEmpty(filePath))
    {
        //Debug.LogError("Error: filePath is null or empty.");
        return null;
    }

    // Check if the file exists
    if (!System.IO.File.Exists(filePath))
    {
        //Debug.LogError($"Error: File not found at {filePath}");
        return null;
    }

    try
    {
            //Debug.Log($"Loading texture from {filePath}");

        byte[] fileData = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        return texture;
    }
    catch (Exception ex)
    {
        //Debug.LogError($"Error while loading texture from {filePath}: {ex.Message}");
        return null;
    }
}

    

  


    private void SaveTextureToFile(Texture2D texture, string filePath) {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null) {
            File.WriteAllBytes(filePath, pngData);
        }
    }

    private void ReplaceTextureSection(Texture2D baseTexture, Texture2D sectionTexture, Rect sectionRect) {
        Color[] sectionPixels = sectionTexture.GetPixels(0, 0, (int)sectionRect.width, (int)sectionRect.height);
        baseTexture.SetPixels((int)sectionRect.x, (int)sectionRect.y, (int)sectionRect.width, (int)sectionRect.height, sectionPixels);
        baseTexture.Apply();
    }

    private Texture2D ResizeTexture(Texture2D sourceTexture, int targetWidth, int targetHeight) {
        RenderTexture renderTexture = RenderTexture.GetTemporary(targetWidth, targetHeight);
        Graphics.Blit(sourceTexture, renderTexture);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;

        Texture2D resultTexture = new Texture2D(targetWidth, targetHeight);
        resultTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        resultTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return resultTexture;
    }
}

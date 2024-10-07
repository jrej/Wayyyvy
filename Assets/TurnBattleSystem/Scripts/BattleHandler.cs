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

public Button CreateCharacterbutton;

public TextMeshProUGUI textlife1 ;
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
public ConnectionManager connectionManager;

    public List<CharacterPosition> characterPositions = new List<CharacterPosition>(); // Store character positions


    public Button toggleInventoryButton;
    public Button createButton ; 
    public Button StartButton;
         public Button toggleInInventoryButton;
           public Button ConnectionButton;
           public Button FightButton;

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

        private string[] arenaImages = { "Assets/arena1.png", "Assets/arena2.png", "Assets/arena3.png", "Assets/arena4.png", "Assets/arena5.png" };  // Array of arena image names
 private SpriteRenderer spriteRenderer;


    // Function to randomly change the background image
    public void ChangeBackground()
    {
        // Randomly select one of the arena images
        string selectedArena = arenaImages[UnityEngine.Random.Range(0, arenaImages.Length)];

        // Load the corresponding sprite (assuming the images are located in Resources folder)
        Sprite newSprite = Resources.Load<Sprite>(selectedArena);

        if (newSprite != null)
        {
            // Set the sprite to the background's SpriteRenderer
            spriteRenderer.sprite = newSprite;
            Debug.Log("Background changed to: " + selectedArena);
        }
        else
        {
            Debug.LogError("Sprite not found for: " + selectedArena);
        }
    }
    public void Mapbackground()
    {
        // Randomly select one of the arena images
        string selectedArena = "Assets/map.png";

        // Load the corresponding sprite (assuming the images are located in Resources folder)
        Sprite newSprite = Resources.Load<Sprite>(selectedArena);

        if (newSprite != null)
        {
            // Set the sprite to the background's SpriteRenderer
            spriteRenderer.sprite = newSprite;
            Debug.Log("Background changed to: " + selectedArena);
        }
        else
        {
            Debug.LogError("Sprite not found for: " + selectedArena);
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


    private void Awake() {
    instance = this;
    canvas = GameObject.Find("Canvas2");
    characters = FindObjectsOfType<Character>();

    // Save the initial positions, sizes, and rotations of each character
    foreach (Character character in characters)
    {
        string characterName = character.gameObject.name;
        Vector3 initialPosition = character.transform.position;
        Vector3 initialSize = character.transform.localScale;
        Quaternion initialRotation = character.transform.rotation; // Store the initial rotation

        // Store the name, initial position, size, and rotation
        characterPositions.Add(new CharacterPosition(characterName, initialPosition, initialSize, initialRotation));
    }

    // Log the initial positions and rotations
    foreach (CharacterPosition cp in characterPositions)
    {
        Debug.Log($"Character {cp.characterName} initial position: {cp.initialPosition}, rotation: {cp.initialRotation}");
    }

    inventoryMenu.SetActive(false);
    SelectionMenu.SetActive(false);
    HideCharacters();
    state = State.WaitingForPlayer;
}




// Function to reset all characters to their initial positions
    public void ResetToInitialPositions()
{
    foreach (Character character in characters)
    {
        character.SetExpression("Default");
        Character.SetState(CharacterState.Idle);
        foreach (CharacterPosition cp in characterPositions)
        {
            // Check if the name of the character matches the saved position's name
            if (character.gameObject.name == cp.characterName)
            {
                // Reset the character's position, size, and rotation
                character.transform.position = cp.initialPosition;
                character.transform.localScale = cp.initialSize;
                character.transform.rotation = cp.initialRotation;

                // Log the reset action
                Debug.Log($"Character {cp.characterName} reset to initial position: {cp.initialPosition} and rotation: {cp.initialRotation}");
            }
        }
    }
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
   ChangeBackground();
    SelectionMenu.SetActive(false);
    
    //buttonCanvas.SetActive(true);
    //buttonCanvas.SetActive(false);
    enemyIndexnum = enemyIndex;
    enemyConfigPath = $"configEnemy{enemyIndex}.json";
    playerConfigPath = "configPlayer.json" ;
   //         SetupPlayer("configPlayer.json");
  //  editor.LoadFromJson("configPlayer.json");
  //  editor.LoadFromJson(enemyConfigPath);
    //SetupEnemy(enemyConfigPath); // Load the selected enemy config file
    StartNewGame(); // Start the battle with the selected enemy
}


private void StartNewGame() {
    // Launch coroutines to spawn player and enemy characters concurrently
    StartCoroutine(SpawnCharacterCoroutine());  // Spawn player character
   
}

// Coroutine to spawn characters asynchronously
private IEnumerator SpawnCharacterCoroutine() {
    // Simulate a delay for loading, customization, or other setup if necessary
    yield return new WaitForSeconds(0.5f); // Adjust or remove the delay as needed

    // Call your SpawnCharacter method
    SpawnCharacter();
    FightButton.gameObject.SetActive(true);

    

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
        // Get the Animator component
        connectionManager = new ConnectionManager();
        connectionManager.Initialize();
        foreach (Character character in characters)
        {
            if (character.gameObject.name == "Human")
            {
                 connectionManager.characterEditor.Character =  humanCharacter ;

                
            }
        }
        // Get the SpriteRenderer component from the backg GameObject
        spriteRenderer = backg.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the backg GameObject.");
            return;
        }
        StartButton.onClick.AddListener(StartGame);
                CreateAccountButton.onClick.AddListener(Login);
                CreateCharacterbutton.onClick.AddListener(createCaracter);

        createButton.onClick.AddListener(SignIn);
        ConnectionButton.onClick.AddListener(Connect);
        FightButton.onClick.AddListener(Battle);

        

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

     // Method to toggle the inventory menu
    private void StartGame() {
        //Debug.Log("Starting Game ");
        startCanvas.SetActive(false);
        LoginMenu.SetActive(true);
        
        
    }
    public void Connect()
{
    connectionManager.emailInput = emailInput.text;
    connectionManager.passwordInput = passwordInput.text;

    // Use a coroutine to load characters to avoid UI freezing
    StartCoroutine(LoadCharactersCoroutine());
}

private IEnumerator LoadCharactersCoroutine()
{
    connectionManager.Connect(Character);
    Debug.Log("Connected");

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

    private void Battle(){
        Debug.Log("FIght button: state : " + state);
                FaceEachOther(Character.gameObject, Enemy.gameObject);

       
        state = State.Busy;

            if(battleManager.playerStats.HP > 0 && battleManager.enemyStats.HP > 0  ){
                StartCoroutine(HandleCombat());
            }
            else{

                if(battleManager.playerStats.HP <= 0 ){
                    Character.SetState(CharacterState.DeathB);
                    Character.SetExpression("Dead");
                    Character.ResetAnimation();
                   //Character.transform.Rotate(-90,00,00);
                }
                else{
                    Enemy.SetState(CharacterState.DeathB);
                    Enemy.SetExpression("Dead");
                   Enemy.ResetAnimation();
                }   

                SelectionMenu.SetActive(true);
                FightButton.gameObject.SetActive(false);
                
                Character.SetState(CharacterState.Walk);
                HideCharacters();
                ResetToInitialPositions();
                DisplayCharactersOnScreen();
                Mapbackground();
            
           // Debug.Log("FIght is Over:");
            }
            state = State.WaitingForPlayer;
    }

      private void SignIn() {
        Debug.Log("SignIn  ");
        SignInMenu.SetActive(true);
        LoginMenu.SetActive(false);
      //  buttonCanvas.SetActive(true);
        DisplayCharactersOnScreen();
    }

      private void Login() {
        Debug.Log("Login ");
        SignInMenu.SetActive(false);
        LoginMenu.SetActive(true);
        
    }

 public void UpdateCharacterInDatabase()
{
    // Retrieve the list of character JSON paths from the connectionManager
    List<string> characterFilePathList = connectionManager.PlayerDatadata.characterJsonPaths;

    Debug.Log("Character Editor Loaded Name: " + connectionManager.characterEditor.Character.name);
    
    // Flag to track if character was found
    bool characterFound = false;

    // Iterate through the paths to find the matching character JSON
    foreach (var characterPath in characterFilePathList)
    {
        // Extract the character name from the JSON file name (stored in characterPath)
        string characterName = Path.GetFileNameWithoutExtension(characterPath);
        
        Debug.Log("Comparing with character name from path: " + characterName);

        // Compare it with the name loaded from the connectionManager.characterEditor
        if (connectionManager.characterEditor.Character.name == characterName)
        {
            // Update the character in the database using the JSON name
            connectionManager.UpdatePlayerCharacter(connectionManager.emailInput, Character, characterName);
            Debug.Log("Character " + characterName + " has been updated in the database.");
            characterFound = true;
            break;
        }
    }

    if (!characterFound)
    {
        Debug.LogError("Character not found in the player's character list. Adding the character.");
        
        // Define the character name from the loaded character in the editor
        string newCharacterName = connectionManager.characterEditor.Character.name;
        
        // Create the path for the new character JSON file
        string directoryPath = Path.Combine("Assets", "Account", connectionManager.emailInput, "Characters");
        string characterFilePath = Path.Combine(directoryPath, newCharacterName + ".json");

        // Add the new character's path to the player's list of characters
        connectionManager.PlayerDatadata.characterJsonPaths.Add(characterFilePath);
        
        // Save the character to the file system and update the database
        connectionManager.UpdatePlayerCharacter(connectionManager.emailInput, Character, newCharacterName);

        Debug.Log("New character " + newCharacterName + " added and saved to the database.");
    }

    // Make sure to save the database after any modification
    connectionManager.SavePlayerDatabase();

    // Simulate a "unit test" to ensure the character can be retrieved
    SimulateCharacterRetrievalTest();
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
        bool isActive = inventoryMenu.activeSelf;
        inventoryMenu.SetActive(!isActive);

        if (isActive)
        {
            // Closing inventory, save character to database
            SaveCharacter();
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
        }
}
   private void QuitGame() {
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
    if(battleManager.playerStats.HP <= 0 ){
        Character.SetState(CharacterState.DeathB);
                Character.SetExpression("Dead");
    }else{
   // state = State.Busy;
    isMoving = true;
                Character.SetState(CharacterState.Run);
                Character.SetExpression("Default");
    

    
    // Move toward the enemy
    yield return StartCoroutine(MoveCharacter(Character.transform, targetPosition, moveSpeed));

    // Perform attacks after reaching the enemy
    Character.SetState(CharacterState.Run);
    
    // Perform attack sequence
    Character.Jab();
    yield return new WaitForSeconds(waitTime);

    Character.Shoot();
    yield return new WaitForSeconds(waitTime);

    Character.Slash();
    yield return new WaitForSeconds(waitTime);
    battleManager.PerformAttack(battleManager.playerStats,battleManager.enemyStats);
    UpdateLifebar();



    // Move back to original position
    yield return StartCoroutine(MoveCharacter(Character.transform, originalPosition, moveSpeed));
    



    isMoving = false;
    //state = State.WaitingForPlayer; // Set back to waiting after moving back
                Character.SetState(CharacterState.Relax);
                // Handle attack logic
   // StartCoroutine(HandlePlayerAttack(Enemy));
   }

}
private IEnumerator MoveToPlayerAndBack() {
    if(battleManager.enemyStats.HP <= 0 ){
        Enemy.SetState(CharacterState.DeathB);
                Enemy.SetExpression("Dead");
    }else{
    
    isMoving = true;
    Enemy.SetState(CharacterState.Run);
    originalPosition = Enemy.transform.position; // Save original position
    targetPosition = Character.transform.position - new Vector3(-10f, 0f, 0f); // Target position near the enemy

    
    // Move toward the enemy
    yield return StartCoroutine(MoveCharacter(Enemy.transform, targetPosition, moveSpeed));

    // Perform attacks after reaching the enemy
    Enemy.SetState(CharacterState.Run);
    
    // Perform attack sequence
    Enemy.Jab();
    yield return new WaitForSeconds(waitTime);

    Enemy.Shoot();
   yield return new WaitForSeconds(waitTime);

    Enemy.Slash();
   yield return new WaitForSeconds(waitTime);

    // Handle attack logic
    //StartCoroutine(HandlePlayerAttack(Enemy));
        battleManager.PerformAttack(battleManager.enemyStats,battleManager.playerStats);
        UpdateLifebar();


    // Move back to original position
    yield return StartCoroutine(MoveCharacter(Enemy.transform, originalPosition, moveSpeed));

    isMoving = false;

                Enemy.SetState(CharacterState.Relax);
                }
}

private IEnumerator MoveCharacter(Transform character, Vector3 destination, float speed) {
    while (Vector3.Distance(character.position, destination) > 0.01f) {
        character.position = Vector3.MoveTowards(character.position, destination, speed * Time.deltaTime);
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



private void Update() {
   
         FollowCharacterWithCamera();
    if (state == State.WaitingForPlayer) {
        // For touch input (Android)
        if (Input.touchCount > 0) {
            
            Touch touch = Input.GetTouch(0);

            // Check if the touch is a "Tap"
            if (touch.phase == TouchPhase.Ended) {
                Battle();  
            }
        }
        // For spacebar input (Desktop testing)
        if (Input.GetKeyDown(KeyCode.Space)) {   
            Battle();   
        }
                //Character.transform.position = new Vector3(-50, -20);
                // Enemy.transform.position = new Vector3(+50, -20);

        }
    }


    private IEnumerator HandleCombat(){
         if(battleManager.playerStats.Speed <battleManager.enemyStats.Speed){

                originalPosition = Character.transform.position; // Save original position
                targetPosition = Enemy.transform.position - new Vector3(10f, 0f, 0f); // Target position near the enemy
                yield return MoveToEnemyAndBack(); // Start movement coroutine

                originalPosition = Enemy.transform.position; // Save original position
                targetPosition = Character.transform.position - new Vector3(10f, 0f, 0f); // Target position near the enemy
                yield return MoveToPlayerAndBack(); // Start movement coroutine


            }else{

                originalPosition = Enemy.transform.position; // Save original position
                targetPosition = Character.transform.position - new Vector3(10f, 0f, 0f); // Target position near the enemy
                 yield return MoveToPlayerAndBack(); // Start movement coroutine
                originalPosition = Character.transform.position; // Save original position
                targetPosition = Enemy.transform.position - new Vector3(10f, 0f, 0f); // Target position near the enemy
                yield return MoveToEnemyAndBack(); // Start movement coroutine
                

            }

    }
private void UpdateLifebar(){
                // Assuming Lifebar1 and Lifebar2 are Image components
Lifebar1.fillAmount = battleManager.playerStats.HP / battleManager.playerMaxHP;
textlife1.SetText(((int)(Lifebar1.fillAmount*100)).ToString());
//Debug.Log("Lifebar1 = " + Lifebar1.fillAmount );
Lifebar2.fillAmount = battleManager.enemyStats.HP / battleManager.enemyMaxHP;
textlife2.SetText(((int)(Lifebar2.fillAmount*100)).ToString());

//Debug.Log("Lifebar2 = " + Lifebar1.fillAmount );
}

private IEnumerator HandlePlayerAttack(Character enemyCharacter) {
    // Wait for attack animation to complete (adjust the time to match your animation)
   
    // Handle post-attack logic (e.g., applying damage)
return MoveToPlayerAndBack();

    // Choose the next active character after the attack
    //ChooseNextActiveCharacter();
}

private void ApplyDamageToEnemy(Character enemyCharacter) {
    // Logic to apply damage to the enemy (customize as needed)
    Debug.Log("Player attacked the enemy!");
    enemyCharacter.SetExpression("Angry");
    // e.g., enemyCharacter.TakeDamage(damageAmount);
        }






    private void SetActiveCharacterBattle(CharacterBattle characterBattle) {
        if (activeCharacterBattle != null) {
            activeCharacterBattle.HideSelectionCircle();
    }


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

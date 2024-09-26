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
using UnityEditor.Build.Reporting;
using Assets.HeroEditor.Common.Scripts.CharacterScripts.Firearms;
using Assets.HeroEditor.Common.Scripts.CharacterScripts.Firearms.Enums;
using HeroEditor.Common.Enums;
using Unity.VisualScripting;

public class Dialogue {
    public List<string> lines;

    public Dialogue() {
        lines = new List<string>();
    }

    public Dialogue(List<string> lines) {
        this.lines = lines;
    }
}

public class BattleHandler : MonoBehaviour {

//public KeyCode FireButton;
        public KeyCode ReloadButton;
    private static BattleHandler instance;
    private Dictionary<string, string> config;

    private CharacterCustomizationUI characterUi;

    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    public Texture2D copSpritesheet;

    //private CharacterBattle playerCharacterBattle;
   // private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    private State state;
    private Text dialogueText;

    public TMP_InputField emailInput;
        public TMP_InputField passwordInput;


private CharacterEditor editor;

    private Dialogue preFightDialogue;

    public AudioClip musicClip;  // Assign this in the Inspector
    private AudioSource audioSource;

    private GameObject canvas;
    private GameObject textObj;

    private GameObject playerSpriteObj;
    private GameObject enemySpriteObj;

    GameObject customizationUIObject;

    private GameObject backgroundObj;
string enemyConfigPath;
string playerConfigPath;

public Button CreateAccountButton;
    private GameObject quitButtonObj;

    public PlayerConfig playerConfig;
        public PlayerConfig enemyConfig;


        public Character Character;
        public Character Enemy;
// Instantiate HeroEditor character prefab

        public Firearm Firearm;
        public Transform ArmL;
        public Transform ArmR;
        public KeyCode FireButton;
        [Header("Check to disable arm auto rotation.")]
	    public bool FixedArm;

    public GameObject inventoryMenu;
        public GameObject SelectionMenu;
        public GameObject LoginMenu;

                public GameObject SignInMenu;

            public GameObject startCanvas; // Drag the StartCanvas into this field in the Inspector


    public Button toggleInventoryButton;
    public Button createButton ; 
    public Button StartButton;
         public Button toggleInInventoryButton;
           public Button ConnectionButton;

        public Button Enemy1;
        public Button Enemy2;
        public Button Enemy3;
        public Button Enemy4;
    public Character CharacterPrefab;
    public Character EnemyPrefab;
    public Button quitButton;
        public GameObject buttonCanvas;


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
  private void LoadUi()
{
    // Try to find the SelectionCanvas in the scene
    Transform selectionCanvasTransform = GameObject.Find("SelectionCanvas")?.transform;

    if (selectionCanvasTransform == null)
    {
        Debug.LogError("SelectionCanvas not found.");
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
                Debug.Log("Player sprite gear loaded successfully.");
            }
            else
            {
                Debug.LogError("Image component not found on PlayerSpriteGearSelect.");
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
        Debug.Log($"Loaded enemy image from {imagePath}");
    }
    else
    {
        Debug.LogError("Image component not found on " + enemyTransform.name);
    }
}


    private void Awake() {



        instance = this;
    
    canvas = GameObject.Find("Canvas2");
    inventoryMenu.SetActive(false);
    SelectionMenu.SetActive(false);
    
    buttonCanvas.SetActive(false);
                

        
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
    //editor = new CharacterEditor();
    SelectionMenu.SetActive(false);
    buttonCanvas.SetActive(true);
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
    StartCoroutine(SpawnCharacterCoroutine(true));  // Spawn player character
    StartCoroutine(SpawnCharacterCoroutine(false)); // Spawn enemy character
}

// Coroutine to spawn characters asynchronously
private IEnumerator SpawnCharacterCoroutine(bool isPlayerTeam) {
    // Simulate a delay for loading, customization, or other setup if necessary
    yield return new WaitForSeconds(0.5f); // Adjust or remove the delay as needed

    // Call your SpawnCharacter method
    SpawnCharacter(isPlayerTeam);

    Debug.Log(isPlayerTeam ? "Player character spawned." : "Enemy character spawned.");
}

// Updated SpawnCharacter method
private void SpawnCharacter(bool isPlayerTeam) {
    Vector3 position = isPlayerTeam ? new Vector3(-50, -20) : new Vector3(+50, -20);

    if (isPlayerTeam) {
        // Instantiate HeroEditor character prefab for the player
        Character = Instantiate(CharacterPrefab);  // Ensure CharacterPrefab is assigned in the Inspector

        // Set character position and scale
        Character.transform.position = position;
        Character.transform.localScale = new Vector3(5f, 5f, 5f);

        Debug.Log("Setting up player character with sprite: " + playerConfigPath);

        // Load character from JSON using the HeroEditor CharacterEditor
        var characterEditor = Character.GetComponent<CharacterEditor>();
        if (characterEditor != null) {
            characterEditor.LoadFromJson(playerConfigPath);  // Load customization from JSON
        } else {
            Debug.LogError("CharacterEditor component missing on the Character prefab.");
        }

        // Start the character (if any additional logic is required after loading)
        characterEditor?.Start();
        
    } else {
        // Instantiate HeroEditor character prefab for the enemy
        Enemy = Instantiate(EnemyPrefab);  // Ensure EnemyPrefab is assigned in the Inspector

        // Set enemy position, rotation, and scale
        Enemy.transform.position = position;
        Enemy.transform.Rotate(0, 180, 0);
        Enemy.transform.localScale = new Vector3(5f, 5f, 5f);

        Debug.Log("Setting up enemy character with sprite: " + enemyConfigPath);

        // Load enemy character from JSON using the HeroEditor CharacterEditor
        var enemyEditor = Enemy.GetComponent<CharacterEditor>();
        if (enemyEditor != null) {
            enemyEditor.LoadFromJson(enemyConfigPath);  // Load customization from JSON
        } else {
            Debug.LogError("CharacterEditor component missing on the Enemy prefab.");
        }

        // Start the enemy character (if additional logic is required after loading)
        enemyEditor?.Start();
    }
}




public void UpdateConfigWithSelectedWeapon(string weaponPath) {
    // Example implementation for updating config
    // This should be replaced with your actual config update logic
    Debug.Log("Updating config with weapon: " + weaponPath);
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
        StartButton.onClick.AddListener(StartGame);
                CreateAccountButton.onClick.AddListener(Login);

        createButton.onClick.AddListener(SignIn);
        ConnectionButton.onClick.AddListener(Connect);

        toggleInInventoryButton.onClick.AddListener(ToggleInventoryMenu);
        toggleInventoryButton.onClick.AddListener(ToggleInventoryMenu);
        quitButton.onClick.AddListener(QuitGame);
        // Add listeners for enemy buttons using lambda expressions
        Enemy1.onClick.AddListener(() => LoadEnemyAndStartBattle(1));
        Enemy2.onClick.AddListener(() => LoadEnemyAndStartBattle(2));
        Enemy3.onClick.AddListener(() => LoadEnemyAndStartBattle(3));
        Enemy4.onClick.AddListener(() => LoadEnemyAndStartBattle(4));

            LoadUi();
      

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
            Debug.LogError($"Image file not found at path: {imagePath}");
        }
    }

     // Method to toggle the inventory menu
    private void StartGame() {
        Debug.Log("Starting Game ");
        startCanvas.SetActive(false);
        LoginMenu.SetActive(true);
        
    }
    private void Connect() {
        Debug.Log("Starting Connection email "+ emailInput.text + "pass : " +passwordInput.text);
        LoginMenu.SetActive(false);
        SelectionMenu.SetActive(true);
        
    }

      private void SignIn() {
        Debug.Log("SignIn  ");
        SignInMenu.SetActive(true);
        LoginMenu.SetActive(false);
        
    }

      private void Login() {
        Debug.Log("Login ");
        SignInMenu.SetActive(false);
        LoginMenu.SetActive(true);
        
    }


    // Method to toggle the inventory menu
    private void ToggleInventoryMenu() {
        Debug.Log("Toglle inventory "+ inventoryMenu.activeSelf);
        if (inventoryMenu != null) {
            bool isActive = inventoryMenu.activeSelf;
            inventoryMenu.SetActive(!isActive);
        }
    }

   private void QuitGame() {
    Debug.Log("Quitting game...");

    GameObject humanClone = GameObject.Find("Human(Clone)");
    if (humanClone != null) {
        Destroy(humanClone);
        Debug.Log("Human(Clone) destroyed.");
    }
    GameObject Clone1 = GameObject.Find("character1(Clone)");
    if (humanClone != null) {
        Destroy(humanClone);
        Debug.Log("character1(Clone) destroyed.");
    }
    GameObject Clone2 = GameObject.Find("character2(Clone)");
    if (humanClone != null) {
        Destroy(humanClone);
        Debug.Log("character2(Clone) destroyed.");
    }
    GameObject Clone3 = GameObject.Find("character3(Clone)");
    if (humanClone != null) {
        Destroy(humanClone);
        Debug.Log("character3(Clone) destroyed.");
    }
    GameObject Clone4 = GameObject.Find("character4(Clone)");
    if (humanClone != null) {
        Destroy(humanClone);
        Debug.Log("character4(Clone) destroyed.");
    }


    Destroy(Character.gameObject);
    Destroy(Enemy.gameObject);


    SelectionMenu.SetActive(false);
    buttonCanvas.SetActive(false);
    startCanvas.SetActive(true);

   /* // If we are running on Android
    #if UNITY_ANDROID
        // Close the application on Android
        Application.Quit();
    #else
        // For any other platform (like in the Unity Editor or Desktop)
        Application.Quit();
    #endif*/
}




private bool isMoving = false;
private Vector3 originalPosition;
private Vector3 targetPosition;
private float moveSpeed = 100f; // Speed of movement
private float waitTime = 1f; // Time to wait between actions


private IEnumerator MoveToEnemyAndBack() {
    state = State.Busy;
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


    // Move back to original position
    yield return StartCoroutine(MoveCharacter(Character.transform, originalPosition, moveSpeed));



    isMoving = false;
    state = State.WaitingForPlayer; // Set back to waiting after moving back
                Character.SetState(CharacterState.Relax);
                // Handle attack logic
    StartCoroutine(HandlePlayerAttack(Enemy));

}
private IEnumerator MoveToPlayerAndBack() {
    state = State.Busy;
    isMoving = true;
    Enemy.SetState(CharacterState.Run);
    originalPosition = Enemy.transform.position; // Save original position
    targetPosition = Character.transform.position - new Vector3(0.5f, 0f, 0f); // Target position near the enemy

    
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

    // Move back to original position
    yield return StartCoroutine(MoveCharacter(Enemy.transform, originalPosition, moveSpeed));

    isMoving = false;
    state = State.WaitingForPlayer; // Set back to waiting after moving back
                Enemy.SetState(CharacterState.Relax);
ApplyDamageToEnemy(Character);
}

private IEnumerator MoveCharacter(Transform character, Vector3 destination, float speed) {
    while (Vector3.Distance(character.position, destination) > 0.01f) {
        character.position = Vector3.MoveTowards(character.position, destination, speed * Time.deltaTime);
        yield return null; // Wait for the next frame
    }
}





private void Update() {
    //animator = Character.GetComponent<Animator>();
    
    if (animator = null){
            Debug.Log("no animator detected in character");

    }

    if (state == State.WaitingForPlayer) {
        // For touch input (Android)
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is a "Tap"
            if (touch.phase == TouchPhase.Ended) {
                state = State.Busy;

                // Trigger player attack animation
                //Character.GetComponent<Animator>().SetTrigger("Slash");; // Use HeroEditor attack animation trigger
                //Character.GetReady();
                // After the animation, handle logic
                Character.Jab();
                StartCoroutine(HandlePlayerAttack(Enemy));
            }
        }

        // For spacebar input (Desktop testing)
        if (Input.GetKeyDown(KeyCode.Space)) {

            originalPosition = Character.transform.position; // Save original position
            targetPosition = Enemy.transform.position - new Vector3(1f, 0f, 0f); // Target position near the enemy
            StartCoroutine(MoveToEnemyAndBack()); // Start movement coroutine
            
        }
    }
}

private IEnumerator HandlePlayerAttack(Character enemyCharacter) {
    // Wait for attack animation to complete (adjust the time to match your animation)
   
    // Handle post-attack logic (e.g., applying damage)
    ApplyDamageToEnemy(enemyCharacter);
return MoveToPlayerAndBack();

    // Choose the next active character after the attack
    //ChooseNextActiveCharacter();
}

private void HandleEnemyAttack(Character enemyCharacter) {
    // Wait for attack animation to complete (adjust the time to match your animation)
   
    // Handle post-attack logic (e.g., applying damage)
    ApplyDamageToEnemy(Character);

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

        activeCharacterBattle = characterBattle;
        activeCharacterBattle.ShowSelectionCircle();
    }


private void LoadCharacterConfig(string path,bool player ) {
    Debug.Log("PATH " + path);
    if (!File.Exists(path)) {
        Debug.LogError("Config file not found: " + path);
        return;
    }
    if(player){
        
        playerConfig = playerConfig.LoadPlayerConfig(path);

        // Optional: Log player config for debugging purposes
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
    Debug.Log("Player is loaded");

}

    public void SetupEnemy(string configPath) {
        enemyConfig = new PlayerConfig();  // Ensure playerConfig is initialized

    // Load the player configuration from the file
    LoadCharacterConfig(configPath,false);

    // Apply the loaded player sprites based on the configuration
    LoadCharacterSprites(false); 
    Debug.Log("Enemy is loaded");
    }

    public void LoadCharacterSprites(bool isPlayer) {
     

    if (isPlayer) {
        // Use PlayerConfig for the player character
                   Debug.Log(" LoadCharacterSprites player ") ;
        string headFile = playerConfig.playerHead.path;
        string bodyFile = playerConfig.playerBody.path;
        string weaponFile = playerConfig.playerWeapon.path;
        string spritepath = playerConfig.spriteSheetPath;
        Debug.Log(" HEAD : " + headFile +" bodyFile : " + bodyFile+ " weaponFile : " + weaponFile+ "Spritsheet at : "+ spritepath ) ;
        // Apply the player-specific textures
        ModifyPlayerBodySpritesheet(bodyFile,true);
        ModifyPlayerWeaponSpritesheet(weaponFile,true);
        ModifyPlayerHeadSpritesheet(headFile,true);
    } else {
                   Debug.Log(" LoadCharacterSprites enemy ") ;
        string headFile = enemyConfig.playerHead.path;
        string bodyFile = enemyConfig.playerBody.path;
        string weaponFile = enemyConfig.playerWeapon.path; 
        string spritepath = enemyConfig.spriteSheetPath;
        Debug.Log(" HEAD : " + headFile +" bodyFile : " + bodyFile+ " weaponFile : " + weaponFile+ "Spritsheet at : "+ spritepath ) ;
        


        ModifyPlayerBodySpritesheet(bodyFile,false);
         Debug.Log(" Enemy body done : ");
        ModifyPlayerWeaponSpritesheet(weaponFile,false);
         Debug.Log(" Enemy weapon done : ");
        ModifyPlayerHeadSpritesheet(headFile,false);
         Debug.Log(" Enemy head done : ");
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
            Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected body image
        Texture2D selectedBodies = LoadTextureFromFile(bodyFile);

        if (selectedBodies == null) {
            Debug.LogError("Failed to load selected bodies.");
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
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
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
            Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected body image
        Texture2D selectedHead = LoadTextureFromFile("Assets/TurnBattleSystem/Textures/head.png");

        if (selectedHead == null) {
            Debug.LogError("Failed to load selected bodies.");
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
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
    }


    public void ModifyPlayerWeaponSpritesheet(string weaponFile,bool player) {
        Debug.Log("Modify playterSpriteshete");
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
            Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected weapon image
        Texture2D selectedWeapons = LoadTextureFromFile(weaponFile);

        if (selectedWeapons == null) {
            Debug.LogError("Failed to load selected weapons.");
            return;
        }

        // Resize the loaded weapon image if necessary
        selectedWeapons = ResizeTexture(selectedWeapons, selectedWeapons.width, selectedWeapons.height);

        // Define coordinates for the wepons (adjust based on your spritesheet)
        Rect weapon1Rect = new Rect(50, 180, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon1Rect);


        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
       // SetupPlayer("configPlayer.txt");
    }
    

public void ModifyPlayerArmorSpritesheet(string armorFile, bool player) {
    Debug.Log("Modify PlayerArmorSpritesheet");

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
        Debug.LogError("Failed to load base spritesheet.");
        return;
    }

    // Load the selected armor image
    Texture2D selectedArmor = LoadTextureFromFile(armorFile);

    if (selectedArmor == null) {
        Debug.LogError("Failed to load selected armor.");
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
    Debug.Log($"Modified spritesheet saved to {outputImagePath}");

    // Optionally, you can trigger player setup here if needed.
    // SetupPlayer("configPlayer.txt");
}

public void ModifyPlayerFeetSpritesheet(string weaponFile, bool player) {
        Debug.Log("Modify playterSpriteshete");
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
            Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected weapon image
        Texture2D selectedWeapons = LoadTextureFromFile(weaponFile);

        if (selectedWeapons == null) {
            Debug.LogError("Failed to load selected weapons.");
            return;
        }

        // Resize the loaded weapon image if necessary
        selectedWeapons = ResizeTexture(selectedWeapons, selectedWeapons.width, selectedWeapons.height);

        // Define coordinates for the wepons (adjust based on your spritesheet)
        Rect weapon1Rect = new Rect(50, 160, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon1Rect);


        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
       // SetupPlayer("configPlayer.txt");
    }


    public void ModifyPlayerLegsSpritesheet(string weaponFile, bool player) {
        Debug.Log("Modify playterSpriteshete");
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
            Debug.LogError("Failed to load base spritesheet.");
            return;
        }

        // Load the selected weapon image
        Texture2D selectedWeapons = LoadTextureFromFile(weaponFile);

        if (selectedWeapons == null) {
            Debug.LogError("Failed to load selected weapons.");
            return;
        }

        // Resize the loaded weapon image if necessary
        selectedWeapons = ResizeTexture(selectedWeapons, selectedWeapons.width, selectedWeapons.height);

        // Define coordinates for the wepons (adjust based on your spritesheet)
        Rect weapon1Rect = new Rect(50, 230, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon1Rect);


        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
       // SetupPlayer("configPlayer.txt");
    }

private Texture2D LoadTextureFromFile(string filePath)
{
    // Check if the filePath is valid
    if (string.IsNullOrEmpty(filePath))
    {
        Debug.LogError("Error: filePath is null or empty.");
        return null;
    }

    // Check if the file exists
    if (!System.IO.File.Exists(filePath))
    {
        Debug.LogError($"Error: File not found at {filePath}");
        return null;
    }

    try
    {
            Debug.Log($"Loading texture from {filePath}");

        byte[] fileData = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        return texture;
    }
    catch (Exception ex)
    {
        Debug.LogError($"Error while loading texture from {filePath}: {ex.Message}");
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

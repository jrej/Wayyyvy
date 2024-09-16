using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System;

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


    private static BattleHandler instance;
    private Dictionary<string, string> config;

    private CharacterCustomizationUI characterUi;

    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    private CharacterBattle playerCharacterBattle;
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    private State state;
    private Text dialogueText;



    private Dialogue preFightDialogue;

    public AudioClip musicClip;  // Assign this in the Inspector
    private AudioSource audioSource;

    private GameObject canvas;
    private GameObject textObj;

    private GameObject playerSpriteObj;
    private GameObject enemySpriteObj;

    GameObject customizationUIObject;

    private GameObject backgroundObj;

    private GameObject quitButtonObj;

    public PlayerConfig playerConfig;
        public PlayerConfig enemyConfig;



    public GameObject inventoryMenu;
        public GameObject SelectionMenu;

    public Button toggleInventoryButton;
        public Button toggleInInventoryButton;

        public Button Enemy1;
        public Button Enemy2;
        public Button Enemy3;
        public Button Enemy4;

    public Button quitButton;
        public GameObject buttonCanvas;


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
    //inventoryMenu.SetActive(false);
    SelectionMenu.SetActive(true);


   ////// dialogueText = canvas.GetComponentInChildren<Text>();

  //  if (dialogueText == null) {
   //     Debug.LogError("DialogueText component not found under Canvas2.");
    ///    return;
   // }

    // Create enemy selection UI
  //  CreateEnemySelectionUI();





        /*instance = this;

        SetupPlayer("configPlayer.txt");
        SetupEnemy("configPlayer.txt");

        customizationUIObject = new GameObject("CharacterCustomizationUI");
        customizationUIObject.AddComponent<CharacterCustomizationUI>();

        canvas = GameObject.Find("Canvas2");

        dialogueText = canvas.GetComponentInChildren<Text>();

        if (dialogueText == null) {
            Debug.LogError("DialogueText component not found under Canvas2.");
            return;
        }

        backgroundObj = new GameObject("TextBackground");
        backgroundObj.transform.SetParent(canvas.transform);

        Image background = backgroundObj.AddComponent<Image>();
        Color fadedBlack = new Color(0, 0, 0, 0.5f);
        background.color = fadedBlack;

        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        backgroundRect.sizeDelta = new Vector2(300, 100);
        backgroundRect.anchoredPosition = new Vector2(0, 0);
        backgroundRect.anchorMin = new Vector2(0.5f, 0.5f);
        backgroundRect.anchorMax = new Vector2(0.5f, 0.5f);
        backgroundRect.pivot = new Vector2(0.5f, 0.5f);

        dialogueText.transform.SetParent(backgroundObj.transform);
        RectTransform textRect = dialogueText.GetComponent<RectTransform>();
        textRect.sizeDelta = backgroundRect.sizeDelta;
        textRect.anchoredPosition = new Vector2(0, -50);

        dialogueText.fontSize = 8;
        dialogueText.color = Color.white;
        dialogueText.alignment = TextAnchor.MiddleCenter;
        dialogueText.canvas.sortingOrder = 1;

        dialogueText.text = "Hello, world!";

        preFightDialogue = new Dialogue {
            lines = new List<string> {
                "Player: So, we finally meet again.",
                "Enemy: This time, you won't get away!",
                "Player: We'll see about that.",
                "Prepare yourself!"
            }
        };

        LoadAndDisplayCharacterSprites();


        // Get or add the AudioSource component to this GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        
        // Assign the music clip
        audioSource.clip = musicClip;
        
        // Set additional AudioSource properties if needed
        audioSource.loop = true;  // Loop the music
        audioSource.volume = 0.5f;  // Set volume (0.0 to 1.0)

        // Play the music
        //
        audioSource.Play();
        */
                

        
    }



private void LoadEnemyAndStartBattle(int enemyIndex) {
    SelectionMenu.SetActive(false);
    buttonCanvas.SetActive(true);
    string enemyConfigPath = $"configEnemy{enemyIndex}.txt";
    SetupEnemy(enemyConfigPath); // Load the selected enemy config file
    StartNewGame(); // Start the battle with the selected enemy
}





           //    string weaponFolderPath = config["PlayerWeapon"]; // Path within Resources folder
    private void CreateWeaponButtonArray() {
    // Create a parent GameObject to hold the buttons
    GameObject buttonArrayParent = new GameObject("WeaponButtonArray");
    buttonArrayParent.transform.SetParent(backgroundObj.transform);

    // Set the parent RectTransform to match the backgroundObj
    RectTransform backgroundRect = backgroundObj.GetComponent<RectTransform>();
    RectTransform buttonArrayRect = buttonArrayParent.AddComponent<RectTransform>();
    buttonArrayRect.sizeDelta = backgroundRect.sizeDelta;

    // Manually adjust the position of buttonArrayParent
    Vector2 positionOffset = new Vector2(-100, -100); // Adjust these values as needed
    buttonArrayRect.anchoredPosition = backgroundRect.anchoredPosition + positionOffset;

    // Create a dark background box for the button array
    GameObject backgroundBox = new GameObject("WeaponButtonBox");
    backgroundBox.transform.SetParent(buttonArrayParent.transform);
    RectTransform backgroundBoxRect = backgroundBox.AddComponent<RectTransform>();
    backgroundBoxRect.sizeDelta = new Vector2(60, 200); // Match size to the buttonArrayRect
    backgroundBoxRect.anchoredPosition = Vector2.zero;

    // Add Image component to background box for visualization (optional)
    Image backgroundImage = backgroundBox.AddComponent<Image>();
    backgroundImage.color = Color.black; // Set to desired color

    // Create a GridLayoutGroup to organize buttons in a grid
    GridLayoutGroup gridLayout = buttonArrayParent.AddComponent<GridLayoutGroup>();
    gridLayout.cellSize = new Vector2(50, 50); // Increase the size for better visibility
    gridLayout.spacing = new Vector2(5, 5); // Adjust spacing as necessary
    gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    gridLayout.constraintCount = 6;

    // Load textures from folder
    string folderPath = "Assets/TurnBattleSystem/Textures/Weapons2";
    Texture2D[] weaponTextures = LoadTexturesFromFolder(folderPath);

    // Create buttons for up to 4 textures
    int buttonCount = Mathf.Min(4, weaponTextures.Length);
    for (int i = 0; i < buttonCount; i++) {
        Texture2D texture = weaponTextures[i];

        // Create a new button
        GameObject buttonObj = new GameObject("WeaponButton_" + i);
        buttonObj.transform.SetParent(buttonArrayParent.transform);

        // Set up RectTransform for the button
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(50, 50); // Match cellSize from GridLayoutGroup
        buttonRect.anchoredPosition = Vector2.zero;

        // Add Button component
        Button button = buttonObj.AddComponent<Button>();

        // Add Image component and set sprite
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        buttonImage.raycastTarget = true;

        // Add button functionality
        int index = i; // Capture the current index for the button
        button.onClick.AddListener(() => {
            Debug.Log("Weapon Button " + index + " clicked");
            OnWeaponButtonClick(index);
        });

        Debug.Log("Created button: " + buttonObj.name + " at position " + buttonRect.anchoredPosition + " with size " + buttonRect.sizeDelta);
    }

    Debug.Log("Weapon buttons created");
}




private GameObject CreateBackgroundImage(GameObject parent, Texture2D texture, Color color) {
    GameObject backgroundObj = new GameObject("WeaponSelectionBackground");
    backgroundObj.transform.SetParent(parent.transform);

    Image image = backgroundObj.AddComponent<Image>();
    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    image.color = color;

    RectTransform rectTransform = image.GetComponent<RectTransform>();
    rectTransform.sizeDelta = new Vector2(500, 300); // Adjust size as needed
    rectTransform.anchoredPosition = Vector2.zero;
    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    rectTransform.pivot = new Vector2(0.5f, 0.5f);

    return backgroundObj;
}

private void CreateBodyButtonArray() {
    // Create a parent GameObject to hold the buttons
    GameObject buttonArrayParent = new GameObject("BodyButtonArray");
    buttonArrayParent.transform.SetParent(backgroundObj.transform);

    // Set the parent RectTransform to match the backgroundObj
    RectTransform backgroundRect = backgroundObj.GetComponent<RectTransform>();
    RectTransform buttonArrayRect = buttonArrayParent.AddComponent<RectTransform>();
    buttonArrayRect.sizeDelta = backgroundRect.sizeDelta;
    
    // Manually adjust the position of buttonArrayParent to be below the weapon buttons
    Vector2 positionOffset = new Vector2(-75, -130); // Adjust these values as needed
    buttonArrayRect.anchoredPosition = backgroundRect.anchoredPosition + positionOffset;

    // Create a GridLayoutGroup to organize buttons in a grid
    GridLayoutGroup gridLayout = buttonArrayParent.AddComponent<GridLayoutGroup>();
    gridLayout.cellSize = new Vector2(25, 25); // Adjust size as necessary
    gridLayout.spacing = new Vector2(5, 5); // Adjust spacing as necessary
    gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    gridLayout.constraintCount = 6;

    // Load textures from folder
    string folderPath = "Assets/TurnBattleSystem/Textures/Bodies2";
    Texture2D[] bodyTextures = LoadTexturesFromFolder(folderPath);

    // Create buttons for up to 6 textures
    int buttonCount = Mathf.Min(6, bodyTextures.Length);
    for (int i = 0; i < buttonCount; i++) {
        Texture2D texture = bodyTextures[i];
        
        // Create a new button
        GameObject buttonObj = new GameObject("BodyButton_" + i);
        buttonObj.transform.SetParent(buttonArrayParent.transform);

        // Set up RectTransform for the button
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(25, 25); // Keep button size consistent

        // Add Button component
        Button button = buttonObj.AddComponent<Button>();

        // Add Image component and set sprite
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Add button functionality
        int index = i; // Capture the current index for the button
        button.onClick.AddListener(() => {
            Debug.Log("Body Button " + index + " clicked");
            OnWeaponBodyClick(index);
        });
    }
}




private void OnWeaponButtonClick(int index) {
    Debug.Log("Weapon button " + index + " clicked");

    // Get the path of the selected weapon texture
    string folderPath = "Assets/TurnBattleSystem/Textures/Weapons2";
    string[] weaponFiles = System.IO.Directory.GetFiles(folderPath, "*.png");
    if (index >= 0 && index < weaponFiles.Length) {
        string selectedWeaponPath = weaponFiles[index];

        // Update config with the selected weapon
        UpdateConfigWithSelectedWeapon(selectedWeaponPath);
    }

    Debug.Log("Weapon button " + index + " clicked");

    // Set the flag to true indicating the user has made a selection
    MadeSelection = true;
}


private void OnWeaponBodyClick(int index) {
    Debug.Log("Weapon button " + index + " clicked");

    // Get the path of the selected weapon texture
    string folderPath = "Assets/TurnBattleSystem/Textures/Bodies2";
    string[] weaponFiles = System.IO.Directory.GetFiles(folderPath, "*.png");
    if (index >= 0 && index < weaponFiles.Length) {
        string selectedWeaponPath = weaponFiles[index];
        
        // Update config with the selected weapon
        // Assuming you have a method to update your config file
        UpdateConfigWithSelectedWeapon(selectedWeaponPath);
    }

    Debug.Log("Weapon button " + index + " clicked");

    // Set the flag to true indicating the user has made a selection
    MadeSelection = true ;
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

private bool MadeSelection = false ;
private bool UserHasMadeSelection() {
    // Implement logic to determine if the user has made a selection
    // This could involve checking a flag or condition that gets updated when the user selects a weapon or body part
    return MadeSelection; // Placeholder, replace with actual implementation
}

// Update the StartBattleWithDialogue method to remove sprites
private IEnumerator StartBattleWithDialogue() {
    if (dialogueText == null) {
        Debug.LogError("DialogueText component not found under Canvas2.");
        yield break;
    }
   // ShowWeaponSelectionUI();

    // Wait for the user to make their selection
   //yield return new WaitUntil(() => UserHasMadeSelection());

   // yield return StartCoroutine(ShowDialogue(preFightDialogue));

    // Deactivate the background and remove sprites after the dialogue is complete
    backgroundObj.SetActive(false);
    Destroy(GameObject.Find("PlayerSprite"));
    Destroy(GameObject.Find("EnemySprite"));

    StartNewGame();
}

    private IEnumerator ShowDialogue(Dialogue dialogue) {
        foreach (string line in dialogue.lines) {
            yield return StartCoroutine(TypeSentence(line));
            yield return new WaitForSeconds(1f); // Wait for a second before showing the next line
        }
        dialogueText.text = ""; // Clear the dialogue after displaying
    }

    private IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust the typing speed as necessary
        }
    }

    public void TestBattleStart() {
        // Use BattleOverWindow to display the dialogue text
        string dialogueText = string.Join("\n", preFightDialogue.lines);
        //BattleOverWindow.Show_Static(dialogueText);

        // Wait for a short time to simulate displaying the dialogue
        StartCoroutine(WaitAndStartGame(5f)); // Wait for 5 seconds before starting the game
    }


    private IEnumerator WaitAndStartGame(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        //BattleOverWindow.Hide_Static();
        StartNewGame();
    }
private Image playerSpriteGearImage;
private Image Enemy1Image;
private Image Enemy2Image;
private Image Enemy3Image;

private Image Enemy4Image;
   /// <summary>
    private string PlayerSpritePath = "Assets/TurnBattleSystem/Textures/PlayerSprite.png";
        private string EnemySpritePath = "Assets/TurnBattleSystem/Textures/EnemySprite.png";

   /// </summary>



    private void Start() {

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
    private void ToggleInventoryMenu() {
        Debug.Log("Toglle inventory "+ inventoryMenu.activeSelf);
        if (inventoryMenu != null) {
            bool isActive = inventoryMenu.activeSelf;
            inventoryMenu.SetActive(!isActive);
        }
    }

   private void QuitGame() {
    Debug.Log("Quitting game...");
    Destroy(playerCharacterBattle.gameObject);
    Destroy(enemyCharacterBattle.gameObject);
    SelectionMenu.SetActive(true);
    buttonCanvas.SetActive(false);

   /* // If we are running on Android
    #if UNITY_ANDROID
        // Close the application on Android
        Application.Quit();
    #else
        // For any other platform (like in the Unity Editor or Desktop)
        Application.Quit();
    #endif*/
}



    private void StartNewGame() {

        // Update spritesheets
        SetupPlayer("configPlayer.txt");
       //Enemy should be loaded with LoadEnemyAndStartBattle by choosing on button
       // SetupEnemy("configPlayer.txt");

        // Spawn player and enemy characters
        playerCharacterBattle = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);

        // Set player character as active and wait for player's action
        SetActiveCharacterBattle(playerCharacterBattle);
        state = State.WaitingForPlayer;
    }

private void Update() {
    if (state == State.WaitingForPlayer) {
        // For touch input (Android)
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is a "Tap"
            if (touch.phase == TouchPhase.Ended) {
                state = State.Busy;
                playerCharacterBattle.Attack(enemyCharacterBattle, () => {
                    ChooseNextActiveCharacter();
                });
            }
        }

        // For spacebar input (Desktop testing)
        if (Input.GetKeyDown(KeyCode.Space)) {
            state = State.Busy;
            playerCharacterBattle.Attack(enemyCharacterBattle, () => {
                ChooseNextActiveCharacter();
            });
        }
    }
}


    private CharacterBattle SpawnCharacter(bool isPlayerTeam) {
        Vector3 position = isPlayerTeam ? new Vector3(-50, -20) : new Vector3(+50, -20);
        Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerTeam);

        return characterBattle;
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle) {
        if (activeCharacterBattle != null) {
            activeCharacterBattle.HideSelectionCircle();
        }

        activeCharacterBattle = characterBattle;
        activeCharacterBattle.ShowSelectionCircle();
    }

    private void ChooseNextActiveCharacter() {
        if (TestBattleOver()) {
            return;
        }

        if (activeCharacterBattle == playerCharacterBattle) {
            SetActiveCharacterBattle(enemyCharacterBattle);
            state = State.Busy;

            enemyCharacterBattle.Attack(playerCharacterBattle, () => {
                ChooseNextActiveCharacter();
            });
        } else {
            SetActiveCharacterBattle(playerCharacterBattle);
            state = State.WaitingForPlayer;
        }
    }

    private void RestartGame() {
        // Destroy current characters
        Destroy(playerCharacterBattle.gameObject);
        Destroy(enemyCharacterBattle.gameObject);

        // Reset and start a new game
        StartNewGame();
    }

    private bool TestBattleOver() {
        if (playerCharacterBattle.IsDead()) {
         //   BattleOverWindow.Show_Static("Enemy Wins!");
            RestartGame();
            return true;
        }
        if (enemyCharacterBattle.IsDead()) {
         //   BattleOverWindow.Show_Static("Player Wins!");
            RestartGame();
            return true;
        }

        return false;
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
        string headFile = playerConfig.playerHead.path;
        string bodyFile = playerConfig.playerBody.path;
        string weaponFile = playerConfig.playerWeapon.path;
       // Debug.Log(" HEAD : " + headFile +" bodyFile : " + bodyFile+ " weaponFile : " + weaponFile ) ;
        // Apply the player-specific textures
        ModifyPlayerBodySpritesheet(bodyFile,true);
        ModifyPlayerWeaponSpritesheet(weaponFile,true);
        ModifyPlayerHeadSpritesheet(headFile,true);
    } else {
        string headFile = enemyConfig.playerHead.path;
        string bodyFile = enemyConfig.playerBody.path;
        string weaponFile = enemyConfig.playerWeapon.path;


        ModifyPlayerBodySpritesheet(bodyFile,true);
        ModifyPlayerWeaponSpritesheet(weaponFile,true);
        ModifyPlayerHeadSpritesheet(headFile,true);
    }
}

    string[] nameSlots = {};

    private void ModifyPlayerBodySpritesheet(string bodyFile,bool player ) {
        //Setup as enemy for default

        string baseImagePath = "Assets/TurnBattleSystem/Textures/EnemySpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/EnemySpritesheet.png";

        if (player){
            // Paths to files and folders
         baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
         outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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
        string baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

        if (player){
            // Paths to files and folders
         baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
         outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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

    /*private void ModifyEnemyBodySpritesheet(string bodyFile) {
        // Paths to files and folders
        string baseImagePath = "Assets/TurnBattleSystem/Textures/EnemySpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/EnemySpritesheet.png";

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
        Rect body1Rect = new Rect(25, 300, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body1Rect);

        Rect body2Rect = new Rect(180, 300, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body2Rect);

        Rect body3Rect = new Rect(300, 300, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body3Rect);

        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
    }
    */

    public void ModifyPlayerWeaponSpritesheet(string weaponFile,bool player) {
        Debug.Log("Modify playterSpriteshete");
        // Paths to files and folders
        string baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

        if (player){
            // Paths to files and folders
         baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
         outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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
    string baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
    string outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

    if (player){
            // Paths to files and folders
         baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
         outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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
        // Paths to files and folders
        string baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

        if (player){
            // Paths to files and folders
         baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
         outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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
        string baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

        if (player){
            // Paths to files and folders
         baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
         outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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
/*
    private void ModifyEnemyWeaponSpritesheet(string weaponFile) {
        // Paths to files and folders
        string baseImagePath = "Assets/TurnBattleSystem/Textures/EnemySpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/EnemySpritesheet.png";

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

        // Define coordinates for the weapons (adjust based on your spritesheet)
        Rect weapon1Rect = new Rect(50, 50, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon1Rect);

        Rect weapon2Rect = new Rect(180, 50, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon2Rect);

        Rect weapon3Rect = new Rect(300, 50, selectedWeapons.width, selectedWeapons.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedWeapons, weapon3Rect);

        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
    }
    */
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

    

    private Texture2D[] LoadTexturesFromFolder(string folderPath) {
    string[] files = System.IO.Directory.GetFiles(folderPath, "*.png");
    List<Texture2D> textures = new List<Texture2D>();
    foreach (string file in files) {
        byte[] fileData = System.IO.File.ReadAllBytes(file);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // This will auto-resize the texture dimensions.
        textures.Add(texture);
    }
    return textures.ToArray();
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

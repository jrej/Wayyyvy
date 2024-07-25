﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;


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

    private GameObject canvas;
    private GameObject textObj;

    private GameObject playerSpriteObj;
    private GameObject enemySpriteObj;

    GameObject customizationUIObject;

    private GameObject backgroundObj;

    private GameObject quitButtonObj;

    public Button quitButton;

    public static BattleHandler GetInstance() {
        return instance;
    }

    private enum State {
        WaitingForPlayer,
        Busy,
    }

    private void Awake() {
        instance = this;

        SetupPlayer("config.txt");
        SetupEnemy("config.txt");

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
                

        
    }
    private void     ShowWeaponSelectionUI(){
        CreateWeaponButtonArray(); // Call the method to create the button array
                CreateBodyButtonArray();
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

private void UpdateConfigWithSelectedWeapon(string weaponPath) {
    // Example implementation for updating config
    // This should be replaced with your actual config update logic
    Debug.Log("Updating config with weapon: " + weaponPath);
    // Example code to update the config file
     config["PlayerWeapon"] = weaponPath;
     
}



 private void QuitGame() {
        Debug.Log("Quitting the game...");
        
            Application.Quit();
        
    }

private void LoadAndDisplayCharacterSprites() {
    // Load player sprite
    string playerSpritePath = config["PlayerHead"]; // Assuming the config has the paths
    Texture2D playerTexture = LoadTextureFromFile(playerSpritePath);
    GameObject playerSpriteObj = new GameObject("PlayerSprite");
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

   

    // Load enemy sprite
    string enemySpritePath = config["EnemyHead"]; // Assuming the config has the paths
    Texture2D enemyTexture = LoadTextureFromFile(enemySpritePath);
    GameObject enemySpriteObj = new GameObject("EnemySprite");
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

    yield return StartCoroutine(ShowDialogue(preFightDialogue));

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
        BattleOverWindow.Show_Static(dialogueText);

        // Wait for a short time to simulate displaying the dialogue
        StartCoroutine(WaitAndStartGame(5f)); // Wait for 5 seconds before starting the game
    }


    private IEnumerator WaitAndStartGame(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        BattleOverWindow.Hide_Static();
        StartNewGame();
    }



    private void Start() {
// Initialize Character Customization UI
        
        StartCoroutine(StartBattleWithDialogue());
    }

    private void StartNewGame() {
        // Update spritesheets
        SetupPlayer("config.txt");
        SetupEnemy("config.txt");

        // Spawn player and enemy characters
        playerCharacterBattle = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);

        // Set player character as active and wait for player's action
        SetActiveCharacterBattle(playerCharacterBattle);
        state = State.WaitingForPlayer;
    }

    private void Update() {
        if (state == State.WaitingForPlayer) {
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
            BattleOverWindow.Show_Static("Enemy Wins!");
            RestartGame();
            return true;
        }
        if (enemyCharacterBattle.IsDead()) {
            BattleOverWindow.Show_Static("Player Wins!");
            RestartGame();
            return true;
        }

        return false;
    }

    private void LoadCharacterConfig(string path) {
        config = new Dictionary<string, string>();
        using (StreamReader reader = new StreamReader(path)) {
            string line;
            while ((line = reader.ReadLine()) != null) {
                string[] keyValue = line.Split('=');
                if (keyValue.Length == 2) {
                    config[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }
        }
    }

    public void SetupPlayer(string configPath) {
        LoadCharacterConfig(configPath);
        LoadCharacterSprites(true);
    }

    public void SetupEnemy(string configPath) {
        LoadCharacterConfig(configPath);
        LoadCharacterSprites(false);
    }

    private void LoadCharacterSprites(bool isPlayer) {
        string headFile = isPlayer ? config["PlayerHead"] : config["EnemyHead"];
        string bodyFile = isPlayer ? config["PlayerBody"] : config["EnemyBody"];
        string weaponFile = isPlayer ? config["PlayerWeapon"] : config["EnemyWeapon"];

        if (isPlayer) {
            ModifyPlayerBodySpritesheet(bodyFile);
            ModifyPlayerWeaponSpritesheet(weaponFile);
        } else {
            ModifyEnemyBodySpritesheet(bodyFile);
            ModifyEnemyWeaponSpritesheet(weaponFile);
        }
    }

    private void ModifyPlayerBodySpritesheet(string bodyFile) {
        // Paths to files and folders
        string baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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
        Rect body1Rect = new Rect(50, 260, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body1Rect);

        Rect body2Rect = new Rect(180, 260, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body2Rect);

        Rect body3Rect = new Rect(300, 260, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body3Rect);

        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
    }

    private void ModifyEnemyBodySpritesheet(string bodyFile) {
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
        Rect body1Rect = new Rect(50, 260, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body1Rect);

        Rect body2Rect = new Rect(180, 260, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body2Rect);

        Rect body3Rect = new Rect(300, 260, selectedBodies.width, selectedBodies.height); // Adjusted coordinates and size based on the image
        ReplaceTextureSection(baseSpritesheet, selectedBodies, body3Rect);

        // Save the modified spritesheet
        SaveTextureToFile(baseSpritesheet, outputImagePath);
        Debug.Log($"Modified spritesheet saved to {outputImagePath}");
    }

    private void ModifyPlayerWeaponSpritesheet(string weaponFile) {
        // Paths to files and folders
        string baseImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";
        string outputImagePath = "Assets/TurnBattleSystem/Textures/PlayerSpritesheet.png";

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

    private Texture2D LoadTextureFromFile(string filePath) {
        byte[] fileData = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        return texture;
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

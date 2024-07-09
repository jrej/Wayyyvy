using UnityEngine;
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

    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    private CharacterBattle playerCharacterBattle;
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    private State state;
    private Text dialogueText;

    private Dialogue preFightDialogue;

     // Create a new Text object and add it to the canvas
    private   GameObject canvas ;
     private  GameObject textObj ;

    public static BattleHandler GetInstance() {
        return instance;
    }
     private enum State {
        WaitingForPlayer,
        Busy,
    }

     private void Awake() {
        instance = this;
        
        canvas = GameObject.Find("Canvas2");
        dialogueText = canvas.GetComponentInChildren<Text>();

    if (dialogueText == null) {
        Debug.LogError("DialogueText component not found under Canvas2.");
        return;
    }

       // textObj.transform.SetParent(canvas.transform);
           /// textObj.transform.SetParent(canvas.transform);
       //dialogueText = textObj.AddComponent<Text>();
        dialogueText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        dialogueText.fontSize = 12;
        dialogueText.color = Color.red;
        dialogueText.rectTransform.sizeDelta = new Vector2(40, 10);
        dialogueText.rectTransform.anchoredPosition = new Vector2(0, -10);
        dialogueText.canvas.sortingOrder = 1; // Adjust as necessary
        dialogueText.alignment = TextAnchor.MiddleCenter; // Example: Aligns the text to the center

    // Example of changing text content
    dialogueText.text = "Hello, world!";

         // Create a fake scenario for the pre-fight dialogue
        preFightDialogue = new Dialogue {
            lines = new List<string> {
                "Player: So, we finally meet again.",
                "Enemy: This time, you won't get away!", 
                "Player: We'll see about that. Prepare yourself!"
            }
        };
    }

    private IEnumerator StartBattleWithDialogue() {
        if (dialogueText  == null) {
            Debug.LogError("DialogueUI is not assigned in the BattleHandler script.");
           yield break;
        }

        yield return StartCoroutine(ShowDialogue(preFightDialogue));
        StartNewGame();
    }

    private IEnumerator ShowDialogue(Dialogue dialogue) {
        foreach (string line in dialogue.lines) {
            dialogueText.text = line;
            yield return new WaitForSeconds(2f); // Example wait time
        }
        dialogueText.text = ""; // Clear the dialogue after displaying
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

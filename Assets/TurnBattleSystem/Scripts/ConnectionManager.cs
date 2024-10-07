using UnityEngine;
using System.IO;
using System.Collections.Generic;

using Assets.HeroEditor.Common.Scripts.EditorScripts;

using Assets.HeroEditor.Common.Scripts.CharacterScripts;
//using UnityEditor.Build.Reporting;

public class ConnectionManager : MonoBehaviour
{
   // public string databaseFilePath = "Assets/Database.json"; // Path to the player info JSON database

    #if UNITY_EDITOR
public string databaseFilePath = "Assets/Database.json";  // In the editor
#else
public string databaseFilePath = Path.Combine(Application.persistentDataPath, "Database.json");  // In builds
#endif

#if UNITY_EDITOR
public string basePath = "Assets/Account/"; // For the Unity Editor
#else
public string basePath = Application.persistentDataPath + "/Account/"; // For runtime builds
#endif


    public string emailInput;
    public string passwordInput;
    
    public CharacterEditor characterEditor = new CharacterEditor(); // Reference to your CharacterEditor
    public Character character; // Reference to your CharacterEditor    public List<Character> loadedcharacter = new List<Character>(); // Initialize the loadedcharacter list

    // Class to hold player info
    [System.Serializable]
    public class PlayerData
    {
        public string email;
        public string password;
        public List<string> characterJsonPaths = new List<string>(); // List of JSON file paths for each character

        // Method to get character JSON paths
        public List<string> GetCharacterJsonPaths() { return characterJsonPaths; }
    }

    // Wrapper class for JSON serialization of player data
    [System.Serializable]
    public class PlayerDatabase
    {
        public List<PlayerData> players = new List<PlayerData>(); // List of PlayerData
    }

    public PlayerDatabase playerDatabase = new PlayerDatabase(); // Store all player data
    public PlayerData PlayerDatadata; // Store the current player

    void Start()
    {
        LoadPlayerDatabase();
        
        
    }

    public ConnectionManager()
    {
        // Initialize components here
        characterEditor = new CharacterEditor();
        playerDatabase = new PlayerDatabase();  // or load from file if needed
        PlayerDatadata = null;
    }

   public void Initialize()
{
    // Ensure the characterEditor is properly initialized before usage
    if (characterEditor == null)
    {
        characterEditor = FindObjectOfType<CharacterEditor>();
        if (characterEditor == null)
        {
            Debug.LogError("CharacterEditor not found in the scene.");
            return;
        }
    }
    playerDatabase = new PlayerDatabase();  // or load from file if needed
    PlayerDatadata = null;
}

public void Connect(Character human)
{
    Debug.LogError("New Connection");
    Initialize();
    string email = emailInput; // Get the email input
    string password = passwordInput; // Get the password input

    // Validate the email and password
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        Debug.LogError("Invalid email or password. Cannot create account.");
        return;
    }

    // Load player database
    LoadPlayerDatabase();

    // Check if email/password combination exists in the player database
    PlayerDatadata = playerDatabase.players.Find(player => player.email == email && player.password == password);

    if (PlayerDatadata != null) 
    {
        // Access the user's character folder
        string directoryPath = Path.Combine(basePath, PlayerDatadata.email, "Characters");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath); // Ensure the directory exists
        }
        LoadCharacters(directoryPath,human);
    }
    else
    {
        // If credentials do not match, create a new account
        Debug.LogError("Invalid login credentials. Creating a new profile.");
        CreateNewAccount(email, password); 
    }
}





    // Save the updated character in the player database
    public void UpdatePlayerCharacter(string email, Character updatedCharacter, string characterName)
    {
        Debug.Log($"Saving character '{characterName}' for player: {email}");

        PlayerData player = playerDatabase.players.Find(p => p.email == email);
        if (player != null)
        {
            string characterFilePath = Path.Combine("Assets/Account", email, "Characters", characterName + ".json");
            
            // Save the character data to the JSON file
            characterEditor.Character = updatedCharacter;
            characterEditor.SaveToJson(characterFilePath, characterName);

            // Add or update the character path in the player's data
            bool characterFound = false;
            for (int i = 0; i < player.characterJsonPaths.Count; i++)
            {
                if (Path.GetFileNameWithoutExtension(player.characterJsonPaths[i]) == characterName)
                {
                    player.characterJsonPaths[i] = characterFilePath;
                    characterFound = true;
                    break;
                }
            }

            if (!characterFound)
            {
                player.characterJsonPaths.Add(characterFilePath);
            }

            // Save the updated player database
            SavePlayerDatabase();
            Debug.Log("Character saved successfully.");
        }
        else
        {
            Debug.LogError("Player not found with email: " + email);
        }
    }

    // Retrieve the character path for a specific character from the database
    public string GetCharacterPath(string email, string characterName)
    {
        PlayerData player = playerDatabase.players.Find(p => p.email == email);
        if (player != null)
        {
            foreach (string characterPath in player.characterJsonPaths)
            {
                if (Path.GetFileNameWithoutExtension(characterPath) == characterName)
                {
                    return characterPath;
                }
            }
        }

        Debug.LogError("Character path not found for: " + characterName);
        return null;
    }

    public void SavePlayerDatabase()
    {
        string json = JsonUtility.ToJson(playerDatabase, true);
        File.WriteAllText("Assets/Database.json", json);
        Debug.Log("Player database saved.");
    }




    // Method to create a new account with the given email and password
    private void CreateNewAccount(string email, string password)
    {
        PlayerData newPlayer = new PlayerData
        {
            email = email,
            password = password,
            characterJsonPaths = new List<string>() // Initialize empty character list
        };

        // Add the new player to the players list
        playerDatabase.players.Add(newPlayer);
        PlayerDatadata = newPlayer; // Set the current player to the new account

        // Save the updated player database
        SavePlayerDatabase();
        Debug.Log("New account created for: " + email);
    }

    public void LoadPlayerDatabase()
    {
        if (File.Exists(databaseFilePath))
        {
            string json = File.ReadAllText(databaseFilePath);
            Debug.Log("json database "+  json);
            playerDatabase = JsonUtility.FromJson<PlayerDatabase>(json); // Deserialize using the wrapper class
        }
        else
        {
            Debug.LogError("Player database not found.");
        }
    }

 private void LoadCharacters(string directoryPath,Character human)
{
    if (PlayerDatadata == null || PlayerDatadata.characterJsonPaths == null)
    {
        Debug.LogError("PlayerDatadata or characterJsonPaths is null. Cannot load characters.");
        return;
    }

    if (characterEditor == null)
    {
        Debug.LogError("CharacterEditor instance is null.");
        characterEditor = FindObjectOfType<CharacterEditor>();  // Attempt to find the CharacterEditor in the scene
        if (characterEditor == null)
        {
            Debug.LogError("Failed to find CharacterEditor in the scene.");
            return;
        }
    }

    if (characterEditor.Character == null)
    {
//        Debug.LogError("Character instance is null.");
        characterEditor.Character = human;  // Attempt to find a Character in the scene
        if (characterEditor.Character == null)
        {
            Debug.LogError("Failed to find Character in the scene.");
            return;
        }
    }

    foreach (var characterJsonPath in PlayerDatadata.characterJsonPaths)
    {
        string fullCharacterPath = Path.Combine(directoryPath, Path.GetFileName(characterJsonPath));
        if (File.Exists(fullCharacterPath))
        {
            characterEditor.LoadFromJson(fullCharacterPath);
            character = (Character)characterEditor.Character;  // Update the character reference
        }
        else
        {
            Debug.LogError("Character file not found: " + fullCharacterPath);
        }
    }
}






    // Method to create a new character
public void CreateCharacter(string characterName, Character[] characters)
{
    string directoryPath = Path.Combine(basePath, PlayerDatadata.email, "Characters"); // Folder for each player's characters
    
    if (!Directory.Exists(directoryPath))
    {
        Directory.CreateDirectory(directoryPath); // Ensure the directory exists
    }

    string path = Path.Combine(directoryPath, characterName + ".json"); // Create full path for the character JSON file

    foreach (Character character in characters)
    {
        if (character.gameObject.name == "Human")
        {
            characterEditor.Character = character;
        }
    }

    // Save the character JSON using CharacterEditor
    characterEditor.SaveToJson(path, characterName);

    // Add the new character's path to the player's list of characters
    PlayerDatadata.characterJsonPaths.Add(path);

    // Save the updated player data
    SavePlayerDatabase();

    Connect((Character)characterEditor.Character);
}



}


using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CharacterCustomizationUI : MonoBehaviour
{
    public Canvas canvas;
    private GameObject mainPanel;
    private GameObject weaponsPanel;
    private GameObject statsPanel;
    private GameObject characterImage;
    private GameObject healthText;
    private GameObject arenaButton;

    private string[] availableWeapons = {
        "Icon28_01.png", "Icon28_02.png", "Icon28_03.png" // Add the names of your weapon sprites here
    };

    void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        // Create Canvas if not already assigned
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // Create Main Panel
        mainPanel = CreatePanel("MainPanel", new Vector2(0, 0), new Vector2(800, 600));

        // Weapons and Bonuses Panel
        weaponsPanel = CreatePanel("WeaponsPanel", new Vector2(-200, 0), new Vector2(400, 400), mainPanel.transform);

        // Grid Layout for Weapons
        GridLayoutGroup gridLayout = weaponsPanel.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(50, 50);

        // Load and setup weapon icons
        LoadWeaponIcons();

        // Character Stats Panel
        statsPanel = CreatePanel("StatsPanel", new Vector2(200, 0), new Vector2(400, 400), mainPanel.transform);

        // Character Image
        characterImage = CreateImage("CharacterImage", new Vector2(0, 0), new Vector2(100, 200), statsPanel.transform);
        string characterResourcePath = "player_head.png";
        characterImage.GetComponent<Image>().sprite = LoadSpriteFromPath(characterResourcePath);

        // Health Points
        healthText = CreateText("HealthText", new Vector2(0, -100), "69 points de vie", statsPanel.transform);

        // Add Stats Texts
        string[] stats = { "Force : 4", "Agilité : 2", "Rapidité : 2" };
        foreach (var stat in stats)
        {
            CreateText("StatText", Vector2.zero, stat, statsPanel.transform);
        }

        // Arena Button
        arenaButton = CreateButton("ArenaButton", new Vector2(0, -200), "ARENE", mainPanel.transform);
        arenaButton.GetComponent<Button>().onClick.AddListener(OnArenaButtonClicked);
    }

    void LoadWeaponIcons()
    {
        foreach (var weaponName in availableWeapons)
        {
            GameObject weaponIcon = CreateImage("WeaponIcon", Vector2.zero, new Vector2(50, 50), weaponsPanel.transform);
            string weaponResourcePath = "Assets/TurnBattleSystem/Textures/Weapons/" + weaponName;
            weaponIcon.GetComponent<Image>().sprite = LoadSpriteFromPath(weaponResourcePath);
            // Add click event to select weapon 
           // weaponIcon.GetComponent<Button>().onClick.AddListener(() => OnWeaponSelected(weaponName));
        }
    }

    void OnWeaponSelected(string weaponName)
    {
        // Handle weapon selection
        Debug.Log("Selected weapon: " + weaponName);
    }

    void OnArenaButtonClicked()
    {
        // Handle arena button click
        Debug.Log("Arena button clicked");
    }

    private GameObject CreatePanel(string name, Vector2 anchoredPosition, Vector2 sizeDelta, Transform parent = null)
    {
        GameObject panel = new GameObject(name);
        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.SetParent(parent, false);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
        panel.AddComponent<Image>(); // Add an image component for visual representation
        return panel;
    }

    private GameObject CreateImage(string name, Vector2 anchoredPosition, Vector2 sizeDelta, Transform parent = null)
    {
        GameObject imageGO = new GameObject(name);
        RectTransform rectTransform = imageGO.AddComponent<RectTransform>();
        rectTransform.SetParent(parent, false);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
        imageGO.AddComponent<Image>();
        return imageGO;
    }

    private GameObject CreateText(string name, Vector2 anchoredPosition, string text, Transform parent = null)
{
    GameObject textGO = new GameObject(name);
    RectTransform rectTransform = textGO.AddComponent<RectTransform>();
    rectTransform.SetParent(parent, false);
    rectTransform.anchoredPosition = anchoredPosition;

    Text textComponent = textGO.AddComponent<Text>();
    textComponent.text = text;
    
    // Load a valid built-in font (LegacyRuntime.ttf)
    //textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

    // Set other text properties as needed
    textComponent.fontSize = 16; // Adjust the font size as per your UI design
    textComponent.color = Color.black; // Set text color
    
    // Optionally, set alignment and other properties
    textComponent.alignment = TextAnchor.MiddleCenter;

    return textGO;
}


    private GameObject CreateButton(string name, Vector2 anchoredPosition, string buttonText, Transform parent = null)
    {
        GameObject buttonGO = new GameObject(name);
        RectTransform rectTransform = buttonGO.AddComponent<RectTransform>();
        rectTransform.SetParent(parent, false);
        rectTransform.anchoredPosition = anchoredPosition;
        Button buttonComponent = buttonGO.AddComponent<Button>();
        Text buttonTextComponent = CreateText("ButtonText", Vector2.zero, buttonText, buttonGO.transform).GetComponent<Text>();
        buttonTextComponent.alignment = TextAnchor.MiddleCenter;
        buttonTextComponent.fontSize = 20;
        return buttonGO;
    }

    private Sprite LoadSpriteFromPath(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found at path: " + filePath);
            return null;
        }

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

}

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
using Assets.HeroEditor.Common.Scripts.CharacterScripts.Firearms.Enums;
using HeroEditor.Common.Enums;
using Unity.VisualScripting;
using Assets.HeroEditor.Common.Scripts.ExampleScripts;
using System.Linq;
using UnityEngine.SceneManagement;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Assets.HeroEditor.InventorySystem.Scripts.Enums;
using Assets.HeroEditor.InventorySystem.Scripts.Helpers;
using UnityEditor;

public class BattleManager  : MonoBehaviour
{
    private Character player;
    private Character enemy;
private List<string> spritePaths = new List<string>();
    
    private CharacterItemExtractor itemExtractor;
    int round  = 0 ;
    

    public CharacterStats playerStats ;
    public    CharacterStats enemyStats ;
    public float playerMaxHP;
     public float enemyMaxHP;

    private bool isMoving = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float moveSpeed = 200f; // Speed of movement
    private float waitTime = 0.4f; // Time to wait between actions
    public InventoryExample PlayerInventory ;

    public BattleManager(Character player, Character enemy)
    {
        this.player = player;
        this.enemy = enemy;
        if (player != null)
        {
            string names = GetAllChildGameObjectNames(player.gameObject);
            DisplayNames(names);
        }
        else
        {
            Debug.LogError("Character GameObject is not assigned.");
        }
//        PlayerInventory.Character = player ;

                this.itemExtractor = new CharacterItemExtractor(); // Initialize the item extractor

                // Calculate stats for both characters
        playerStats = CalculateStats(player);
        enemyStats = CalculateStats(enemy);
        playerStats.DisplayStats();
        enemyStats.DisplayStats();
        playerMaxHP = playerStats.HP;
        enemyMaxHP = enemyStats.HP;
        //Debug.Log("BattleManager loaded");
        //ExtractAllSprites();
        
    }
   


    private string GetAllChildGameObjectNames(GameObject parent)
    {
        string names = "";

        // Loop through each child GameObject
        foreach (Transform child in parent.transform)
        {
            names += child.gameObject.name; // Append the child's name
            
            // Check for SpriteRenderer component
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                names += " (Sprite Found : "+ spriteRenderer.sprite.texture.name + ")"; // Indicate that a sprite is found
            }
            names += "\n"; // New line

            // Recursively get names of any children of this child
            names += GetAllChildGameObjectNames(child.gameObject);
        }

        return names; // Return the complete list of names
    }

    private void DisplayNames(string names)
    {
        
            Debug.Log("Display item name " + names);
        
    }

     // Function to extract item information for both characters
    public void ExtractCharacterItems()
    {
        // Load item data from JSON
        //itemExtractor.LoadItemData("items_data.json"); // Update with your actual path

        //Debug.Log("Extracting item information for player and enemy.");

        // Extract item info for player
        return;
       // itemExtractor.Start();
        //player.DisplayEquipment();
        //enemy.DisplayEquipment();
        // Extract item info for enemy
       // itemExtractor.ExtractItemInfo(enemy);
    }
    
// Calculate stats for a character based on equipped items
    private CharacterStats CalculateStats(Character character)
    {
        CharacterStats stats = new CharacterStats (character.name);
        // Calculation logic here...
        return stats;
    }

    // Simulate the entire battle between player and enemy
    public  string Battle()
    {
        round ++;
        // Extract item information for both characters
        //ExtractCharacterItems();

        // Calculate stats for both characters
        playerStats = CalculateStats(player);
        enemyStats = CalculateStats(enemy);
                //Debug.Log($"{playerStats.Name} attacks {enemyStats.Name} ");

        //Debug.Log("round begins between " + playerStats.Name + " and " + enemyStats.Name);

        // Simple turn-based loop
        if (playerStats.HP > 0 && enemyStats.HP > 0)
        {
            AttackEnemy();

            // Check if the enemy is still alive
            if (enemyStats.HP <= 0)
            {
                //Debug.Log(playerStats.Name + " wins!");
                
            }

            // Enemy attacks player
           AttackPlayer();

            // Check if the player is still alive
            if (playerStats.HP <= 0)
            {
                //Debug.Log(enemyStats.Name + " wins!");
                
            }
        }else{
             //Debug.Log("Fight last + "+round+" rounds ");
             round = 0;
        }
        return round.ToString();
    }

    // Perform an attack from one character to another
    public float PerformAttack(CharacterStats attackerStats , CharacterStats defenderStats)
    {
float damage = Mathf.Max(0, (attackerStats.Attack - defenderStats.Defense / 2f) / 3f);

        defenderStats.HP -= damage ;//because now 3 attacks 
        return damage;
        //Debug.Log($"{attackerStats.Name} attacks {attackerStats.Name} for {damage} damage. {attackerStats.Name} has {attackerStats.HP} life left.");
    }

    
private IEnumerator AttackEnemy() {
                originalPosition = player.transform.position; // Save original position
            targetPosition = enemy.transform.position - new Vector3(10f, 0f, 0f); // Target position near the enemy

                player.SetState(CharacterState.Run);
                player.SetExpression("Default");

    
    // Move toward the enemy
    yield return StartCoroutine(MoveCharacter(player.transform, targetPosition, moveSpeed));

    // Perform attacks after reaching the enemy
    player.SetState(CharacterState.Run);
    
    // Perform attack sequence
    player.Jab();
    yield return new WaitForSeconds(waitTime);

    player.Shoot();
    yield return new WaitForSeconds(waitTime);

    player.Slash();
    yield return new WaitForSeconds(waitTime);

    PerformAttack(playerStats,enemyStats);


    // Move back to original position
    yield return StartCoroutine(MoveCharacter(player.transform, originalPosition, moveSpeed));



    
                player.SetState(CharacterState.Relax);
                // Handle attack logic

}

private IEnumerator AttackPlayer() {
                originalPosition = enemy.transform.position; // Save original position
            targetPosition = player.transform.position - new Vector3(10f, 0f, 0f); // Target position near the enemy

                enemy.SetState(CharacterState.Run);
                enemy.SetExpression("Default");

    
    // Move toward the enemy
    yield return StartCoroutine(MoveCharacter(enemy.transform, targetPosition, moveSpeed));

    // Perform attacks after reaching the enemy
    enemy.SetState(CharacterState.Run);
    
    // Perform attack sequence
    enemy.Jab();
    yield return new WaitForSeconds(waitTime);

    enemy.Shoot();
    yield return new WaitForSeconds(waitTime);

    enemy.Slash();
    yield return new WaitForSeconds(waitTime);

    PerformAttack(enemyStats,playerStats);


    // Move back to original position
    yield return StartCoroutine(MoveCharacter(enemy.transform, originalPosition, moveSpeed));



    
                enemy.SetState(CharacterState.Relax);
                // Handle attack logic

}


private IEnumerator MoveCharacter(Transform character, Vector3 destination, float speed) {
    while (Vector3.Distance(character.position, destination) > 0.01f) {
        character.position = Vector3.MoveTowards(character.position, destination, speed * Time.deltaTime);
        yield return null; // Wait for the next frame
    }
}
}

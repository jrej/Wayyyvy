using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterStats
{
    public string Name = "NewFighter";  // Character's name

    // Character stats
    public float HP = 100;               // Hit Points (HP)
    public float Attack = 50;            // Physical Attack
    public float Defense = 50;           // Physical Defense
    public float SpecialAttack = 50;     // Special Attack
    public float SpecialDefense = 50;    // Special Defense
    public float Speed = 50;             // Speed

    // New Leveling System
    public int Level = 1;                // Character Level (1 to 100)
    public float Experience = 0;         // Current experience
    public float ExperienceToNextLevel = 100;  // Experience needed to reach the next level

    // New Luck System
    public float Luck = 0;               // Luck percentage (calculated based on Defense, Speed, and Level)

    // Constructor to initialize the stats
    public CharacterStats(string name, int hp, int attack, int defense, int specialAttack, int specialDefense, int speed)
    {
        Name = name;
        HP = hp;
        Attack = attack;
        Defense = defense;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        Speed = speed;

        CalculateLuck();  // Calculate luck at initialization
    }

    public CharacterStats(string name)
    {
        Name = name;

        // Assign random values within a specified range
        HP = UnityEngine.Random.Range(50, 151);
        Attack = UnityEngine.Random.Range(30, 101);
        Defense = UnityEngine.Random.Range(30, 101);
        SpecialAttack = UnityEngine.Random.Range(30, 101);
        SpecialDefense = UnityEngine.Random.Range(30, 101);
        Speed = UnityEngine.Random.Range(30, 101);

        CalculateLuck();  // Calculate luck at initialization
    }

    // Level up the character when experience threshold is reached
    public void GainExperience(float exp)
    {
        Experience += exp;

        // Check if the character has enough experience to level up
        while (Experience >= ExperienceToNextLevel && Level < 100)
        {
            LevelUp();
        }
    }

    // Function to handle leveling up
    private void LevelUp()
{
    Level++;
    Experience -= ExperienceToNextLevel;  // Subtract the experience needed for the level up
    ExperienceToNextLevel *= 1.1f;  // Increase the experience required for the next level (can adjust scaling factor)

    // Randomly improve character stats on level up
    float randomHPIncrease = UnityEngine.Random.Range(8, 15);   // Random HP increase between 8 and 15
    float randomAttackIncrease = UnityEngine.Random.Range(3, 7);  // Random Attack increase between 3 and 7
    float randomDefenseIncrease = UnityEngine.Random.Range(3, 7); // Random Defense increase between 3 and 7
    float randomSpeedIncrease = UnityEngine.Random.Range(3, 7);   // Random Speed increase between 3 and 7
    float randomSpecialAttackIncrease = UnityEngine.Random.Range(3, 7); // Random Special Attack increase
    float randomSpecialDefenseIncrease = UnityEngine.Random.Range(3, 7); // Random Special Defense increase

    // Apply the random stat increases
    HP += randomHPIncrease;
    Attack += randomAttackIncrease;
    Defense += randomDefenseIncrease;
    Speed += randomSpeedIncrease;
    SpecialAttack += randomSpecialAttackIncrease;
    SpecialDefense += randomSpecialDefenseIncrease;

    // Recalculate luck after leveling up
    CalculateLuck();

    // Log the results of the level-up process
    Debug.Log(Name + " leveled up to " + Level + "!");
    Debug.Log("HP increased by " + randomHPIncrease);
    Debug.Log("Attack increased by " + randomAttackIncrease);
    Debug.Log("Defense increased by " + randomDefenseIncrease);
    Debug.Log("Speed increased by " + randomSpeedIncrease);
    Debug.Log("Special Attack increased by " + randomSpecialAttackIncrease);
    Debug.Log("Special Defense increased by " + randomSpecialDefenseIncrease);
}


    // Method to calculate the luck percentage based on attributes
    public void CalculateLuck()
    {
        // Example formula: Luck is based on defense, speed, and level
        Luck = (Defense * 0.3f + Speed * 0.2f + Level * 0.1f) / 2;
        Luck = Mathf.Clamp(Luck, 0, 100);  // Ensure that luck stays within the range of 0-100%
        Debug.Log("Luck calculated for " + Name + ": " + Luck + "%");
    }

    // Method to display stats in the console
    public void DisplayStats()
    {
        Debug.Log("Name: " + Name);
        Debug.Log("Level: " + Level);
        Debug.Log("HP: " + HP);
        Debug.Log("Attack: " + Attack);
        Debug.Log("Defense: " + Defense);
        Debug.Log("Special Attack: " + SpecialAttack);
        Debug.Log("Special Defense: " + SpecialDefense);
        Debug.Log("Speed: " + Speed);
        Debug.Log("Experience: " + Experience + " / " + ExperienceToNextLevel);
        Debug.Log("Luck: " + Luck + "%");
    }
}

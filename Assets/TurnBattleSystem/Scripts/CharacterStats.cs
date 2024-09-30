using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
public class CharacterStats

{
    public string Name = "NewFighter";  // Pokémon's name

    // Pokémon stats
    public float HP = 100;               // Hit Points (HP)
    public float Attack = 50;            // Physical Attack
    public float Defense = 50;           // Physical Defense
    public float SpecialAttack = 50;     // Special Attack
    public float SpecialDefense = 50;    // Special Defense
    public float Speed = 50;             // Speed

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
    }
    // Constructor to initialize with random values
    public CharacterStats(string name)
    {
        Name = name;

        // Assign random values within a specified range
        HP = UnityEngine.Random.Range(50, 151);               // Random HP between 50 and 150
        Attack = UnityEngine.Random.Range(30, 101);           // UnityEngine.Random Attack between 30 and 100
        Defense = UnityEngine.Random.Range(30, 101);          // UnityEngine.Random Defense between 30 and 100
        SpecialAttack = UnityEngine.Random.Range(30, 101);    // UnityEngine.Random Special Attack between 30 and 100
        SpecialDefense = UnityEngine.Random.Range(30, 101);   // UnityEngine.Random Special Defense between 30 and 100
        Speed = UnityEngine.Random.Range(30, 101);            // Random Speed between 30 and 100
    }
    public CharacterStats()
    {
        Name = "Undefined";

        // Assign random values within a specified range
        HP = UnityEngine.Random.Range(100, 151);               // Random HP between 50 and 150
        Attack = UnityEngine.Random.Range(50, 90);           // UnityEngine.Random Attack between 30 and 100
        Defense = UnityEngine.Random.Range(30, 101);          // UnityEngine.Random Defense between 30 and 100
        SpecialAttack = UnityEngine.Random.Range(30, 101);    // UnityEngine.Random Special Attack between 30 and 100
        SpecialDefense = UnityEngine.Random.Range(30, 101);   // UnityEngine.Random Special Defense between 30 and 100
        Speed = UnityEngine.Random.Range(30, 101);            // Random Speed between 30 and 100
    }

    // Method to display stats in the console
    public void DisplayStats()
    {
        Debug.Log("Name: " + Name);
        Debug.Log("HP: " + HP);
        Debug.Log("Attack: " + Attack);
        Debug.Log("Defense: " + Defense);
        Debug.Log("Special Attack: " + SpecialAttack);
        Debug.Log("Special Defense: " + SpecialDefense);
        Debug.Log("Speed: " + Speed);
    }
}

    // Add other relevant stats


﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CharacterBattle : MonoBehaviour {

    private Character_Base characterBase;
    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    private bool isPlayerTeam;
    private GameObject selectionCircleGameObject;
    private HealthSystem healthSystem;
    private World_Bar healthBar;
    private float dodgeChance = 0.5f; // 20% chance to dodge
    private Vector3 originalPosition;
    private PlayerConfig playerConfig;


    private enum State {
        Idle,
        Sliding,
        Busy,
        Dodging,
        Jumping,
    }

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
        HideSelectionCircle();
        state = State.Idle;
    }

    private void Start() {
    }


    public void Setup(bool isPlayerTeam) {
         playerConfig = new PlayerConfig();
        playerConfig.LoadPlayerConfig("configPlayer.txt");
        playerConfig.CalculateTotalStats();
        this.isPlayerTeam = isPlayerTeam;
        
        if (isPlayerTeam) {
            characterBase.SetAnimsSwordTwoHandedBack();
            characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().playerSpritesheet;
        } else {
            characterBase.SetAnimsSwordShield();
            characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().enemySpritesheet;
        }
playerConfig.totalLifePoints = 100 ;
        // Use playerConfig stats for initial setup
    healthSystem = new HealthSystem(playerConfig.totalLifePoints);
    Debug.Log("LIFEPOINT : " + playerConfig.totalLifePoints);
    healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(12, 1.7f), Color.grey, Color.red, 1f, playerConfig.totalLifePoints, new World_Bar.Outline { color = Color.black, size = .6f });

       // healthSystem = new HealthSystem(100);
       // healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(12, 1.7f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = .6f });
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        PlayAnimIdle();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        healthBar.SetSize(healthSystem.GetHealthPercent());
    }

    private void PlayAnimIdle() {
        if (isPlayerTeam) {
            characterBase.PlayAnimIdle(new Vector3(+1, 0));
        } else {
            characterBase.PlayAnimIdle(new Vector3(-1, 0));
        }
    }

    public int PlayAnimJump(Vector3 direction) {
        return 0;
    }

    private void Update() {
        float reachedDistance = 0;
        switch (state) {
        case State.Idle:
            break;
        case State.Busy:
            break;
        case State.Sliding:
            float slideSpeed = 10f;
            transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

            reachedDistance = 1f;
            if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance) {
                // Arrived at Slide Target Position
                onSlideComplete();
            }
            break;
        case State.Dodging:
            float dodgeSpeed = 15f;
            reachedDistance = 1f;

            transform.position += (slideTargetPosition - GetPosition()) * dodgeSpeed * Time.deltaTime;

            if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance) {
                // Arrived at Dodge Target Position
                state = State.Idle;
                onSlideComplete();
            }
            break;
        case State.Jumping:
            float jumpSpeed = 12f;
            reachedDistance = 1f;

            transform.position += (slideTargetPosition - GetPosition()) * jumpSpeed * Time.deltaTime;

            if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance) {
                // Arrived at Jump Target Position
                state = State.Busy;
                
            }
            break;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Damage(CharacterBattle attacker, int damageAmount) {
        if (UnityEngine.Random.value < dodgeChance) {
            // Dodge the attack
            Dodge(attacker, () => {
                // Successfully dodged, back to idle
                state = State.Idle;
            });

            return;
        }

        healthSystem.Damage(damageAmount);
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        Vector3 dirFromAttacker = (GetPosition() - attacker.GetPosition()).normalized;

        DamagePopup.Create(GetPosition(), damageAmount, false);
        characterBase.SetColorTint(new Color(1, 0, 0, 1f));
        Blood_Handler.SpawnBlood(GetPosition(), dirFromAttacker);

        CodeMonkey.Utils.UtilsClass.ShakeCamera(1f, .1f);

        if (healthSystem.IsDead()) {
            // Died
            characterBase.PlayAnimLyingUp();
        }
    }

    private void Dodge(CharacterBattle attacker, Action onDodgeComplete) {

    
// Use agility to determine dodge
    if (UnityEngine.Random.value < playerConfig.totalAgility * 0.01f) {
        // Dodge the attack
        Dodge(attacker, () => {
            state = State.Idle;
        });
        return;
    }


    Vector3 dodgeDirection = (GetPosition() - attacker.GetPosition()).normalized;
    Vector3 position = GetPosition();
    Vector3 dodgeTargetPosition = position + dodgeDirection * 10f; // Adjust the distance as needed

    // Slide to dodge position
    SlideToPosition(dodgeTargetPosition, () => {
        // Dodge completed, invoke callback
        state = State.Idle;
        onDodgeComplete();
    });

    // Set dodge state and play animation
    state = State.Dodging;
    characterBase.PlayAnimDodge(dodgeDirection);


    // Slide to dodge position
    SlideToPosition(position, () => {
        // Dodge completed, invoke callback
        state = State.Idle;
        onDodgeComplete();
    });

    // Set dodge state and play animation
    state = State.Dodging;
    characterBase.PlayAnimDodge(-dodgeDirection);
}


    public bool IsDead() {
        return healthSystem.IsDead();
    }

    public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete) {
    float jumpAttackChance = 0.0f; // 30% chance to perform a jump attack
    float randomValue = UnityEngine.Random.value;

    if (randomValue < jumpAttackChance) {
        // Perform a jump attack
        JumpAttack(targetCharacterBattle, onAttackComplete);
    } else {
        // Perform a slide attack
        Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 10f;
        Vector3 startingPosition = GetPosition();

        // Slide to Target
        SlideToPosition(slideTargetPosition, () => {
            // Arrived at Target, attack him
            state = State.Busy;
            Vector3 attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;


     



            characterBase.PlayAnimAttack(attackDir, () => {
                        // Use the attack stat when calculating damage
            int damageAmount = UnityEngine.Random.Range(playerConfig.totalAttack - 10, playerConfig.totalAttack + 10);
            targetCharacterBattle.Damage(this, damageAmount);
                }, () => {
                // Attack completed, slide back
                SlideToPosition(startingPosition, () => {
                    // Slide back completed, back to idle
                    state = State.Idle;
                    characterBase.PlayAnimIdle(attackDir);
                    onAttackComplete();
                });
            });
        });
    }
}


    public void JumpAttack(CharacterBattle targetCharacterBattle, Action onAttackComplete) {
        originalPosition = GetPosition();
        Vector3 jumpTargetPosition = targetCharacterBattle.GetPosition();
        // Jump to Target
        SlideToPosition(jumpTargetPosition, () => {
            // Arrived at Target, perform the attack
            state = State.Busy;
            characterBase.PlayAnimAttack((jumpTargetPosition - originalPosition).normalized, () => {
                // Target hit
                int damageAmount = UnityEngine.Random.Range(20, 50);
                targetCharacterBattle.Damage(this, damageAmount);
                }, () => {
                // Attack completed, jump back
                SlideToPosition(originalPosition, () => {
                    // Jump back completed, back to idle
                    state = State.Idle;
                    characterBase.PlayAnimIdle((originalPosition - jumpTargetPosition).normalized);
                    onAttackComplete();
                });
            });
        });
        state = State.Jumping;
        //characterBase.PlayAnimJump((jumpTargetPosition - originalPosition).normalized);
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete) {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
        if (slideTargetPosition.x > 0) {
            characterBase.PlayAnimSlideRight();
        } else {
            characterBase.PlayAnimSlideLeft();
        }
    }

    public void HideSelectionCircle() {
        selectionCircleGameObject.SetActive(false);
    }

    public void ShowSelectionCircle() { 
        selectionCircleGameObject.SetActive(true);
    }
}

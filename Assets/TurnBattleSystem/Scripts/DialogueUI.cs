using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour {
    public Text dialogueText;
    public Button continueButton;

    private void Start() {
        Hide();
        continueButton.onClick.AddListener(() => Hide());
    }

    public void Show(string dialogue) {
        dialogueText.text = dialogue;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class Dialogue {
    public List<string> lines;
}
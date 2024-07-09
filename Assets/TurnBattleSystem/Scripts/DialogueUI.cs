using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;


public class DialogueUI : MonoBehaviour {
    public Text dialogueText;
    private Coroutine typingCoroutine;

    public void ShowDialogue(string line) {
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line) {
        dialogueText.text = "This is a test";
        foreach (char letter in line.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust typing speed here
        }
    }

    public void HideDialogue() {
        dialogueText.text = "";
    }
}

using UnityEngine;

public class QuitManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            QuitGame();
        }
    }

    public void QuitGame()
    {
         Debug.Log("quit game");

         Application.Quit();
    }
}
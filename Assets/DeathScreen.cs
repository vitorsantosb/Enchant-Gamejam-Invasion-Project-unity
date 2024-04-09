using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathScreen : MonoBehaviour
{
    public void QuitApplication()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }
    
}

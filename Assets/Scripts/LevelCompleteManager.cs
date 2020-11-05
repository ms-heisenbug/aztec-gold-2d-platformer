using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
     static string timerS;
     [SerializeField] TextMeshProUGUI txt;

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 == 4)
        {
            SceneManager.LoadScene(5);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        txt.text += timerS + " seconds";
    }

    public static void SetTimer(string timer)
    {
        timerS = timer;
        Debug.Log("Timer: " + timerS);
    }
}

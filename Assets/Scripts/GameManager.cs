using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject deathMenu;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerStartPoint;

    public void EndGame()
    {
        player.gameObject.SetActive(false);
        deathMenu.gameObject.SetActive(true);
    }

    public void Restart()
    {
        deathMenu.gameObject.SetActive(false);
        ResetPlayerPosition();
        ReloadCurrentScene();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = playerStartPoint.transform.position;
        player.gameObject.SetActive(true);
    }

    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

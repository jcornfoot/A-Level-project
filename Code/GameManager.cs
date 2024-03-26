using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Reference:
https://www.youtube.com/watch?v=pKFtyaAPzYo
*/

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public void GameOver() {
        gameOverUI.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Menu() {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void Quit() {
        Application.Quit();
    }
}

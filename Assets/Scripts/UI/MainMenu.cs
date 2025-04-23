using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int FirstGame = 0;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        FirstGame = PlayerPrefs.GetInt("FirstTimeCheck");
    }
    public void Start()
    {
        if (FirstGame == 0)
        {
            SceneManager.LoadScene(2);
        }
    }
}

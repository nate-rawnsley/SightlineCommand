using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour //made by Dylan
{
    private int FirstGame = 0;
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    } //basic loading scenes depending on buttons
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        FirstGame = PlayerPrefs.GetInt("FirstTimeCheck"); //checks if the first time opening the game to force player into tutorial
    }
    public void Start()
    {
        if (FirstGame == 0)
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("FirstTimeCheck", 1); //first time check done here
        }
    }

    public void Level1()
    {
        SceneManager.LoadScene(2);
        }
    public void Level2()
    {
        SceneManager.LoadScene(3);
    }
    public void Level3()
    {

        SceneManager.LoadScene(4);
    }//basic loading scenes depending on buttons
    public void MainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
    public void TutorialScene()
    {
        SceneManager.LoadScene(1);
    }


}

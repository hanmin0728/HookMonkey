using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoSingleton<SceneHandler>
{
  public void Restart()
    {
        SceneManager.LoadScene("Stage");
        Time.timeScale = 1;
    } 
    public void Lobby()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public void Stage2()
    {
        SceneManager.LoadScene("Stage2");
        Time.timeScale = 1;
    }   
    public void Stage3()
    {
        SceneManager.LoadScene("Stage3");
        Time.timeScale = 1;
    }
    public void Clear()
    {
        SceneManager.LoadScene("Clear");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

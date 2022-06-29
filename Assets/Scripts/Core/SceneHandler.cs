using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoSingleton<SceneHandler>
{
  public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("One");
    } 
    public void Lobby()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    public void Stage2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Two");
    }   
    public void Stage3()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Three");
    }
    public void Clear()
    {
        SceneManager.LoadScene("Clear");
        Time.timeScale = 1;
    }
    public void TimesacleSet()
    {
        Time.timeScale = 1;
    }  
    public void TimescaleZero()
    {
        Time.timeScale = 0;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}

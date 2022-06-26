using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoBehaviour
{
  public void Restart()
    {
        SceneManager.LoadScene("DC");
        Time.timeScale = 1;
    } 
    public void Lobby()
    {
        SceneManager.LoadScene("A");
        Time.timeScale = 1;
    }
}

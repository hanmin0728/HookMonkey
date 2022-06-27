using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector director;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            director.Play();
        }    
    }
}

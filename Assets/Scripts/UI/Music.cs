using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private int MuteSounds = 0;
    public AudioSource MusicSource;
    private void Awake()
    {        
        MusicChange();
    }
    private void Update()
    {
        MuteSounds = PlayerPrefs.GetInt("MuteCheck");
    }
    public void MusicChange()
    {
        if (MuteSounds == 1)
        {
            MusicSource.Pause();
        }
        else {
            MusicSource.Play();
        }
    }

    public void MUTE(bool Check)
    {
        Check = !Check;
        if (Check == true)
        {
            PlayerPrefs.SetInt("MuteCheck", 0);
        }
        if (Check == false) {
            PlayerPrefs.SetInt("MuteCheck", 1);
        }
            
    }

}

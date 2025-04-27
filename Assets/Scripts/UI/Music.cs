using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private int MuteSounds = 0;
    public AudioSource MusicSource;
    private void Awake()
    {
        MuteSounds = PlayerPrefs.GetInt("MuteCheck");
    }
    private void Start()
    {
        if (MuteSounds == 1)
        {
            MusicSource.volume = 0;
        }
        else {
            MusicSource.volume = 0.5f;
                }
    }

    public void MUTE()
    {
        if (MuteSounds == 1)
        {
            PlayerPrefs.SetInt("MuteCheck", 0);
        }
        if (MuteSounds == 0) {
            PlayerPrefs.SetInt("MuteCheck", 1);
        }
            
    }

}

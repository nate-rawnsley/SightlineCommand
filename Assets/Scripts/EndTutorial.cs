using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorial : MonoBehaviour
{
    public void EndTutor()
    {
        PlayerPrefs.SetInt("FirstTimeCheck", 1);
    }
}

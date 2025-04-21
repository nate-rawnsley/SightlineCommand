using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private int CamAngle = 1;
    public Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }
    public void ChangeCam()
    {
            switch (CamAngle)
        {
            case 0:
                cam.transform.position = new Vector3(-44.9f, 41.5f, -17.1f);
                cam.transform.rotation = Quaternion.Euler(50f, 90f, 0f);
                CamAngle = 1;
                break;
            case 1:
                cam.transform.position = new Vector3(39.1f, 41.5f, -17.1f);
                cam.transform.rotation = Quaternion.Euler(50f, -90f, 0f);
                CamAngle = 0;
                break;
        }
    }

}
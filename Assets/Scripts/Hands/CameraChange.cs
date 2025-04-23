using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private int CamAngle = 1;
    public Camera cam;
    public Vector3 HumanPosition;
    public Vector3 HumanRotation = new Vector3(50f, 90f, 0f);
    public Vector3 AlienPosition;
    public Vector3 AlienRotation = new Vector3(50f, -90f, 0f);

    private void Awake()
    {
        cam = Camera.main;
    }
    public void ChangeCam()
    {
            switch (CamAngle)
        {
            case 0:
                cam.transform.position = HumanPosition;
                cam.transform.rotation = Quaternion.Euler(HumanRotation);
                CamAngle = 1;
                break;
            case 1:
                cam.transform.position = AlienPosition;
                cam.transform.rotation = Quaternion.Euler(AlienRotation);
                CamAngle = 0;
                break;
        }
    }

}
using UnityEngine;


/// <summary>
/// Nate
/// Simple canvas billboard script that sets rotation when the camera rotation is changed.
/// Adapted from https://www.youtube.com/watch?v=eiGvVgwtJ8k
/// </summary>
public class Billboard : MonoBehaviour {

    private void OnEnable() {
        CameraChange.CameraChanged += SetForward; //makes sure that all of the texts are facing towards the camera once spawned
    }

    private void OnDisable() {
        CameraChange.CameraChanged -= SetForward;
    }

    private void Awake() {
        SetForward();
    }

    private void SetForward() {
        transform.rotation = Camera.main.transform.rotation;
        //transform.forward = Camera.main.transform.forward;
    }
}

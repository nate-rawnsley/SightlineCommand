using UnityEngine;

/// <summary>
/// Nate
/// A simple script on the camera that allows it to be manually repositioned with WASD + Q/E.
/// </summary>
public class CameraMovement : MonoBehaviour {

    public float speed = 25f;
    private float initialSpeed;
    private int direction = 1;

    private void Awake() {
        initialSpeed = speed;
        CameraChange.CameraChanged += SwapDirection;
    }

    /// <summary>
    /// Reverses the orientation of the camera, as the separate teams have opposite viewpoints.
    /// Called by action on the CameraChange script.
    /// </summary>
    private void SwapDirection() {
        direction *= -1;
    }

    public void SetInitialPosition(float scale) {
        Vector3 newPos = transform.position;
        newPos.y = scale * 10;
        speed = initialSpeed * scale;

    }

    private void Update() {
        sprint();

        Vector3 newPos = transform.position;
        float difference = Time.deltaTime * speed * direction;
        //to be replaced with Unity Input System in future
        if (Input.GetKey(KeyCode.W)) {
            newPos.z += difference;
        }
        if (Input.GetKey(KeyCode.A)) {
            newPos.x -= difference;
        }
        if (Input.GetKey(KeyCode.S)) {
            newPos.z -= difference;
        }
        if (Input.GetKey(KeyCode.D)) {
            newPos.x += difference;
        }
        if (Input.GetKey(KeyCode.Q)) {
            newPos.y -= Time.deltaTime * speed; 
        }   
        if (Input.GetKey(KeyCode.E)) {
            newPos.y += Time.deltaTime * speed;
        }

        transform.position = newPos;
    }

    private void sprint() //Added By Dylan
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 60f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 25f;
        }
    }
}

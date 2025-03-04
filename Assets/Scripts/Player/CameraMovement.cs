using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed = 2.5f;
    private float initialSpeed;

    private void Awake() {
        initialSpeed = speed;
    }

    public void SetInitialPosition(float scale) {
        Vector3 newPos = transform.position;
        newPos.y = scale * 10;
        speed = initialSpeed * scale;
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
    }

    private void Update() {
        Vector3 newPos = transform.position;
        //this is not a neat way of doing this so i might change it later but it works for now.
        if (Input.GetKey(KeyCode.W)) {
            newPos.z += Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A)) {
            newPos.x -= Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S)) {
            newPos.z -= Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D)) {
            newPos.x += Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.Q)) {
            newPos.y -= Time.deltaTime * speed; 
        }   
        if (Input.GetKey(KeyCode.E)) {
            newPos.y += Time.deltaTime * speed;
        }

        transform.position = newPos;
    }
}

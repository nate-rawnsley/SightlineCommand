using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed = 2.5f;

    public void SetInitialPosition(float scale) {
        Vector3 newPos = transform.position;
        newPos.y = scale * 10;
        Debug.Log($"{speed}, {scale}, {speed * scale}");
        speed *= scale;
        transform.position = newPos;
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

using UnityEngine;

//from https://www.youtube.com/watch?v=eiGvVgwtJ8k

public class Billboard : MonoBehaviour {

    private void Awake() { 
        transform.forward = Camera.main.transform.forward;
    }

}

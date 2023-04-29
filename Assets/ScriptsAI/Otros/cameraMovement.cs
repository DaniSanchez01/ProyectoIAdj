using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 0.4f;
    float rotationSpeed = 1.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;

        if (Input.GetKey(KeyCode.W)) {
            pos = pos + transform.forward*speed;
        }
        if (Input.GetKey(KeyCode.S)) {
            pos = pos + transform.forward*-speed;
        }
        if (Input.GetKey(KeyCode.A)) {
            pos = pos + transform.right*-speed;
        }
        if (Input.GetKey(KeyCode.D)) {
            pos = pos + transform.right*speed;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            pos = new Vector3(pos.x,pos.y+ speed,pos.z);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            pos = new Vector3(pos.x,pos.y- speed,pos.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(0,-rotationSpeed,0);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(0,rotationSpeed,0);
        }

        transform.position = pos;
    }
}

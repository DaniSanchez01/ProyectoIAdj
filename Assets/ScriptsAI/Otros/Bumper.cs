using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    private float limit = 37.5f;
    void Update() {
        if (transform.position.x >limit || transform.position.x <-limit || transform.position.z >limit || transform.position.z <-limit){
            transform.position = Vector3.zero;
        }
    }
}

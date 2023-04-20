using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper3 : MonoBehaviour
{

    private float izqX = -10f;
    private float derX = 100f;
    private float izqZ = -10;
    private float derZ = 100f;

    void Update() {
        if (transform.position.x >derX || transform.position.x <izqX || transform.position.z >derZ || transform.position.z < izqZ){
            transform.position = new Vector3(20f,0f,42f);
        }
    }
}

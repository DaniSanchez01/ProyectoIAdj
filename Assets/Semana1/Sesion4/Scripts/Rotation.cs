using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update()
    {
        // https://docs.unity3d.com/ScriptReference/Transform.Rotate.html

        // Rotate the object around its local X axis at 1 degree per second
        //transform.Rotate(Vector3.right * Time.deltaTime * 200);

        // ...also rotate around the World's Y axis
        transform.Rotate(Vector3.up * Time.deltaTime * 1000, Space.World);
    }
}

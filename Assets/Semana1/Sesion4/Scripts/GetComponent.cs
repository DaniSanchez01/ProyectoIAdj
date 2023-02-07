using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComponent : MonoBehaviour
{
    public float mass = 0;    // public information
    public Material material = null;  // public information

    Rigidbody rb = null;   // Reference of the Rigidbody
    MeshRenderer mr = null; // Reference of the MeshRenderer

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   // Get Rigidbody from this.GameObject
        mr = GetComponent<MeshRenderer>(); // Get MeshRenderer from this.GameObject
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null) mass = rb.mass;  // Modify component Rigidboy and observe
        if (mr != null) material = mr.material;  // Modify component MeshRenderer and observe
    }
}

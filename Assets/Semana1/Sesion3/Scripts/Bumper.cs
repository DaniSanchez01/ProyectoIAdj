using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name.Equals("Frontera")) // ¿Colisiona con el objeto "Frontera"?
            transform.root.position = new Vector3(0,0.82f,0);  // Reset posición del padre.
    }
}

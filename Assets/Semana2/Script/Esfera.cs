using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esfera : MonoBehaviour
{

    [Tooltip("Radio interior de la IA")]
    [SerializeField] protected float _interiorRadius = 1f;

    [Tooltip("Radio de llegada de la IA")]
    [SerializeField] protected float _arrivalRadius = 3f;

    public string stringToEdit = "Hello World";

    void OnGUI()
    {
        // Make a text field that modifies stringToEdit.
        stringToEdit = GUI.TextField(new Rect(transform.position.x, transform.position.y, 200, 20), stringToEdit, 25);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interiorRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _arrivalRadius);

    }
}

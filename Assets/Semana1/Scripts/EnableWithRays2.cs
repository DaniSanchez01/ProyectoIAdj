using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWithRays2 : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject markSpecial;
    void Start()
    {
        
    }

    void Update() { // the user pressed down the virtual button identified by "Fire1".
         if (Input.GetButtonDown ("Fire1")) {
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit hit;
             if (Physics.Raycast (ray, out hit)) mark(hit.transform.gameObject );
         }
    }
    private void mark(GameObject thing) {
        if (thing.name.Equals("Cube") || thing.name.Equals("Sphere")) {

       
        GameObject marker = null;
        // If there is a child in the object called Mark
        if (thing.transform.Find("Mark") != null) // Get your reference
            marker = thing.transform.Find("Mark").gameObject;
        // If there is no reference then
        if (marker == null) {
            // create an instance of the prefab лл< CREATE INSTANTIATE
            marker = Instantiate(markSpecial, thing.transform);
            marker.transform.localPosition = Vector3.up * 1; // Change your position relative
            marker.name = "Mark"; // Change your name
        }
        else // If there is reference then
        Destroy(marker); // Destroy the object лл DESTROY
        }
    }
}

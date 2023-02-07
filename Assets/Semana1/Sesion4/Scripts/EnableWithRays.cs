using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWithRays : MonoBehaviour
{
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // the user pressed down the virtual button identified by "Fire1".
        if (Input.GetButtonDown("Fire1"))
        {
            // Returns a ray going from camera through a screen point.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Other: .Linecast() .BoxCast() .SphereCast() .CapsuleCast()
            if (Physics.Raycast(ray, out hit)) // draw marker
                draw(ray, hit);
        }

    }

    // <summary>
    /// Draw a marker
    /// </summary>
    /// <param name="ray">Ray</param>
    /// <param name="hit">Hit</param>
    void draw(Ray ray, RaycastHit hit)
    {
        GameObject thing = hit.transform.gameObject;

        string str = thing.name; //  object name

        if (str.Equals("Cube")) enable(thing); // Using components
        if (str.Equals("Sphere")) child(thing); // Using objects
    }


    /// <summary>
    /// Enable marker above the specified thing.
    /// </summary>
    /// <param name="thing">Thing. El gameobject</param>
    private void enable(GameObject thing)
    {
        bool show = false;

        Transform mark = thing.transform.Find("Mark");

        show = mark.GetComponent<Renderer>().enabled;

        mark.GetComponent<Renderer>().enabled = !show;
        mark.GetComponent<Rotation>().enabled = !show;
    }

    public GameObject markSpecial = null;
    private void child(GameObject thing)
    {
        GameObject mark = null;

        if (thing.transform.Find("Mark") != null)
            mark = thing.transform.Find("Mark").gameObject;

        if (mark == null)
        {
            mark = Instantiate(markSpecial, thing.transform);
            mark.transform.localPosition = Vector3.up * 1;
            mark.name = "Mark";
        }
        else
        {
            Destroy(mark);
        }
    }

}

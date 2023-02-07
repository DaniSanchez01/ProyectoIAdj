using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTraceScreen : MonoBehaviour
{
    // Indicates if there is an object with the color changed
    private bool firstTime = true;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Returns a ray going from camera through a screen point.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Other: .Linecast() .BoxCast() .SphereCast() .CapsuleCast()
        if (Physics.Raycast(ray, out hit))
            draw(ray, hit);

    }

    /// <summary>
    /// Draw the specified ray and hit.
    /// </summary>
    /// <param name="ray">Ray.</param>
    /// <param name="hit">Hit.</param>
    void draw(Ray ray, RaycastHit hit)
    {

        // The hit object is not the plane
        string str = hit.transform.gameObject.name;
        if ( !(str.Equals("Plane") || str.Equals("Quad")))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Debug.DrawLine(hit.point, hit.point + 20 * hit.normal, Color.blue);
        }

        // we will change the color, if possible
        changeColor(hit);
    }

    // Global variables to change the color

    // GameOjects of the scene
    private GameObject firstThing = null;
    private GameObject secondThing = null;

    // GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer = null;
    //The original color of the GameObject
    Color m_OriginalColor = Color.green;

    /// <summary>
    /// Change the color of the object "touched" by the mouse
    /// </summary>
    /// <param name="hit">Hit.</param>
    void changeColor(RaycastHit hit)
    {
        // It is a geometric figure and I have not changed the color yet
        string str = hit.transform.gameObject.name;
        if (firstTime && ! (str.Equals("Plane") || str.Equals("Quad")))
        {
            firstThing = hit.transform.gameObject;

            // Get the GameObject’s mesh renderer to access the GameObject’s material and color
            m_Renderer = firstThing.GetComponent<MeshRenderer>();

            // Fetch the original color of the GameObject
            m_OriginalColor = m_Renderer.material.color;

            // New material color
            m_Renderer.material.color = Color.gray;

            // The color is changed
            firstTime = false; 

            return;
        }

        // If the first hit object was the ground, this variable is not defined.
        if (firstThing == null) return;

        // We have an object with the changed color and we are hitting an object.
        secondThing = hit.transform.gameObject;

        // Will they be the same object?
        // If the answer is yes, you do not have to recover the color.
        if (firstThing == secondThing) return;

        // But if the answer is no,
        // Reset the color of the GameObject back to normal
        m_Renderer.material.color = m_OriginalColor;

        // Another color change is possible
        firstTime = true; 
    }
}

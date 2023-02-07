using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseEnter.html

    public Renderer rend;

    void Start()
    {
        Cursor.visible = true;
        rend = GetComponent<Renderer>();
    }

    // The mesh goes red when the mouse is over it...
    void OnMouseEnter()
    {
        rend.material.color = new Color(1, 0, 0);  //Color.red;
    }

    // ...the red fades out to blue as the mouse is held over...
    void OnMouseOver()
    {
        rend.material.color += new Color(-.5f, 0, .5f) * Time.deltaTime;
    }

    // ...and the mesh finally turns white when the mouse moves away.
    void OnMouseExit()
    {
        rend.material.color = Color.white;
    }
}

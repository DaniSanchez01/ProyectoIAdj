using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse1 : MonoBehaviour
 {
     public Renderer rend;
     
    void Start() {
        rend = GetComponent<Renderer>() ;
    }
    
    void OnMouseEnter() { // The mesh goes red when the mouse is over it...
        rend.material.color = new Color(1, 0, 0); //Color.red;
    }

    void OnMouseOver() { // ...the red fades out to blue as the mouse is held over...
        rend.material.color += new Color(-.5f, 0, .5f) * Time.deltaTime;
    }
    
    void OnMouseExit() { /* ...and the mesh finally turns white when the mouse moves away.*/
        rend.material.color = Color.white;
    }
}

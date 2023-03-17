using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable : MonoBehaviour
{
    public void enable()
    {
        bool show = false;

        Transform mark = transform.Find("Mark");

        show = mark.GetComponent<Renderer>().enabled;

        mark.GetComponent<Renderer>().enabled = !show;
        mark.GetComponent<Rotation1>().enabled = !show;
    }
    
}

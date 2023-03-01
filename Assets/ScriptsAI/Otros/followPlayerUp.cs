using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerUp : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(target.transform.position.x,transform.position.y, target.transform.position.z); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x,transform.position.y, target.transform.position.z); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerBehind : MonoBehaviour
{
    public GameObject target;
    private Vector3 dif;

    // Start is called before the first frame update
    void Start()
    {
        float x = target.transform.position.x - transform.position.x;
        float y = target.transform.position.y - transform.position.y;
        float z = target.transform.position.y - transform.position.y;
        dif = new Vector3(x,y,z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position - dif;
    }
}

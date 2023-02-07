using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaSteering : MonoBehaviour
{
    private Vector3 target;
    public float velocity = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newDirection = target - transform.position;

        transform.LookAt(transform.position + newDirection);

        transform.position += newDirection * velocity * Time.deltaTime;

    }

    public void NewTarget(Vector3 newtarget)
    {
        target = newtarget;
    }
}

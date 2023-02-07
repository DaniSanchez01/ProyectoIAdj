using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI4GamesSesion2
{
    public class Seek1 : MonoBehaviour
    {
        public Transform target;
        public float velocity = 2;

        void Update()
        {
            Vector3 newDirection = target.position - transform.position;

            // Mirar en la dirección del vector leído.
            transform.LookAt(transform.position + newDirection);

            // Avanzar de acuerdo a la velocidad establecida
            transform.position += newDirection * velocity * Time.deltaTime;
        }

        private void OnDrawGizmos() // Gizmo: una línea en la dirección del objetivo
        {
            Vector3 from = transform.position; // Origen de la línea
            Vector3 to = transform.localPosition + (target.position - transform.position) * velocity; // Detino de la línea
            Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

            from = from + elevation;
            to = to + elevation;

            Gizmos.DrawLine(from, to);
        }

    }
}
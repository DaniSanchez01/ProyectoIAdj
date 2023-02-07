using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI4GamesSesion3
{
    public class SeekAceleracion : MonoBehaviour
    {
        public Transform target;
        public float maxAceleration = 2;
        public float maxVelocity = 4;
        private Vector3 velocity = Vector3.zero;

        void Update()
        {
            Vector3 newDirection = target.position - transform.position;

            // Mirar en la dirección del vector leído.
            transform.LookAt(transform.position + newDirection);

            // Avanzar de acuerdo a la velocidad establecida
            velocity += newDirection * maxAceleration * Time.deltaTime;

            if (velocity.magnitude > maxVelocity)
                velocity = velocity.normalized * maxVelocity;

            transform.position += velocity * Time.deltaTime;
        }

        private void OnDrawGizmos() // Gizmo: una línea en la dirección del objetivo
        {
            Vector3 from = transform.position; // Origen de la línea
            Vector3 to = transform.position + velocity; // Detino de la línea
            Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

            from = from + elevation;
            to = to + elevation;

            Gizmos.color = Color.yellow;   // Mirando en la dirección de la velocidad.
            Gizmos.DrawLine(from, to);

            Gizmos.color = Color.red;        // Mirando en la dirección de la orientación.
            Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
            Gizmos.DrawRay(from, direction);
        }

    }
}
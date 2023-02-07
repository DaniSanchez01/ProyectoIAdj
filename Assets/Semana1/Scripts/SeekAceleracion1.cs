using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI4GamesSesion3
{
    public class SeekAceleracion1 : MonoBehaviour
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

        private void OnDrawGizmos()
        {
            Vector3 from = transform.position;
            Vector3 to = transform.localPosition+velocity;
            Vector3 elevation = new Vector3(0, 1, 0);

            from += elevation;
            to += elevation;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(from, to);

            Vector3 to2 = transform.forward *5;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(from, to2);

        }

    }
}
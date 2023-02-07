using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI4GamesSesion3
{
    public class Wander1 : MonoBehaviour
    {
        public float velocity = 4;    // Máxima velocidad

        public float maxAngle = 360; // Una circunferencia
        private float angle = 0f;   // Angulo de orientación actual.
        private float newAngle = 0f; // Nuevo ángulo (será aleatorio)

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            StartCoroutine("NewDirection");  // Inicia la corutina
        }

        /// <summary>
        /// Update is called every frame
        /// </summary>
        void Update()
        {
            angle = Mathf.LerpAngle(angle, newAngle, Time.deltaTime);  // Linearly interpolates between a and b by t.
            transform.eulerAngles = new Vector3(0, angle, 0);  // The rotation as Euler angles in degrees.
            transform.position += transform.forward * velocity * Time.deltaTime; // The world space new position of the Transform.
        }

        /// <summary>
        /// To generate a new random address
        /// </summary>
        /// <returns>The angle.</returns>
        /// A coroutine is like a function that has the ability to pause execution and 
        /// return control to Unity but then to continue where it left off on the following frame.
        IEnumerator NewDirection()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f); // Cambia la orientación cada 0.25 segundo.

                newAngle = (Random.value - Random.value) * maxAngle;
            }
        }
    }
}
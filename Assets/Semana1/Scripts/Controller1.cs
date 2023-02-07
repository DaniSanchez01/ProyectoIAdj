using UnityEngine;

namespace AI4GamesSesion1
{
    public class Controller1 : MonoBehaviour
    {
        public float velocity = 8f;

        // Update is called once per frame
        void Update()
        {
            // Leer el teclado
            Vector3 newDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Mirar en la dirección del vector leído.
            transform.LookAt(transform.position + newDirection);

            // Avanzar de acuerdo a la velocidad establecida
            transform.position += newDirection * velocity * Time.deltaTime;
        }
    }
}
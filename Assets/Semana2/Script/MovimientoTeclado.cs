using UnityEngine;

public class MovimientoTeclado : MonoBehaviour
{

    public Animator animator ;
    public Vector3 vel = new Vector3();

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 accel = new Vector3(horizontal, 0, vertical);

        vel += vel + accel * Time.deltaTime;

        animator.SetFloat("Velocity", vel.magnitude);


        transform.position += vel * Time.deltaTime + accel;
    }
}
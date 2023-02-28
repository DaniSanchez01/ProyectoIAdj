using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[AddComponentMenu("Steering/InteractiveObject/Agent")]
public class Agent : Bodi
{
    public float lookahead = 5f;
    public float avoidDistance = 2f;
    
    [Tooltip("Radio interior de la IA")]
    [SerializeField] protected float _interiorRadius = 1f;

    [Tooltip("Radio de llegada de la IA")]
    [SerializeField] protected float _arrivalRadius = 3f;

    [Tooltip("Ángulo interior de la IA")]
    [SerializeField] protected float _interiorAngle = 5f; // ángulo sexagesimal.

    [Tooltip("Ángulo exterior de la IA")]
    [SerializeField] protected float _exteriorAngle = 50f; // ángulo sexagesimal.

    // AÑADIR LAS PROPIEDADES PARA ESTOS ATRIBUTOS. SI LO VES NECESARIO.
    public bool giz = true;
    public float interiorRadius
    {
        get { return _interiorRadius; }
        set { _interiorRadius = Mathf.Clamp(value, 0, _arrivalRadius); }
    }

    public float arrivalRadius
    {
        get { return _arrivalRadius; }
        set { _arrivalRadius = Mathf.Max(_interiorRadius, value); }
    }

    public float interiorAngle
    {
        get { return _interiorAngle; }
        set { _interiorAngle = Mathf.Clamp(value, 0, _exteriorAngle); }
    }

    public float exteriorAngle
    {
        get { return _exteriorAngle; }
        set { _exteriorAngle = Mathf.Clamp(value,_interiorRadius, 180.0f); }
    }
    
    // AÑADIR MÉTODS FÁBRICA, SI LO VES NECESARIO.
    // En algún momento te puede interesar crear Agentes con tengan una posición
    // y unos radios: por ejemplo, crar un punto de llegada para un auténtico
    // Agente Inteligente. Este punto de llegada no tienen que ser inteligente,
    // solo tienen que ser "sensible" - si fuera necesario - a que lo tocan.
    // Planteate la posibilidad de crear aquí métodos fábrica (estáticos) para
    // crear esos agentes. Para ello crea un GameObject y usa:
    
    public static Agent CreateStaticVirtual(Vector3 pos, float intRadius = 1f, float arrRadius = 3f, bool paint = true) {
        GameObject virt = new GameObject();
        virt.AddComponent<BoxCollider>();
        virt.GetComponent<Collider>().isTrigger = true;
        Agent cuerpo = virt.AddComponent<Agent>();
        cuerpo.interiorRadius = intRadius;
        cuerpo.arrivalRadius = arrRadius;
        cuerpo.exteriorAngle = 50f;
        cuerpo.interiorAngle = 5f;
        cuerpo.Acceleration = Vector3.zero;
        cuerpo.AngularAcc = 0.0f;
        cuerpo.Velocity = Vector3.zero;
        cuerpo.Rotation = 0.0f;
        cuerpo.Position = pos;
        cuerpo.Orientation = 0f;
        cuerpo.giz = paint;
        return cuerpo;
    }
    public Agent CreateVirtual(Vector3 pos, float intRadius = -1, float arrRadius = -1, bool paint = true)
    {
        GameObject virt = new GameObject();
        virt.AddComponent<BoxCollider>();
        virt.GetComponent<Collider>().isTrigger = true;
        Agent cuerpo = virt.AddComponent<Agent>();
        if (intRadius == -1) intRadius = interiorRadius;
        cuerpo.interiorRadius = intRadius;
        if (arrRadius == -1) arrRadius = arrivalRadius;
        cuerpo.arrivalRadius = arrRadius;
        cuerpo.exteriorAngle = exteriorAngle;
        cuerpo.interiorAngle = interiorAngle;
        cuerpo.Acceleration = Vector3.zero;
        cuerpo.AngularAcc = 0.0f;
        cuerpo.Velocity = Vector3.zero;
        cuerpo.Rotation = 0.0f;
        cuerpo.Position = pos;
        cuerpo.Orientation = Orientation;
        cuerpo.giz = paint;
        return cuerpo;
    }

    public void UpdateVirtual(Agent cuerpo, Vector3 pos)
    {
        cuerpo.interiorRadius = interiorRadius;
        cuerpo.arrivalRadius = arrivalRadius;
        cuerpo.exteriorAngle = exteriorAngle;
        cuerpo.interiorAngle = interiorAngle;
        cuerpo.Acceleration = Vector3.zero;
        cuerpo.AngularAcc = 0.0f;
        cuerpo.Velocity = Vector3.zero;
        cuerpo.Rotation = 0.0f;
        cuerpo.Position = pos;
        cuerpo.Orientation = Orientation;
    }
    // .AddComponent<BoxCollider>();
    // .GetComponent<Collider>().isTrigger = true;
    // .AddComponent<Agent>();
    // Establece los valores del Bodi y radios/ángulos a los valores adecuados.
    // Esta es solo una de las muchas posiblidades para resolver este problema.



    // AÑADIR LO NECESARIO PARA MOSTRAR LA DEPURACIÓN. Te puede interesar los siguientes enlaces.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html
    // https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html
    // https://docs.unity3d.com/ScriptReference/Gizmos.DrawWireSphere.html
    // https://docs.unity3d.com/ScriptReference/Gizmos-color.html

    void OnDrawGizmos()
    {
        if (giz == true) {        
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _interiorRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _arrivalRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position,transform.position+2*Vector3.forward);
            Gizmos.color = Color.black;
            Vector3 exteriorPos = OrientationToVector(exteriorAngle+Orientation);
            Vector3 exteriorNeg = OrientationToVector(-exteriorAngle+Orientation);
            Gizmos.DrawLine(transform.position,transform.position + 5*exteriorPos);
            Gizmos.DrawLine(transform.position,transform.position + 5*exteriorNeg);
            Gizmos.color = Color.red;
            Vector3 interiorPos = OrientationToVector(interiorAngle+Orientation);
            Vector3 interiorNeg = OrientationToVector(-interiorAngle+Orientation);
            Gizmos.DrawLine(transform.position,transform.position + 5*interiorPos);
            Gizmos.DrawLine(transform.position,transform.position + 5*interiorNeg);
            Gizmos.color = Color.green;
            Vector3 forward = OrientationToVector(Orientation);
            Gizmos.DrawLine(transform.position,transform.position + 5*forward);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(Position,Position+Velocity.normalized*lookahead);
        }
    }
}



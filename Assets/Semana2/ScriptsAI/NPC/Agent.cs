using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[AddComponentMenu("Steering/InteractiveObject/Agent")]
public class Agent : Bodi
{
    [Tooltip("Radio interior de la IA")]
    [SerializeField] protected float _interiorRadius = 1f;

    [Tooltip("Radio de llegada de la IA")]
    [SerializeField] protected float _arrivalRadius = 3f;

    [Tooltip("Ángulo interior de la IA")]
    [SerializeField] protected float _interiorAngle = 5f; // ángulo sexagesimal.

    [Tooltip("Ángulo exterior de la IA")]
    [SerializeField] protected float _exteriorAngle = 30f; // ángulo sexagesimal.


    // AÑADIR LAS PROPIEDADES PARA ESTOS ATRIBUTOS. SI LO VES NECESARIO.

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
        set { _exteriorAngle = Mathf.Max(_interiorAngle, value); }
    }
    
    // AÑADIR MÉTODS FÁBRICA, SI LO VES NECESARIO.
    // En algún momento te puede interesar crear Agentes con tengan una posición
    // y unos radios: por ejemplo, crar un punto de llegada para un auténtico
    // Agente Inteligente. Este punto de llegada no tienen que ser inteligente,
    // solo tienen que ser "sensible" - si fuera necesario - a que lo tocan.
    // Planteate la posibilidad de crear aquí métodos fábrica (estáticos) para
    // crear esos agentes. Para ello crea un GameObject y usa:
    public void CreateVirtual(Vector3 pos, float or)
    {
        GameObject virt = new GameObject();
        virt.AddComponent<BoxCollider>();
        virt.GetComponent<Collider>().isTrigger = true;
        Agent cuerpo = virt.AddComponent<Agent>();
        cuerpo.interiorRadius = interiorRadius;
        cuerpo.arrivalRadius = arrivalRadius;
        cuerpo.Acceleration = Vector3.zero;
        cuerpo.AngularAcc = 0.0f;
        cuerpo.Velocity = Vector3.zero;
        cuerpo.Rotation = 0.0f;
        cuerpo.Position = pos;
        cuerpo.Orientation = or;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interiorRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _arrivalRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+2*Vector3.forward);

    }
}



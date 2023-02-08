using UnityEngine;

public enum Range {
    grados180,
    radianes
}
public class Bodi : MonoBehaviour
{

    [SerializeField] protected float _mass = 1;
    [SerializeField] protected float _maxSpeed = 1;
    [SerializeField] protected float _maxRotation = 1;
    [SerializeField] protected float _maxAcceleration = 1;
    [SerializeField] protected float _maxAngularAcc = 1;
    [SerializeField] protected float _maxForce = 1;

    protected Vector3 _acceleration; // aceleración lineal
    protected float _angularAcc;  // aceleración angular
    protected Vector3 _velocity; // velocidad lineal
    protected float _rotation;  // velocidad angular
    protected float _speed;  // velocidad escalar
    protected float _orientation;  // 'posición' angular
    // Se usará transform.position como 'posición' lineal

    /// Un ejemplo de cómo construir una propiedad en C#
    /// <summary>
    /// Mass for the NPC
    /// </summary>
    public float Mass
    {
        get { return _mass; }
        set { _mass = Mathf.Max(0, value); }
    }

    // CONSTRUYE LAS PROPIEDADES SIGUENTES. PUEDES CAMBIAR LOS NOMBRE A TU GUSTO
    // Lo importante es controlar el set
    public float MaxForce
    {
        get { return _maxForce; }
        set { _maxForce = Mathf.Max(0, value); } // No podemos tener una fuerza negativa como fuerza max
    }
    public float MaxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = Mathf.Max(0, value); }
    }
    public float Speed
    {
        get { return _speed; }
        set { /*float sp*/_speed = Mathf.Max(0, _maxSpeed);
            /*if (sp < 0.03) _speed = 0f;
            else _speed = sp;*/
        }
    }
    public Vector3 Velocity
    {
        get { return new Vector3(_velocity.x, _velocity.y, _velocity.z); } // Devuelve una copia del vector
        set {  
                /*Vector3 vel*/ _velocity = Vector3.ClampMagnitude(value, _maxSpeed);
                /*if (vel.magnitude < 0.03) _velocity = Vector3.zero;
                else _velocity = vel;*/
        }
    }
    public float MaxRotation
    {
        get { return _maxRotation; }
        set { _maxRotation = Mathf.Max(0, value); }
    }
    public float Rotation
    {
        get { return _rotation; }
        set { /*float rot*/_rotation = Mathf.Clamp(value, -_maxRotation, _maxRotation);
            /*if (rot < 0.01) _rotation = 0f;
            else _rotation = rot;*/
        }
    }
    public float MaxAcceleration
    {
        get { return _maxAcceleration; }
        set { _maxAcceleration = Mathf.Max(0, value); }
    }
    public Vector3 Acceleration
    {
        get { return new Vector3(_acceleration.x, _acceleration.y, _acceleration.z); } // Devuelve una copia del vector
        set { /*Vector3 acc*/_acceleration = Vector3.ClampMagnitude(value, _maxAcceleration);
            /*if (acc.magnitude < 0.03) _acceleration = Vector3.zero;
            else _acceleration = acc;*/
        }       // Solo se acepta una aceleración nueva no mayor a max (magnitud)
    }
    
    public float MaxAngularAcc
    {
        get { return _maxAngularAcc; }
        set { _maxAngularAcc = Mathf.Max(0, value); }
    }

    public float AngularAcc
    {
        get { return _angularAcc; }
        set { /*float ang*/ _angularAcc= Mathf.Clamp(value,-_maxAngularAcc, _maxAngularAcc);
            /*if (ang < 0.05) _angularAcc = 0f;
            else _angularAcc = ang;*/
        } // Podría existir una aceleración angular negativa
    }
    // public Vector3 Position. Recuerda. Esta es la única propiedad que trabaja sobre transform.
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public float Orientation
    {
        get { return _orientation; }
        set { _orientation = MapToRange(value,Range.grados180);
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, _orientation);         
            }
    }

    // TE PUEDEN INTERESAR LOS SIGUIENTES MÉTODOS.
    // Añade todos los que sean referentes a la parte física.

    // public float Heading()
    //      Retorna el ángulo heading en (-180, 180) en grado o radianes. Lo que consideres
    
    // Retorna un ángulo de (-180, 180) a (0, 360) expresado en grado or radianes
    public static float MapToRange(float rotation, Range r) {
        if (r == Range.grados180) {
            rotation = rotation % 360;
            if (rotation <= -180)
                rotation += 360;
            if (rotation > 180)
                rotation -= 360;
        }
        if (r == Range.radianes) {
            rotation = rotation % 360;
            if (rotation <= -180)
                rotation += 360;
            if (rotation > 180)
                rotation -= 360;
            rotation = Mathf.PI * rotation / 180.0f;
        }
        return rotation;
    }
    
    //Retorna la orientación de este bodi, un ángulo de (-180, 180), a (0, 360) expresado en grado or radianes
    public float MapToRange(Range r) {
        float angle = Orientation;
        if (r == Range.radianes) {
            angle = Mathf.PI * angle / 180.0f;
        }
        return angle;
    }
    
    //Retorna el ángulo de una posición usando el eje Z como el primer eje
    public float PositionToAngle() {
        Vector3 ejeZ = Vector3.forward;
        Vector3 pos = Position.normalized;
        //u.v = ||u||*||v||*sin(alpha) (en unity)
        //sin(alpha) = (u.v)/(||u||*||v||)
        float angle = Mathf.Acos(Vector3.Dot(ejeZ,pos)) * 180 / Mathf.PI;
        return angle;
    }

    //target.Position-agent.Position = distance
    public float PositionToAngle(Vector3 distance) {
        Vector3 ejeZ = Vector3.forward;
        Vector3 pos = distance.normalized;
        //u.v = ||u||*||v||*sin(alpha) (en unity)
        //sin(alpha) = (u.v)/(||u||*||v||)
        float angle = Mathf.Acos(Vector3.Dot(ejeZ,pos)) * 180.0f /Mathf.PI;
        return angle;
    }
    //Retorna un vector a partir de una orientación usando Z como primer eje
    public Vector3 OrientationToVector(float angle) {
        angle = MapToRange(angle,Range.radianes);
        float coordX = Mathf.Sin(angle);
        float coordZ = Mathf.Cos(angle);
        Vector3 nuevo = new Vector3(coordX,0,coordZ);
        return nuevo;
    }

    // public Vector3 VectorHeading()  // Nombre alternativo
    //      Retorna un vector a partir de una orientación usando Z como primer eje
    // public float GetMiniminAngleTo(Vector3 rotation)
    //      Determina el menor ángulo en 2.5D para que desde la orientación actual mire en la dirección del vector dado como parámetro
    // public void ResetOrientation()
    //      Resetea la orientación del bodi
    // public float PredictNearestApproachTime(Bodi other, float timeInit, float timeEnd)
    //      Predice el tiempo hasta el acercamiento más cercano entre este y otro vehículo entre B y T (p.e. [0, Mathf.Infinity])
    // public float PredictNearestApproachDistance3(Bodi other, float timeInit, float timeEnd)

}

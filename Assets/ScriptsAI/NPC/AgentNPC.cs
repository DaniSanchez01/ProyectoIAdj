using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum State
{
    Normal,
    Formation,
    leaderFollowing,
    runningToPoint,
    LRTA,
}

public enum Team
{
    Red,
    Blue,
    
}
public class AgentNPC : Agent
{ 
    // Este será el steering final que se aplique al personaje.
    [SerializeField] protected Steering steer;
    // Todos los steering que tiene que calcular el agente.
    private List<SteeringBehaviour> listSteerings;
    public bool useArbitro = false;

    public State agentState = State.Normal;
    public typeArbitro arbitro = typeArbitro.Quieto;  
    private typeArbitro firstArbitro;
    public typePath pathToFollow;
    public Team team;
    public Agent firstTarget;
    private Agent circleVirt;
    private int inicio;
    private bool waiting = false;



    public Agent CircleVirt {
        set { circleVirt = value;}
        get { return circleVirt; }
    }

    public typeArbitro FirstArbitro {
        get { return firstArbitro; }
    }


    protected void Awake()
    {
        this.steer = new Steering();

        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Puedes usar GetComponents<>()
        this.listSteerings = GetComponents<SteeringBehaviour>().ToList();


    }


    // Use this for initialization
    protected virtual void Start()
    {
        this.Velocity = Vector3.zero;
        if (useArbitro) 
            {
                firstArbitro = arbitro;
                listSteerings = GestorArbitros.GetArbitraje(arbitro,this,firstTarget, pathToFollow); //he puesto vagante
            }

    }

    public void startTimer() {
        inicio = Environment.TickCount;
        waiting = true;
    }

    public void NoWait() {
        waiting = false;
    }

    public void finishTimer(){
        if (waiting) {
            if ((Environment.TickCount - inicio) > 10000){
                waiting = false;
                changeArbitro(firstArbitro);
            }
        }
    }

    public void changeArbitro(typeArbitro arb) {
        this.arbitro = arb;
        this.deleteAllSteerings();
        listSteerings = GestorArbitros.GetArbitraje(arb,this,firstTarget, pathToFollow);
    }

    public void deleteAllSteerings() {
        while (gameObject.TryGetComponent<SteeringBehaviour>(out SteeringBehaviour a)) {
            a.DestroyVirtual(firstTarget);
            DestroyImmediate(a);
        }
    }

    public bool checkSteering(string nameSteer) {
        foreach (var s in listSteerings) {
            if (s.NameSteering == nameSteer) {
                return true;
            }
        }
        return false;
    }
    public SteeringBehaviour takeSteering(string nameSteer) {
        foreach (var s in listSteerings) {
            if (s.NameSteering == nameSteer) {
                return s;
            }
        }
        return null;
    }

    public void rodearPunto() {
        changeArbitro(typeArbitro.Posicionar);
        Arrive a = (Arrive) takeSteering("Arrive");
        Align b = (Align) takeSteering("Align");
        a.NewTarget(circleVirt);
        b.NewTarget(circleVirt);
        startTimer();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!useArbitro) this.listSteerings = GetComponents<SteeringBehaviour>().ToList();
        
        finishTimer();

        ApplySteering(Time.deltaTime);

        // En cada frame podría ejecutar otras componentes IA
    }


    private void ApplySteering(float deltaTime)
    {
        // Actualizar las propiedades para Time.deltaTime según NewtonEuler
        Acceleration = this.steer.linear;
        AngularAcc = this.steer.angular;
        Velocity += Acceleration * deltaTime;
        Rotation += AngularAcc * deltaTime; 
        Position += Velocity * deltaTime; 
        Orientation += Rotation * deltaTime; 
        // Pasar los valores Position y Orientation a Unity.
        // Posición no es necesario. Ver observación final.
        transform.rotation = new Quaternion(); //Quaternion.identity;
        transform.Rotate(Vector3.up, Orientation);
        // Ni se te ocurra usar cuaterniones para la rotación.
        // Aquí tienes la solución sin cuaterniones.

    }



    public virtual void LateUpdate()
    {
        Steering kinematicFinal = new Steering();
        //Steering kinematic = new Steering();

        // Reseteamos el steering final.
        this.steer = new Steering();

        
        // Recorremos cada steering
        foreach (SteeringBehaviour behavior in listSteerings)
        {
            Steering steeringActual = behavior.GetSteering(this);
            kinematicFinal.linear = kinematicFinal.linear + steeringActual.linear * behavior.Weight;
            kinematicFinal.angular = kinematicFinal.angular + steeringActual.angular * behavior.Weight;
        }
        //// La cinemática de este SteeringBehaviour se tiene que combinar
        //// con las cinemáticas de los demás SteeringBehaviour.
        //// Debes usar kinematic con el árbitro desesado para combinar todos
        //// los SteeringBehaviour.
        //// Llamaremos kinematicFinal a la aceleraciones finales de esas combinaciones.

        // A continuación debería entrar a funcionar el actuador para comprobar
        // si la propuesta de movimiento es factible:
        // kinematicFinal = Actuador(kinematicFinal, self)


        // El resultado final se guarda para ser aplicado en el siguiente frame.
        this.steer = kinematicFinal;
    }
    
}

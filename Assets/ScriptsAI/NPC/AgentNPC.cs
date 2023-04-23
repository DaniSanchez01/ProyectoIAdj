﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public enum State
{
    //Estados base es decir que se dan para todas las clases de NPC que tengamos
    Normal,
    Formation,
    leaderFollowing,
    runningToPoint,
    LRTA,

    //Estados del soldado
    VigilarSoldier,
    AtacarSoldier,
    HuirSoldier,
    CurarseSoldier,

}

public enum Team
{
    Red,
    Blue,
    
}
public abstract class AgentNPC : Agent
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

    //atributos relacionados con el comportamiento
    [SerializeField] private int vida; //nuevo atributo para saber la vida del personaje
    [SerializeField] private AgentNPC enemigoActual;  //enemigo actual que se ha detectado
    [SerializeField] private bool inmovil; //indica si se queda totalmente inmovil o no debido a que ha atacado
    [SerializeField] private float rangoAtaque; //simboliza el rango de ataque de una unidad
    public bool console = false;


    public Agent CircleVirt {
        set { circleVirt = value;}
        get { return circleVirt; }
    }

    public typeArbitro FirstArbitro {
        get { return firstArbitro; }
    }

    //La vida de un NPC puede ser vista por el resto de clases pero solo modificada por las clases que derivan de el NPC es decir que solo el mismo NPC puede modificarse su vida.
    public int Vida
    {
        get { return vida; }
        protected set { vida = value; }
    }

    //El enemigo actual de un NPC puede ser vista por todas las clases pero esta propiedad solo puede ser modificada por las clases hijas es decir solo un NPC puede establecer a su enemigo actual
    public AgentNPC EnemigoActual
    {
        get { return enemigoActual; }
        protected set { enemigoActual = value; }
    }

    //El NPC actual puede quedarse inmovilizado o no despues de atacar.
    public bool Inmovil
    {
        get { return inmovil; }
        protected set { inmovil = value; }
    }

    //Propiedad que simboliza el rango de ataque de una unidad determinada
    public float RangoAtaque
    {
        get { return rangoAtaque; }
        protected set { rangoAtaque = value; }
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

    public abstract float getTerrainCost(Nodo a);


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

    public void putBocadillo() {
        Transform hijo = transform.Find("Bocadillo");
        hijo  = hijo.Find("Texto");
        TMP_Text t = hijo.GetComponent<TMP_Text>();
        string frase;
        switch (agentState) {
            case(State.Normal):
                frase = "Estoy joya";
                break;
            case(State.Formation):
                frase = "Estoy en formación";
                break;
            case(State.leaderFollowing):
                frase = "Estoy siguiendo al lider";
                break;
            case(State.runningToPoint):
                frase = "Estoy yendo a un punto";
                break;   
            case(State.LRTA):
                frase = "Estoy haciendo A*";
                break;
            case(State.VigilarSoldier):
                frase = "Estoy vigilando";
                break;
            case(State.AtacarSoldier):
                frase = "Al ataque!";
                break;
            case(State.HuirSoldier):
                frase = "No quiero morir!!!";
                break;
            case(State.CurarseSoldier):
                frase = "Necesito vida";
                break;
            default:
                frase = "Que habrá para comer?";
                break;
        }
        t.text = frase;
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

        putBocadillo();
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
            if (behavior != null)
            {
                Steering steeringActual = behavior.GetSteering(this);
                kinematicFinal.linear = kinematicFinal.linear + steeringActual.linear * behavior.Weight;
                kinematicFinal.angular = kinematicFinal.angular + steeringActual.angular * behavior.Weight;
            }
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

    //metodos relacionados con el comportamiento

    /*
     * Metodo que dada una cantidad de daño la resta a la vida del personaje.
     * Pre:ninguna
     * Post:devuelve la vida del NPC despues de recibir el daño
     */
    public virtual int recibirDamage(int cantidad)
    {
        vida = vida - cantidad;
        if (vida < 0) vida = 0;
        return vida;
    }

    /*
     * Comprueba si el NPC no tiene vida
     * Pre: ninguna
     * Post: devuelve un verdadero si su vida == 0 y falso en caso contrario
     */
    public bool estaMuerto()
    {
        return vida == 0;
    }

    /*
     * Metodo que comprueba si el enemigo pasado como parametro esta en el rango del NPC.
     * Pre: el "enemigo" pasado como paramtro debe tener un equipo distinto al NPC
     * Post: devuelve verdadero o falso segun si el NPC ve al enemigo o no.
     */
    public bool sigoViendoEnemigo(AgentNPC enemigo)
    {
        return Vector3.Distance(enemigo.Position, this.Position) <= this.arrivalRadius;
    }

    //Comprueba si el enemigo Actual esta en nuestro rango de ataque para atacarle
    public bool estaARangoEnemigoAct()
    {
        if (EnemigoActual != null && Vector3.Distance(EnemigoActual.Position, this.Position) <= rangoAtaque) return true;
        else return false; //observar que puede retornar false porque no haya un enemigo o este no este a rango.
    }

    /*
     * Funcion que nos permite localizar un enemigo en un radio determinado dado por el radio interior.
     * Pre: ninguna
     * Post: Devuelve el componente AgentNPC del enemigo detectado
     */
    public AgentNPC veoEnemigo()
    {
        Collider[] colisiones = Physics.OverlapSphere(this.Position, this.arrivalRadius);

        foreach (Collider obj in colisiones)
        {
            AgentNPC componenteNPC = obj.GetComponent<AgentNPC>();

            if (componenteNPC != null && !componenteNPC.team.Equals(this.team) && (Vector3.Distance(componenteNPC.Position, this.Position) <= this.arrivalRadius))
                return componenteNPC;
        }
        return null;
    }


    /*
    * Corutina que es usada para que un personaje ataque, primero si el personaje tiene el enemigo a rango y por tanto le ataca se esperar 2 segundos quedandose inmovil. Observar que este metodo aunque
    * se tiene que iniciar manualmente parara solo cuando se salga del estado "atacarSoldier" as� que no es necesario pararlo manualmente.
    * Pre: se debe haber establecido el estado del NPC a "ataqueSoldier" y enemigoActual != null
    * Post: atacada cada 2 segundos si el enemigo detectado esta en su rango.
    * 
    */
    public IEnumerator atacar()
    {
        if (console) Debug.Log("Corutina atacar() comienzo");
        while (true)
        {
            //1. La corutina comprueba que el enemigo no esta muerto y que el NPC lo tiene a rango
            if (!EnemigoActual.estaMuerto() && estaARangoEnemigoAct())
            {
                //1.1 Cuando ataca inflinge dano e inmovilizate 2 segundos
                if (console) Debug.Log("Atacar");
                EnemigoActual.recibirDamage(3);
                //quedate quieto durante 2 segundos
                Inmovil = true; //quedate quieto
                this.Acceleration = Vector3.zero;
                this.AngularAcc = 0;
                this.Velocity = Vector3.zero;
                this.Rotation = 0;
                yield return new WaitForSeconds(2); //Esperate 2 segundos quieto

                //1.2 Despues de haber esperado indicamos que ya se puede mover
                Inmovil = false;
            }

            //2. Si se ha ejecutado el if entonces la rutina se suspende hasta el siguiente frame para dejar que el update() se ejecute una vez y asi permitir transiciionar si el personaje tiene poca vida
            //en caso contrario de que no se haya ejecutado el if pues la corutina se ejecutara en el siguiente frame para que asi no caigamos en un bucle infinito y dejemos que continue la ejecucion de otras
            //funciones como los update() del siguiente frame.
            yield return null;
        }
        // if (console) Debug.Log("Fin de la corutina atacar()");
    }
}

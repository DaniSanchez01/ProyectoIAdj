using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoldierAgentNPC : AgentNPC
{
    private AgentNPC enemigoActual; //enemigo actual que se ha detectado
    [SerializeField] private bool inmovil; //indica si se queda totalmente inmovil o no debido a que ha atacado
    [SerializeField] private IEnumerator coataque; //corutina de ataque que solo se activara cuando se este en modo ataque.
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 8f;
        this.MaxAcceleration = 20f;
        this.MaxRotation = 180f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 300f;
        this.interiorAngle = 8f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        base.Start();

        agentState = State.VigilarSoldier; //el estado normal del soldier
        Vida = 200;
        inmovil = false;
        coataque = atacar(); //guarda un identificador que distingue a una instancia de la corutina atacar()
       
    }

    /*
     * Funcion que nos permite localizar un enemigo en un radio determinado dado por el radio interior.
     * Pre: ninguna
     * Post: Devuelve el componente AgentNPC del enemigo detectado
     */
    private AgentNPC veoEnemigo()
    {
       Collider[] colisiones =  Physics.OverlapSphere(this.Position, this.arrivalRadius);

        foreach (Collider obj in colisiones)
        {
            AgentNPC componenteNPC = obj.GetComponent<AgentNPC>();

            if (componenteNPC != null &&  !componenteNPC.team.Equals(this.team) && (Vector3.Distance(componenteNPC.Position, this.Position) <= this.arrivalRadius)) 
                return componenteNPC;
        }
        return null;
    }
    

    /*
     * Comprueba si un enemigo detectado anteriormente sigue en tu rango de vision.
     * Pre: debes estar en estado = "VigilarAtaque" y enemigoActual!=null
     */
    private bool sigoViendoEnemigoAct()
    {
        
        return Vector3.Distance(enemigoActual.Position, this.Position) <= this.arrivalRadius;
    }

    /*
     * Comprueba si el enemigo al que se esta atacando esta en el rango para poder darle un golpe.
     * Pre: debes tener algun enemigo seleccionado
     * Post: devueleve verdadero si esta en el rango de ataque y falso en caso contrario.
     */
    private bool estaARangoEnemigo()
    {
        if (enemigoActual != null && Vector3.Distance(enemigoActual.Position, this.Position) <= 0.8) return true;
        else return false; //observar que puede retornar false porque no haya un enemigo o este no este a rango.
        
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
        Debug.Log("Corutina atacar() comienzo");
        while (agentState == State.AtacarSoldier)
        {
            //1. La corutina comprueba que el enemigo no esta muerto y que el NPC lo tiene a rango
            if (!enemigoActual.estaMuerto() && estaARangoEnemigo())
            {
                //1.1 Cuando ataca inflinge dano e inmovilizate 2 segundos
                Debug.Log("Atacar");
                enemigoActual.recibirDamage(3);
                //quedate quieto durante 2 segundos
                inmovil = true; //quedate quieto
                this.Acceleration = Vector3.zero;
                this.AngularAcc = 0;
                this.Velocity = Vector3.zero;
                this.Rotation = 0;
                yield return new WaitForSeconds(2); //Esperate 2 segundos quieto

                //1.2 Despues de haber esperado indicamos que ya no se puede mover
                inmovil = false;
            }

            //2. En otro caso es decir si el enemigo esta muerto
            //o no esta en mi rango lo intento el siguiente frame
            else yield return null;
        }
        Debug.Log("Fin de la corutina atacar()");
    }
    

    /*
     * Metodo que es usado para entrar en un estado determinado, ejecuta las acciones de entrada y cambia el estadoActual al estado indicado como parametro. Recibe parametros que pueden
     * ser usados o no
     * Pre: ninguna
     * Post: estadoAgent = estadoAEntrar, se han ejecutado las acciones de entrada de estadoAEntrar.
     */
    private void entrar(State estadoAEntrar)
    {
        switch (estadoAEntrar)
        {
            case State.VigilarSoldier:
                Debug.Log("Entrando en el estado de vigilar");
                agentState = estadoAEntrar;
                break;
            case State.AtacarSoldier:
                Debug.Log("Entrando en el estado de atacar");
                
                agentState = estadoAEntrar;
                GestorArbitros.GetArbitraje(typeArbitro.Perseguidor, this, enemigoActual, pathToFollow); //indicamos al enemigo que sigue para atacarle
                StartCoroutine(coataque);
                break;

            default:
                Debug.Log("No se conoce el estado asi que no se entra en ningun estado");
                break;
        }
    }
    /*
     * Metodo que se llama para salir del estado actual.
     * Pre: ninguna
     * Post: ejecuta las acciones necesarias para salir del estado actual
     */
    private void salir(State estadoActual)
    {
        switch (estadoActual)
        {
            case State.VigilarSoldier:
                Debug.Log("Saliendo del estado de vigilar");
                this.deleteAllSteerings(); //se eliminan los steerings al salir del estado de "ataque"
                break;
            case State.AtacarSoldier:
                Debug.Log("Saliendo del estado de atacar");
                StopCoroutine(coataque); //se para la rutina de ataque
                inmovil = false;
                this.deleteAllSteerings(); //se eliminan los steerings al salir del estado de "ataque"
                break;
           
            default:
                Debug.Log("No se conoce el estado asi que no se sale de ese estado");
                break;
        }
    }

    

    /*
     * Este metodo es usado para comporbar si se puede transitar a algun estado desde el estado actual.
     * Pre: ninguna
     * Post: si no se puede transitar a ningun estado desde el estado actual entonces no se hace nada, en caso contrario se sale del estado actual y se entra en el estado destino.
     */
    private void transitar(State estadoAct)
    {
        switch(estadoAct)
        {
            case State.VigilarSoldier:
                
                //accion asociada al estado vigilar
                enemigoActual = veoEnemigo();

                if(enemigoActual) //1 transicion de vigilarSoldier
                {
                    
                    salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                    entrar(State.AtacarSoldier); //voy a entrar en ataque y digo el enemigo que he detectado
                }
                break;
            case State.AtacarSoldier:

                /*
                Debug.Log("Posicion actual enemigo" + enemigoActual.Position);
                Debug.Log("Posicion actual NPC soldier" + this.Position);
                Debug.Log("Distancia entre puntos: " + Vector3.Distance(enemigoActual.Position, this.Position));
                */
                if ((enemigoActual.estaMuerto() || !sigoViendoEnemigoAct()))
                {
                    
                    salir(estadoAct);
                    entrar(State.VigilarSoldier);
                }
                break;
            default:
                break;
        }

    }

    

    // Update is called once per frame
    public override void Update()
    {
        if (!inmovil) //si no estas inmovil puedes actualizar 
        {
           
            base.Update(); //dejamos que se ejecute la lista de steerings que tenemos actualmente
            transitar(agentState); //Intentamos cambiar transicionar desde el estado actual
        }
        
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected  override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, (float) 0.8);
    }


}

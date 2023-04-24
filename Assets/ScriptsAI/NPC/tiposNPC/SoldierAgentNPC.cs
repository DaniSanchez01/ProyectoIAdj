using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoldierAgentNPC : AgentNPC
{
   
    
    
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
        Inmovil = false;
        RangoAtaque = 0.8f;
        CoAtaque = atacar(); //guarda un identificador que distingue a una instancia de la corutina atacar()
        modoNPC = Modo.Defensivo; //al principio los NPC comenzaran en un modo defensivo
    }

    public override float getTerrainCost(Nodo a) {
            
            TypeTerrain t = GameObject.FindObjectOfType<TerrainMap>().getTerrenoCasilla(a.Celda.x,a.Celda.y);
            switch (t) {
                case (TypeTerrain.camino):
                    return 1;
                case (TypeTerrain.llanura):
                    return 2;
                case (TypeTerrain.bosque):
                    return 3;
                case (TypeTerrain.desierto):
                    return 5;
                default:
                    return 9999;             
            }

    }

    

    
   
    

    
    public override void entrar(State estadoAEntrar)
    {
        switch (estadoAEntrar)
        {
            case State.VigilarSoldier:
                if (console) Debug.Log("Entrando en el estado de vigilar");
                //Aqui podemos hacer que siga alguna ruta o que haga wander para buscar enemigo segun el tipo de modo en el que este
                agentState = estadoAEntrar;
                break;
            case State.WanderSoldier:
                if (console) Debug.Log("Entrando en el estado WanderSoldier");
                GestorArbitros.GetArbitraje(typeArbitro.Aleatorio, this, EnemigoActual, pathToFollow);
                agentState = estadoAEntrar;
                break;
            case State.AtacarSoldier:
                if (console) Debug.Log("Entrando en el estado de atacar");
                
                agentState = estadoAEntrar;
                GestorArbitros.GetArbitraje(typeArbitro.Perseguidor, this, EnemigoActual, pathToFollow); //indicamos al enemigo que sigue para atacarle
                StartCoroutine(CoAtaque);
                break;

            case State.HuirSoldier:
                if (console) Debug.Log("Entrando en el estado de huir");
                agentState = estadoAEntrar;
                GestorArbitros.GetArbitraje(typeArbitro.Huidizo, this, EnemigoActual, pathToFollow); //indicamos que queremos que huya del enemigo actual.
                break;
            case State.CurarseSoldier:
                if (console) Debug.Log("Entrando en el estado de curarse");
                agentState = estadoAEntrar;
                GestorArbitros.GetArbitraje(typeArbitro.Aleatorio, this, null, pathToFollow); //habria que cambiarlo con seguir a algun NPC del mapa
                break;
            default:
                if (console) Debug.Log("No se conoce el estado asi que no se entra en ningun estado");
                break;
        }
    }
    

    public override void salir(State estadoActual)
    {
        switch (estadoActual)
        {
            case State.VigilarSoldier:
                if (console) Debug.Log("Saliendo del estado de vigilar");
                this.deleteAllSteerings(); //se eliminan los steerings al salir del estado de "ataque"
                break;
            case State.WanderSoldier:
                if (console) Debug.Log("Saliendo del estado de Wander");
                this.deleteAllSteerings();
                break;
            case State.AtacarSoldier:
                if (console) Debug.Log("Saliendo del estado de atacar");
                StopCoroutine(CoAtaque); //se para la rutina de ataque
                Inmovil = false;
                this.deleteAllSteerings(); //se eliminan los steerings al salir del estado de "ataque"
                break;
            case State.HuirSoldier:
                if (console) Debug.Log("Saliendo del estado de huir");
                this.deleteAllSteerings(); //se eliminan los steerings al salir del estado de "huir"
                break;
            case State.CurarseSoldier:
                if (console) Debug.Log("Saliendo del estado de curarse");
                this.deleteAllSteerings();
                break;
            default:
                if (console) Debug.Log("No se conoce el estado asi que no se sale de ese estado");
                break;
        }
    }

    

    
    public override void transitar(State estadoAct)
    {
        //Si esta defendiendo se usa el automata que especifica el comportamiento de defensa
        if (estaDefendiendo())
        {
            switch (estadoAct)
            {
                case State.VigilarSoldier:

                    //accion asociada al estado vigilar aparte de los steerings correspondientes.
                    EnemigoActual = veoEnemigo();

                    //1. La primera transición del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    if (EnemigoActual) //1 transicion de vigilarSoldier
                    {

                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.AtacarSoldier); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;
                case State.AtacarSoldier:

                    //1. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    if (Vida <= 20) //si nos falta vida huimos
                    {
                        salir(estadoAct);
                        entrar(State.HuirSoldier);
                    }

                    //2. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        salir(estadoAct);
                        entrar(State.VigilarSoldier);
                    }

                    //en otro caso pues no se hace nada y se ejecutaria cada cierto tiempo la rutina atacar()
                    break;

                case State.HuirSoldier:

                    //1. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                    if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.CurarseSoldier);
                    }
                    break;
                case State.CurarseSoldier:
                    //1. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado vigilar
                    if (Vida == 200)
                    {
                        salir(estadoAct);
                        entrar(State.VigilarSoldier);
                    }
                    break;
                default:
                    break;
            }
        }

        //En otro caso se aplica el automata de ataque
        else
        {
            switch (estadoAct)
            {
                case State.WanderSoldier:

                    //accion asociada al estado Wander aparte de los steerings correspondientes.
                    EnemigoActual = veoEnemigo();

                    //1. La primera transición del estado Wander se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    if (EnemigoActual) //1 transicion de WanderSoldier
                    {

                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.AtacarSoldier); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;
                case State.AtacarSoldier:

                    //1. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    if (Vida <= 20) //si nos falta vida huimos
                    {
                        salir(estadoAct);
                        entrar(State.HuirSoldier);
                    }

                    //2. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de Wander.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        salir(estadoAct);
                        entrar(State.WanderSoldier);
                    }

                    //en otro caso pues no se hace nada y se ejecutaria cada cierto tiempo la rutina atacar()
                    break;

                case State.HuirSoldier:

                    //1. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                    if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.CurarseSoldier);
                    }
                    break;
                case State.CurarseSoldier:
                    //1. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado Wander
                    if (Vida == 200)
                    {
                        salir(estadoAct);
                        entrar(State.WanderSoldier);
                    }
                    break;
                default:
                    break;
            }
        }

    }

    

    // Update is called once per frame
    public override void Update()
    {
        if (!Inmovil) //si no estas inmovil puedes actualizar 
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
        Gizmos.DrawWireSphere(transform.position, (float) RangoAtaque);
    }


}

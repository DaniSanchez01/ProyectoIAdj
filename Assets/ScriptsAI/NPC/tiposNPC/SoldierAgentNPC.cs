using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoldierAgentNPC : AgentNPC
{
   
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 5f;
        this.MaxAcceleration = 16f;
        this.MaxRotation = 180f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 300f;
        this.interiorAngle = 8f;
        this.influencia = 0.7f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        agentState = State.Vigilar; //el estado normal del soldier
        Vida = 200;
        VidaMax = 200;
        Inmovil = false;
        RangoAtaque = 1.2f;
        CoAtaque = atacar(); //guarda un identificador que distingue a una instancia de la corutina atacar()
        modoNPC = Modo.Defensivo; //al principio los NPC comenzaran en un modo defensivo
        entrar(State.Vigilar);
        base.Start();


    }

    public override float getTerrainCost(Nodo a) {
            
            TypeTerrain t = mapaTerrenos.getTerrenoCasilla(a.Celda.x,a.Celda.y);
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

    

    
   
    

  

    
    public override void transitar(State estadoAct)
    {
        //Si esta defendiendo se usa el automata que especifica el comportamiento de defensa
        if (estaDefendiendo())
        {
            //if (console) Debug.Log(estadoAct);
            switch (estadoAct)
            {
                case State.Vigilar:

                    //accion asociada al estado vigilar aparte de los steerings correspondientes.
                   

                    //1. La primera transición del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    if(veoEnemigo()) //1 transicion de vigilarSoldier
                    {

                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;
                case State.Atacar:

                    //1. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    if (Vida <= 20) //si nos falta vida huimos
                    {
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //2. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }

                    //en otro caso pues no se hace nada y se ejecutaria cada cierto tiempo la rutina atacar()
                    break;

                case State.Huir:
                    //1. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                    if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.BuscandoCuracion);
                    }
                    break;
                case State.BuscandoCuracion:
                    if (haLlegadoADestino(puntoInteres)) {
                        salir(estadoAct);
                        entrar(State.Curandose);
                    }
                    break;
                case State.Curandose:
                    //1. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado vigilar
                    if (Vida == 200)
                    {
                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }
                    break;
                case State.RecorriendoCamino:
                    if (haLlegadoADestino(puntoInteres)) {
                        FindObjectOfType<LectorTeclado>().clearList(this);
                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }
                    else if(veoEnemigo()) {
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            entrar(State.Atacar);
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
                case State.Conquistar:

                    //accion asociada al estado Wander aparte de los steerings correspondientes.
                    
                    //1. La primera transición del estado Wander se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    if(veoEnemigo()) //1 transicion de WanderSoldier
                    {

                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;
                case State.Atacar:

                    //1. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    if (Vida <= 20) //si nos falta vida huimos
                    {
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //2. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de Wander.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        salir(estadoAct);
                        entrar(State.Conquistar);
                    }

                    //en otro caso pues no se hace nada y se ejecutaria cada cierto tiempo la rutina atacar()
                    break;

                case State.Huir:

                    //1. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                    if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.BuscandoCuracion);
                    }
                    break;
                case State.BuscandoCuracion:
                    if (haLlegadoADestino(puntoInteres)) {
                        salir(estadoAct);
                        entrar(State.Curandose);
                    }
                    break;
                case State.Curandose:
                    //1. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado Wander
                    if (Vida == 200)
                    {
                        salir(estadoAct);
                        entrar(State.Conquistar);
                    }
                    break;
                case State.RecorriendoCamino:
                    if (haLlegadoADestino(puntoInteres)) {
                        FindObjectOfType<LectorTeclado>().clearList(this);
                        salir(estadoAct);
                        entrar(State.Conquistar);
                    }
                    else if(veoEnemigo()) {
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            entrar(State.Atacar);
                        }
                    break;
                default:
                    break;
            }
        }

    }

    protected override int calculateDamage() {
        float terrainMultiplier = 1f;
        float enemyMultiplier = 1f;
        if(EnemigoActual is SoldierAgentNPC) {
            enemyMultiplier = 1f;
        }
        else if (EnemigoActual is ArchierAgentNPC) {
            enemyMultiplier = 1.5f;
        }
        else enemyMultiplier = 0.75f;
        
        Vector2Int celdaActual = grid.getCeldaDePuntoPlano(this.Position);
        TypeTerrain t = mapaTerrenos.getTerrenoCasilla(celdaActual.x,celdaActual.y);
        switch (t) {
            case TypeTerrain.camino:
                terrainMultiplier = 1.75f;
                break;
            case TypeTerrain.llanura:
                terrainMultiplier = 1.5f;
                break;
            case TypeTerrain.bosque:
                terrainMultiplier = 0.5f;
                break;
            case TypeTerrain.desierto:
                terrainMultiplier = 0.5f;
                break;
            default:
                break;
        }
        int realDamage = (int) System.Math.Round(baseDamage * enemyMultiplier * terrainMultiplier);
        return realDamage;
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

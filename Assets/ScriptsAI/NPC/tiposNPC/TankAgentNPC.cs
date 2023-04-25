using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAgentNPC : AgentNPC
{


    /*
     * Este personaje representaria un personaje pesado y que por tanto tiene mucha masa y poca aceleraci�n aunque se puede seguir moviendo
     */

    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 5f;
        this.MaxAcceleration = 15f;
        this.MaxRotation = 140f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 180f;
        this.interiorAngle = 8f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        base.Start();

        //atributos inicializados del tanque
        agentState = State.VigilarTanque;
        Vida = 300;
        Inmovil = false;
        RangoAtaque = 0.6f;
        CoAtaque = atacar();
        modoNPC = Modo.Defensivo;


    }

    public override float getTerrainCost(Nodo a) {
            
            TypeTerrain t = GameObject.FindObjectOfType<TerrainMap>().getTerrenoCasilla(a.Celda.x,a.Celda.y);
            switch (t) {
                case (TypeTerrain.camino):
                    return 1;
                case (TypeTerrain.llanura):
                    return 3;
                case (TypeTerrain.bosque):
                    return 5;
                case (TypeTerrain.desierto):
                    return 3;
                default:
                    return 9999;             
            }

    }


    public override void entrar(State estadoAEntrar)
    {
        switch(estadoAEntrar)
        {
            case State.VigilarTanque:
                if (console) Debug.Log("Entrando en el estado VigilarTanque");
                //aqui podemos poner un arrive al punto a vigilar o una ruta
                agentState = estadoAEntrar;
                break;
            case State.ConquistarTanque:
                if (console) Debug.Log("Entrando en el estado de ConquistarTanque");
                //aqui podemos poner un punto a conquistar
                agentState = estadoAEntrar;
                break;
            case State.AtacarTanque:
                if (console) Debug.Log("Entrando en el estado AtacarTanque");
                agentState = estadoAEntrar;
                GestorArbitros.GetArbitraje(typeArbitro.Perseguidor, this, EnemigoActual, pathToFollow);
                StartCoroutine(CoAtaque);
                break;

            case State.HuirTanque:
                if (console) Debug.Log("Entrando en el estado de huirTanque");
                agentState = estadoAEntrar;
                GestorArbitros.GetArbitraje(typeArbitro.Huidizo, this, EnemigoActual, pathToFollow);
                break;

            case State.CurarTanque:
                if (console) Debug.Log("Entrando en el estado de CurarTanque");
                agentState = estadoAEntrar;
                //aqui podemos poner un arrive a algun waypoint de curacion o algun pathfollowing
                break;

            case State.Berserker:
                if (console) Debug.Log("Entrando en modo Berserker");
                GestorArbitros.GetArbitraje(typeArbitro.Perseguidor, this, EnemigoActual, pathToFollow);
                agentState = estadoAEntrar;
                StartCoroutine(CoAtaque);
                break;
            case State.ConquistarBerserker:
                if (console) Debug.Log("Entrando en modo ConquistarBerserker");
                GestorArbitros.GetArbitraje(typeArbitro.Perseguidor, this, EnemigoActual, pathToFollow);
                agentState = estadoAEntrar;
                break;
            default:
                if (console) Debug.Log("No se conoce el estado asi que no se entra en ningun estado");
                break;

        }
    }

    public override void salir(State estadoAEntrar)
    {
        switch (estadoAEntrar)
        {
            case State.VigilarTanque:
                if (console) Debug.Log("Saliendo del estado VigilarTanque");
                this.deleteAllSteerings();
                break;
            case State.ConquistarTanque:
                if (console) Debug.Log("Saliendo del estado ConquistarTanque");
                this.deleteAllSteerings();
                break;
            case State.AtacarTanque:
                if (console) Debug.Log("Saliendo del estado AtacarTanque");
                StopCoroutine(CoAtaque); //paras de atacar
                this.deleteAllSteerings(); //eliminas steerings
                Inmovil = false; //ya no estas inmovil
                break;

            case State.HuirTanque:
                if (console) Debug.Log("Saliendo del estado de huirTanque");
                this.deleteAllSteerings();
                break;

            case State.CurarTanque:
                if (console) Debug.Log("Saliendo del estado de CurarTanque");
                this.deleteAllSteerings();
                break;

            case State.Berserker:
                if (console) Debug.Log("Saliendo del estado Berserker");
                StopCoroutine(CoAtaque);
                this.deleteAllSteerings();
                Inmovil = false;
                break;
            case State.ConquistarBerserker:
                if (console) Debug.Log("Saliendo del estado ConquistarBerserker");
                this.deleteAllSteerings();
                break;
            default:
                if (console) Debug.Log("No se conoce el estado asi que no se entra en ningun estado");
                break;

        }
    }



    public override void transitar(State estadoAct)
    {
        //si esta defendiendo se ejecuta el automata de defensa
        if(estaDefendiendo())
        {
            switch(estadoAct)
            {
                case State.VigilarTanque:
                    //accion asociada al estado VigilarTanque
                    EnemigoActual = veoEnemigo();

                    //1. Transicion del estado VigilarTanque
                    if(EnemigoActual)
                    {
                        salir(estadoAct);
                        entrar(State.AtacarTanque);
                    }
                    break;
                case State.AtacarTanque:

                    //1. La primera transicion para el tanque es comprobar si le queda poca vida para huir
                    if(Vida <= 40)
                    {
                        salir(estadoAct);
                        entrar(State.HuirTanque);
                    }

                    //2. Si tenemos vida suficiente pero no vemos al enemigo o esta muerto o ambas pues volvemos al estado Wander
                    else if(EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual))
                    {
                        salir(estadoAct);
                        entrar(State.VigilarTanque);
                    }
                    break;
                case State.HuirTanque:

                    //1. Si el enemigo que me esta siguiendo ha muerto o ya no me ve voy a curarme
                    if(EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.CurarTanque);
                    }
                    break;

                case State.CurarTanque:
                    
                    //1. Si tengo la vida maxima ya puedo volver a buscar enemigos
                    if(Vida == 300)
                    {
                        salir(estadoAct);
                        entrar(State.VigilarTanque);
                    }
                    break;
                default:
                    break;

            }
        }

        //automata de ataque que se ejecuta si esta en modo ofensivo
        else { 
        switch(estadoAct)
            {
                case State.ConquistarTanque:
                    //accion asociada al estado ConquistarTanque
                    EnemigoActual = veoEnemigo();

                    //1. Transicion del estado ConquistarTanque
                    if (EnemigoActual)
                    {
                        salir(estadoAct);
                        entrar(State.AtacarTanque);
                    }
                    break;
                case State.AtacarTanque:
                    //1. La primera transicion para el tanque es comprobar si le queda poca vida para huir
                    if (Vida <= 40)
                    {
                        salir(estadoAct);
                        entrar(State.Berserker);
                    }
                    //2. Si tenemos vida suficiente pero no vemos al enemigo o esta muerto o ambas pues volvemos al estado Wander
                    else if (EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual))
                    {
                        salir(estadoAct);
                        entrar(State.ConquistarTanque);
                    }
                    break;
                case State.Berserker:

                    //si el enemigo actual esta muerto o no lo sigo viendo pues paso a hacer un wander con la diferencia de que en este busco aliados y enemigos
                    if(EnemigoActual.estaMuerto() ||!sigoViendoEnemigo(EnemigoActual))
                    {
                        salir(estadoAct);
                        entrar(State.ConquistarBerserker);
                    }
                    break;

                case State.ConquistarBerserker:
                    EnemigoActual = veoEnemigo();

                    //1.Transicion al estado berserker
                    if(EnemigoActual) {
                        salir(estadoAct);
                        entrar(State.Berserker);
                    }
                    EnemigoActual = veoAliado();

                    //2. Transicion al estado berserker si veo un aliado lo ataco
                    if(EnemigoActual)
                    {
                        salir(estadoAct);
                        entrar(State.Berserker);
                    }

                    break;

            }
        }
    }


    // Update is called once per frame
    public  override void Update()
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, (float)RangoAtaque);
    }
}

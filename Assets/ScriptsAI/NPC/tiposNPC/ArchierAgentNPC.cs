using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchierAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 10f;
        this.MaxAcceleration = 30f;
        this.MaxRotation = 180f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 300f;
        this.interiorAngle = 8f;
        this.influencia = 0.4f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        //inicializacion de atributos
        agentState = State.Vigilar;
        Vida = 150;
        Inmovil = false;
        RangoAtaque = 1.5f;
        CoAtaque = atacar();
        modoNPC = Modo.Defensivo;
        base.Start();

        

    }

    public override float getTerrainCost(Nodo a) {
            
            TypeTerrain t = GameObject.FindObjectOfType<TerrainMap>().getTerrenoCasilla(a.Celda.x,a.Celda.y);
            switch (t) {
                case (TypeTerrain.camino):
                    return 1;
                case (TypeTerrain.llanura):
                    return 2;
                case (TypeTerrain.bosque):
                    return 1;
                case (TypeTerrain.desierto):
                    return 5;
                default:
                    return 9999;             
            }

    }




    public override void transitar(State estadoAct)
    {
        //Automata de defensa
        if (estaDefendiendo())
        {
            switch (estadoAct)
            {
                case State.Vigilar:
                    //accion asociada al estado vigilar aparte de los steerings correspondientes.
                    EnemigoActual = veoEnemigo();

                    //1. La primera transicion del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    if (EnemigoActual) //1 transicion de Vigilar
                    {
                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;

                case State.Atacar:

                    //1. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    if (Vida <= 50) //si nos falta vida huimos
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
                    break;
                case State.Huir:
                    //1. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                    if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.Curarse);
                    }
                    break;
                case State.Curarse:
                    //1. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado vigilar
                    if (Vida == 150)
                    {
                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }
                    break;
                default:
                    break;
            }
        }

        //Automata de ataque
        else
        {
            switch (estadoAct)
            {
                case State.Conquistar:
                    //accion asociada al estado vigilar aparte de los steerings correspondientes.
                    EnemigoActual = veoEnemigo();

                    //1. La primera transicion del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    if (EnemigoActual) //1 transicion de Vigilar
                    {
                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;

                case State.Atacar:

                    //1. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    if (Vida <= 50) //si nos falta vida huimos
                    {
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //2. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        salir(estadoAct);
                        entrar(State.Conquistar);
                    }
                    break;
                case State.Huir:
                    //1. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                    if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.Curarse);
                    }
                    break;
                case State.Curarse:
                    //1. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado vigilar
                    if (Vida == 150)
                    {
                        salir(estadoAct);
                        entrar(State.Conquistar);
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
        if (!Inmovil)
        {
            base.Update();
            transitar(agentState);
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

}

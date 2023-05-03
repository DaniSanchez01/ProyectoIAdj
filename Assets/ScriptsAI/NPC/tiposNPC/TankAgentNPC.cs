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
        this.MaxSpeed = 3f;
        this.MaxAcceleration = 12f;
        this.MaxRotation = 140f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 180f;
        this.interiorAngle = 8f;
        this.influencia = 1f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        //atributos inicializados del tanque
        agentState = State.Vigilar;
        Vida = 300;
        Inmovil = false;
        RangoAtaque = 0.6f;
        CoAtaque = atacar();
        modoNPC = Modo.Defensivo;
        base.Start();




    }

    public override float getTerrainCost(Nodo a) {
            
            TypeTerrain t = mapaTerrenos.getTerrenoCasilla(a.Celda.x,a.Celda.y);
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


    




    public override void transitar(State estadoAct)
    {
        //si esta defendiendo se ejecuta el automata de defensa
        if(estaDefendiendo())
        {
            switch(estadoAct)
            {
                case State.Vigilar:
                    //accion asociada al estado Vigilar
                    EnemigoActual = veoEnemigo();

                    //1. Transicion del estado Vigilar
                    if(EnemigoActual)
                    {
                        salir(estadoAct);
                        entrar(State.Atacar);
                    }
                    break;
                case State.Atacar:

                    //1. La primera transicion para el tanque es comprobar si le queda poca vida para huir
                    if(Vida <= 40)
                    {
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //2. Si tenemos vida suficiente pero no vemos al enemigo o esta muerto o ambas pues volvemos al estado Wander
                    else if(EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual))
                    {
                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }
                    break;
                case State.Huir:

                    //1. Si el enemigo que me esta siguiendo ha muerto o ya no me ve voy a curarme
                    if(EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.Curarse);
                    }
                    break;

                case State.Curarse:
                    
                    //1. Si tengo la vida maxima ya puedo volver a buscar enemigos
                    if(Vida == 300)
                    {
                        salir(estadoAct);
                        entrar(State.Vigilar);
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
                case State.Conquistar:
                    //accion asociada al estado Conquistar
                    EnemigoActual = veoEnemigo();

                    //1. Transicion del estado Conquistar
                    if (EnemigoActual)
                    {
                        salir(estadoAct);
                        entrar(State.Atacar);
                    }

                    //2.Si tu vida<=40 y ves un aliado
                    else if(Vida<=40)
                    {
                        EnemigoActual = veoAliado();
                        if(EnemigoActual)
                        {
                            salir(estadoAct);
                            entrar(State.Atacar);
                        }
                    }

                    //en otro caso no haces nada

                    break;
                case State.Atacar:
                    //1. Si tenemos vida suficiente pero no vemos al enemigo o esta muerto o ambas pues volvemos al estado Wander
                    if (EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual))
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

    protected override void revivir() {
        Vida = 300;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, (float)RangoAtaque);
    }
}

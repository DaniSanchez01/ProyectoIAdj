using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchierAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 7f;
        this.MaxAcceleration = 20f;
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
        VidaMax = 150;
        Inmovil = false;
        RangoAtaque = 1.5f;
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

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }


                    //2. La primera transicion del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                   else  if(veoEnemigo()) //1 transicion de Vigilar
                    {
                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;

                case State.Atacar:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    else if (Vida <= 50) //si nos falta vida huimos
                    {
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //3. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }
                    break;
                case State.Huir:


                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                   else if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.BuscandoCuracion);
                    }
                    break;
                case State.BuscandoCuracion:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }
                    //2. si he llegado
                    else if (haLlegadoADestino(puntoInteres)) {
                        salir(estadoAct);
                        entrar(State.Curandose);
                    }
                    break;
                case State.Curandose:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado vigilar
                    else if (Vida == 150)
                    {
                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }
                    break;
                case State.Muerto:
                    //1. si la corutina reaparecer() ya se ha ejecutado despues de los 5 segundos
                    if (Vida == VidaMax)
                    {
                        salir(estadoAct);
                        entrar(State.RecorriendoCamino);
                    }
                    break;
                case State.RecorriendoCamino:
                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. si ha llegado al destino
                    else if (haLlegadoADestino(puntoInteres)) {
                        FindObjectOfType<LectorTeclado>().clearList(this);
                        salir(estadoAct);
                        entrar(State.Vigilar);
                    }
                    //3. si veo algun enemigo
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

        //Automata de ataque
        else
        {
            switch (estadoAct)
            {
                case State.Conquistar:
                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }


                    //2. La primera transicion del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    else if (veoEnemigo()) //1 transicion de Vigilar
                    {
                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
                    }
                    break;

                case State.Atacar:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    else if (Vida <= 50) //si nos falta vida huimos
                    {
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //3. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        salir(estadoAct);
                        entrar(State.Conquistar);
                    }
                    break;
                case State.Huir:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. La primera transicion en el estado huir es comprobar si el enemigo actual esta muerto o ya no me ve
                    else if (EnemigoActual.estaMuerto() || !EnemigoActual.sigoViendoEnemigo(this))
                    {
                        salir(estadoAct);
                        entrar(State.BuscandoCuracion);
                    }
                    break;
                case State.BuscandoCuracion:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2.
                    else if (haLlegadoADestino(puntoInteres)) {
                        salir(estadoAct);
                        entrar(State.Curandose);
                    }
                    break;
                case State.Curandose:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }


                    //2. La primera transicion es comprobar si su vida esta llena y si es asi pasar al estado vigilar
                    else if (Vida == 150)
                    {
                        salir(estadoAct);
                        entrar(State.Conquistar);
                    }
                    break;
                case State.Muerto:
                    //1. si la corutina reaparecer() ya se ha ejecutado despues de los 5 segundos
                    if (Vida == VidaMax)
                    {
                        salir(estadoAct);
                        entrar(State.RecorriendoCamino);
                    }
                    break;
                case State.RecorriendoCamino:
                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2.si ha llegado a su destino
                    else if (haLlegadoADestino(puntoInteres)) {
                        FindObjectOfType<LectorTeclado>().clearList(this);
                        salir(estadoAct);
                        entrar(State.Conquistar);
                    }
                    //3. si ve un enemigo
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
        if (EnemigoActual is ArchierAgentNPC) {
            enemyMultiplier = 1.5f;
        }
        else if(EnemigoActual is SoldierAgentNPC) {
            enemyMultiplier = 1.25f;
        }
        else enemyMultiplier = 0.5f;
        Vector2Int celdaActual = grid.getCeldaDePuntoPlano(this.Position);
        TypeTerrain t = mapaTerrenos.getTerrenoCasilla(celdaActual.x,celdaActual.y);
        switch (t) {
            case TypeTerrain.camino:
                terrainMultiplier = 0.75f;
                break;
            case TypeTerrain.llanura:
                terrainMultiplier = 1.25f;
                break;
            case TypeTerrain.bosque:
                terrainMultiplier = 2f;
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

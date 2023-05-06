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
        Vida = 200;
        VidaMax = 200;
        Inmovil = false;
        RangoAtaque = 1.2f;
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
                    return 2;
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

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }
                    //2. Transicion si veo un enemigo
                    else if (veoEnemigo())
                    {
                        salir(estadoAct);
                        entrar(State.Atacar);
                    }
                    break;
                case State.Atacar:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. La primera transicion para el tanque es comprobar si le queda poca vida para huir
                    else if (Vida <= 40)
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //3. Si tenemos vida suficiente pero no vemos al enemigo o esta muerto o ambas pues volvemos al estado Wander
                    else if(EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual))
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        finalidadPathFinding = typeRecorrerCamino.aVigilar;
                        puntoInteres = getFirstPointPath(pathToFollow);
                        entrar(State.RecorriendoCamino);
                    }
                    break;
                case State.Huir:

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. Si el enemigo que me esta siguiendo ha muerto o ya no me ve voy a curarme
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

                    //2. si ha llegado al punto de interes
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

                    //2. Si tengo la vida maxima ya puedo volver a buscar enemigos
                    else if (Vida == VidaMax)
                    {
                        salir(estadoAct);
                        finalidadPathFinding = typeRecorrerCamino.aVigilar;
                        puntoInteres = getFirstPointPath(pathToFollow);
                        entrar(State.RecorriendoCamino);
                    }
                    break;
                case State.Muerto:
                    //1. si la corutina reaparecer() ya se ha ejecutado despues de los 5 segundos
                    if (Vida == VidaMax)
                    {
                        salir(estadoAct);
                        finalidadPathFinding = typeRecorrerCamino.reaparecer;
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

                    //2. si ha llegado a su destino
                    else if (haLlegadoADestino(puntoInteres)) {
                        FindObjectOfType<LectorTeclado>().clearList(this);
                        salir(estadoAct);
                        if (finalidadPathFinding == typeRecorrerCamino.reaparecer || finalidadPathFinding == typeRecorrerCamino.seleccionUsuario || finalidadPathFinding == typeRecorrerCamino.aDefender) {
                            finalidadPathFinding = typeRecorrerCamino.aVigilar;
                            puntoInteres = getFirstPointPath(pathToFollow);
                            entrar(State.RecorriendoCamino);
                        }
                        else entrar(State.Vigilar);
                    }
                    //3.Si veo la torre enemiga a mitad de camino
                    else if(veoTorreEnemiga()) {
                            
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            entrar(State.Atacar);
                        }
                    //4. si ve un enemigo
                    else if(veoEnemigo())
                        {
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            entrar(State.Atacar);
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

                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }
                    //3.Si veo la torre enemiga a mitad de camino
                    else if(veoTorreEnemiga()) {
                            
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            entrar(State.Atacar);
                        }
                    //2. Transicion del estado Conquistar
                    else if (veoEnemigo())
                    {
                        salir(estadoAct);
                        entrar(State.Atacar);
                    }

                    //3.Si tu vida<=40 y ves un aliado
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
                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. Si tenemos vida suficiente pero no vemos al enemigo o esta muerto o ambas pues volvemos al estado Wander
                    else if (EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual))
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        //Si vamos a atacar la torre, entrar directamente en modo conquistar
                        if (irATorre){
                            entrar(State.Conquistar);
                        }
                        //Si vamos a hacer una patrulla en territorio enemigo, ir al primer punto
                        else {
                            finalidadPathFinding = typeRecorrerCamino.aConquistar;
                            puntoInteres = getFirstPointPath(OffensivePathToFollow);
                            entrar(State.RecorriendoCamino);
                        }
                    }
                    break;
                case State.Muerto:
                    //1. si la corutina reaparecer() ya se ha ejecutado despues de los 5 segundos
                    if (Vida == VidaMax)
                    {
                        salir(estadoAct);
                        if (GuerraTotal) {
                            entrar(State.Conquistar);
                        }
                        else {
                            finalidadPathFinding = typeRecorrerCamino.reaparecer;
                            entrar(State.RecorriendoCamino);
                        }
                    }
                    break;
                case State.RecorriendoCamino:
                    //1.Transicion que es comprobar si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }
                    //2.
                    if (haLlegadoADestino(puntoInteres)) {
                        salir(estadoAct);
                        if ((finalidadPathFinding == typeRecorrerCamino.reaparecer || finalidadPathFinding == typeRecorrerCamino.seleccionUsuario || finalidadPathFinding == typeRecorrerCamino.aDefender)) {
                            finalidadPathFinding = typeRecorrerCamino.aConquistar;
                            puntoInteres = getFirstPointPath(pathToFollow);
                            entrar(State.RecorriendoCamino);
                        }
                        //Vamos a la torre
                        else entrar(State.Conquistar);
                    }
                    //3.Si veo la torre enemiga a mitad de camino
                    else if(veoTorreEnemiga()) {
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            entrar(State.Atacar);
                        }
                    else if (veoEnemigo()){
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
            enemyMultiplier = 1.5f;
        }
        else if (EnemigoActual is ArchierAgentNPC) {
            enemyMultiplier = 2f;
        }
        else enemyMultiplier = 1f;
        
        Vector2Int celdaActual = grid.getCeldaDePuntoPlano(this.Position);
        TypeTerrain t = mapaTerrenos.getTerrenoCasilla(celdaActual.x,celdaActual.y);
        switch (t) {
            case TypeTerrain.camino:
                terrainMultiplier = 1f;
                break;
            case TypeTerrain.llanura:
                terrainMultiplier = 0.75f;
                break;
            case TypeTerrain.bosque:
                terrainMultiplier = 0.25f;
                break;
            case TypeTerrain.desierto:
                terrainMultiplier = 1.5f;
                break;
            default:
                break;
        }
        int realDamage = (int) System.Math.Round(baseDamage * enemyMultiplier * terrainMultiplier);
        return realDamage;
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

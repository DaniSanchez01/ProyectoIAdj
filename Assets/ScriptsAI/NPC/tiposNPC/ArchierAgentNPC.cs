using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchierAgentNPC : AgentNPC
{
    private bool buenRangoAtaque = false; //variable para controlar que tenemos
    //un buen rango de ataque
    // Start is called before the first frame update
    protected override void Start()
    {
        this.MaxSpeed = 7f;
        this.MaxAcceleration = 20f;
        this.MaxRotation = 180f;
        this.MaxForce = 30f;
        this.MaxAngularAcc = 300f;
        this.interiorAngle = 8f;
        this.influencia = 0.5f;
        if (team == Team.Blue){
            this.Orientation = 180f;
        }
        //inicializacion de atributos
        Vida = 80;
        VidaMax = 80;
        Inmovil = false;
        RangoAtaque = 7.0f;
        RangoVision = 8.0f;
        tipoAtaque = typeAtaque.persiguiendo;
        CoAtaque = atacar();
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
                        tipoAtaque = typeAtaque.persiguiendo;
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
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

                    //2. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    else if (Vida <= 30) //si nos falta vida huimos
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //3. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        finalidadPathFinding = typeRecorrerCamino.aVigilar;
                        puntoInteres = getFirstPointPath(pathToFollow);
                        entrar(State.RecorriendoCamino);
                    }
                    //Si no es ninguna de estas opciones
                    else {
                        switch(tipoAtaque) {
                            //Si el Arquero esta persiguiendo al enemigo
                            case typeAtaque.persiguiendo:
                                //Si ya lo tiene a rango, mantener distancia
                                if (estaARangoEnemigoAct()) {
                                    tipoAtaque = typeAtaque.manteniendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.VelocityMatch, this, EnemigoActual, pathToFollow);
                                }
                                break;
                            case typeAtaque.retrocediendo:
                                //Si ya se ha alejado lo suficiente, mantener distancia
                                if (Vector3.Distance(EnemigoActual.Position, this.Position) >= (RangoAtaque * 0.90f))
                                {
                                    //Debug.Log(gameObject.name + "aaaa");
                                    tipoAtaque = typeAtaque.manteniendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.VelocityMatch, this, EnemigoActual, pathToFollow);
                                }
                                break;
                            case typeAtaque.manteniendo:
                                //Si se ha acercado mucho el enemigo retroceder
                                if (Vector3.Distance(EnemigoActual.Position, this.Position) <= (RangoAtaque * 0.50f))
                                {
                                    //Debug.Log(gameObject.name + "aaaa");
                                    tipoAtaque = typeAtaque.retrocediendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.Huidizo, this, EnemigoActual, pathToFollow);
                                }
                                //Si se ha alejado mucho el enemigo perseguir
                                else if (Vector3.Distance(EnemigoActual.Position, this.Position) > RangoAtaque)
                                {
                                    //Debug.Log(gameObject.name + "bbbb");
                                    tipoAtaque = typeAtaque.persiguiendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.Perseguidor, this, EnemigoActual, pathToFollow);
                                }
                                break;
                            default:
                                break;      
                        }
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

                    //2. si ha llegado al destino
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
                            tipoAtaque = typeAtaque.persiguiendo;
                            entrar(State.Atacar);
                        }
                    //4. si veo algun enemigo
                    else if(veoEnemigo()) {
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            tipoAtaque = typeAtaque.persiguiendo;
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
                    //3.Si veo la torre enemiga a mitad de camino
                    else if(veoTorreEnemiga()) {
                            
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            tipoAtaque = typeAtaque.persiguiendo;
                            entrar(State.Atacar);
                        }
                    //2. La primera transicion del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    else if (veoEnemigo()) //1 transicion de Vigilar
                    {
                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
                        tipoAtaque = typeAtaque.persiguiendo;
                        entrar(State.Atacar); //voy a entrar en ataque y digo el enemigo que he detectado
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

                    //2. La primera transicion que se comprueba es la de huir pues si nos falta vida tendremos que huir para evitar un comportamiento anti-suicida
                    else if (Vida <= 50 && !GuerraTotal) //si nos falta vida huimos y si no estamos en guerrra total
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //3. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                    {

                        veoTorre = false;
                        salir(estadoAct);
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
                    //Si no es ninguna de estas opciones
                    else {
                        switch(tipoAtaque) {
                            //Si el Arquero esta persiguiendo al enemigo
                            case typeAtaque.persiguiendo:
                                //Si ya lo tiene a rango, mantener distancia
                                if (estaARangoEnemigoAct()) {
                                    tipoAtaque = typeAtaque.manteniendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.VelocityMatch, this, EnemigoActual, pathToFollow);
                                }
                                break;
                            case typeAtaque.retrocediendo:
                                //Si ya se ha alejado lo suficiente, mantener distancia
                                if (Vector3.Distance(EnemigoActual.Position, this.Position) >= (RangoAtaque * 0.90f))
                                {
                                    //Debug.Log(gameObject.name + "aaaa");
                                    tipoAtaque = typeAtaque.manteniendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.VelocityMatch, this, EnemigoActual, pathToFollow);
                                }
                                break;
                            case typeAtaque.manteniendo:
                                //Si se ha acercado mucho el enemigo retroceder
                                if (Vector3.Distance(EnemigoActual.Position, this.Position) <= (RangoAtaque * 0.50f))
                                {
                                    //Debug.Log(gameObject.name + "aaaa");
                                    tipoAtaque = typeAtaque.retrocediendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.Huidizo, this, EnemigoActual, pathToFollow);
                                }
                                //Si se ha alejado mucho el enemigo perseguir
                                else if (Vector3.Distance(EnemigoActual.Position, this.Position) > RangoAtaque)
                                {
                                    //Debug.Log(gameObject.name + "bbbb");
                                    tipoAtaque = typeAtaque.persiguiendo;
                                    this.deleteAllSteerings();
                                    listSteerings = GestorArbitros.GetArbitraje(typeArbitro.Perseguidor, this, EnemigoActual, pathToFollow);
                                }
                                break;
                            default:
                                break;      
                        }
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
                    else if (Vida == VidaMax)
                    {
                        salir(estadoAct);
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

                    //2.si ha llegado a su destino
                    else if (haLlegadoADestino(puntoInteres)) {
                        FindObjectOfType<LectorTeclado>().clearList(this);
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
                            tipoAtaque = typeAtaque.persiguiendo;
                            entrar(State.Atacar);
                        }
                    //3. si ve un enemigo
                    else if(veoEnemigo()) {
                            FindObjectOfType<LectorTeclado>().clearList(this);
                            salir(estadoAct);
                            tipoAtaque = typeAtaque.persiguiendo;
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

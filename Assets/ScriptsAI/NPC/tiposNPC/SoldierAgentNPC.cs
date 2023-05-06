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
        Vida = 140;
        VidaMax = 140;
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
                   
                    //1.Transicion que es comprobar si me han matado
                    if(Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }
                    //2. La primera transición del estado Vigilar se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    else if(veoEnemigo())
                    {

                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
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
                   else  if (Vida <= 20) //si nos falta vida huimos
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //3. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de vigilar.
                    else if (!veoTorre) {
                        if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
                        {
                            veoTorre = false;
                            salir(estadoAct);
                            finalidadPathFinding = typeRecorrerCamino.aVigilar;
                            puntoInteres = getFirstPointPath(pathToFollow);
                            entrar(State.RecorriendoCamino);
                        }
                    }

                    //en otro caso pues no se hace nada y se ejecutaria cada cierto tiempo la rutina atacar()
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

                    //2.Ttransicion que se da si he conseguido llegar al punto destino vivo
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
                    if(Vida == VidaMax)
                    {
                        salir(estadoAct);
                        finalidadPathFinding = typeRecorrerCamino.reaparecer;
                        entrar(State.RecorriendoCamino);
                    }
                    break;
                case State.RecorriendoCamino:

                    //1. Si me han matado
                    if (Vida == 0)
                    {
                        salir(estadoAct);
                        entrar(State.Muerto);
                    }

                    //2. Si he llegado al punto de interes (final del camino)
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
                    //4.Si veo a un enemigo a mitad de camino
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

        //Si el NPC está en modo ofensivo se aplica el automata de ataque
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
                            entrar(State.Atacar);
                        }
                    //2. La primera transición del estado Wander se corresponde a cambiar a estado de ataque si se ve un enemigo.
                    else if (veoEnemigo()) //1 transicion de WanderSoldier
                    {

                        salir(estadoAct); //Me quedo quieto despues de salir no tengo steerings
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
                    else if (Vida <= 20 && !GuerraTotal) //si nos falta vida huimos y si no estamos en guerra total
                    {
                        veoTorre = false;
                        salir(estadoAct);
                        entrar(State.Huir);
                    }

                    //2. La segunda es si tenemos vida suficiente y enemigo esta muerto o no lo seguimos viendo  o ambas deberemos pasar a un estado de Wander.
                    else if ((EnemigoActual.estaMuerto() || !sigoViendoEnemigo(EnemigoActual)))
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

                    //en otro caso pues no se hace nada y se ejecutaria cada cierto tiempo la rutina atacar()
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

                    //1. La primera transicion es comprobar si su vida esta llena
                    else if (Vida == VidaMax)
                    {
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

                    //2.si he llegado al destino
                    else if (haLlegadoADestino(puntoInteres)) {
                        FindObjectOfType<LectorTeclado>().clearList(this);
                        salir(estadoAct);
                        //Si acabamos de llegar al punto donde morimos o donde nos seleccionaron, y tenemos que patrullar en el campo enemigo
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
                    //si veo al enemigo
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

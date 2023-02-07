using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Si se quiere una agente con animación simple, busca un modelo con 2 o 3 estados de animación.
///  Crea una máquina de estados (Animator) para esos estados. Idle y Walk
///  Pasa de uno a otro y de otro a uno usando un parámetro real que llamarás "Velocity"
///  Arrastra el Animator al atributo animator de esta clase y ya tienes un personaje con movimiento.
/// </summary>
[RequireComponent(typeof(Animation))]
public class AgentPlayerWithAnimation : AgentPlayer 
{
    public Animator animator; // Añade aquí el Animator

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        // Descomenta
        // animator.SetFloat("Velocity", Speed); // Speed, propiedad que calcula el módulo de Velocity
    }
}

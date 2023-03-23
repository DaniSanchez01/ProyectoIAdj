using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AgentNPC))]
public class SteeringBehaviour : MonoBehaviour
{

    protected string nameSteering = "no steering";
    public float weight = 1; //es el peso correspondiente al steering de base todos tienen 1 

    public string NameSteering
    {
        set { nameSteering = value; }
        get { return nameSteering; }
    }

    public float Weight //aqui especifico que el peso se puede modificar y acceder
    {
        set { weight = value; }
        get { return weight;  }
    }

    public virtual void NewTarget(Agent t){

    }


    /// <summary>
    /// Cada SteerinBehaviour retornará un Steering=(vector, escalar)
    /// acorde a su propósito: llegar, huir, pasear, ...
    /// Sobreescribie siempre este método en todas las clases hijas.
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public virtual Steering GetSteering(AgentNPC agent)
    {
        return null;
    }

    public virtual void DestroyVirtual(Agent first) {
        
    }

    protected virtual void OnGUI()
    {
        // Para la depuración te puede interesar que se muestre el nombre
        // del steeringbehaviour sobre el personaje.
        // Te puede ser util Rect() y GUI.TextField()
        // https://docs.unity3d.com/ScriptReference/GUI.TextField.html
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchierAgentNPC : AgentNPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.Mass = 1; //tiene mas masa
        this.MaxSpeed = 1.2f; // la maxima velocidad tambien se disminuye
        this.MaxRotation = 1.2f; //la maxima velocidad angular que puede tener al ser mas pesado tienes menos velocidad angular
        this.MaxAcceleration = 1.2f; //la maxima aceleracion tambien la he reducido
        this.MaxAngularAcc = 1.1f; //la maxima aceleracion agular tambien se ha reducido
        this.MaxForce = 1; //la maxima fuerza
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

}

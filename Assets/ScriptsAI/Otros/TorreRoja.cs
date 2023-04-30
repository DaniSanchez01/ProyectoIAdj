using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TorreRoja : MonoBehaviour
{

    private int vida = 300;
    private TMP_Text contador;
    // Start is called before the first frame update
    void Start()
    {
        contador = transform.Find("ContadorVida").GetComponent<TMP_Text>();
    }

    public void recibirDamage(int x) {
        vida = vida-x;
        if (vida<0) vida=0;
        contador.text = "Vida: "+vida;
        if (vida == 0) finishGame();
    }

    public void finishGame() {
        
    }
}

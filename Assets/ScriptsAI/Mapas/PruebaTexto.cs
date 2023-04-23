using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PruebaTexto : MonoBehaviour
{

    public string texto = "";
    public bool activar = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform hijo = transform.Find("Bocadillo");
        hijo.gameObject.SetActive(activar);
        hijo  = hijo.Find("Texto");
        TMP_Text t = hijo.GetComponent<TMP_Text>();
        t.text = texto;
    }
}

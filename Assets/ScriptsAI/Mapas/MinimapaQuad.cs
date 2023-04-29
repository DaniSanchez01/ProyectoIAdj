using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


    public class MinimapaQuad : MonoBehaviour
    {
        
        GameObject[,] quadMap;
        Vector3 posicionBase = new Vector3(50f,300f,50f);
        Color darkRed = new Color(0.5f, 0f, 0f, 1f);
        Color darkBlue = new Color(0f, 0f, 0.5f, 1f);
        Color lightGrey = new Color(0.8f, 0.8f, 0.8f, 1f);



        void Start() {
            quadMap = new GameObject[30, 30];
            for (int i=0;i<30;i++) {
                for (int j=0;j<30;j++) {
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.parent = transform;
                    quad.transform.localPosition = new Vector3(i,j,0);
                    quad.transform.Rotate(new Vector3(0,0,0));
                    // Obtener la referencia del material
                    Material myMaterial = quad.GetComponent<Renderer>().material;

                    quadMap[i,j] = quad;
                }
            }
        }

        public void ChangeColor(float[,] mapa) {
            for (int x = 0; x < 30; x++) {
                for (int y = 0; y < 30; y++) {
                    float valor = mapa[x, y];
                    if (valor > 0) {
                        if (valor > 1f) valor = 1f;
                        Color color = Color.Lerp(Color.white, Color.blue, valor);
                        quadMap[x,y].GetComponent<Renderer>().material.color = color;

                    } 
                    else {
                        if (valor < -1f) valor = -1f;
                        Color color = Color.Lerp(Color.white, Color.red, -valor);
                        quadMap[x,y].GetComponent<Renderer>().material.color = color;
                    }
                }
            }
        }       

        public void ChangeColorTension(float[,] mapa) {
            for (int x = 0; x < 30; x++) {
                for (int y = 0; y < 30; y++) {
                    float valor = mapa[x, y];
                    valor =valor/2f;
                    Color color = Color.Lerp(Color.white, Color.black, valor);
                    quadMap[x,y].GetComponent<Renderer>().material.color = color;
                }
            }
        }   
        
    }

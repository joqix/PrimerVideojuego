using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorDeAvatares : MonoBehaviour
{

    [SerializeField] private GameObject listaAvatares;
    [SerializeField] private GameObject image;
    [SerializeField] private ControlConexion CtrlConexion;
    
    private int nHijos;
    private int indiceAvatares;

    private GameObject[] avatares;
    // Start is called before the first frame update
    void Start()
    {
        indiceAvatares = 0;
        nHijos = listaAvatares.transform.childCount;
        avatares = new GameObject[nHijos];

        for(int i=0; i < nHijos; i++)
        {
            avatares[i] = listaAvatares.transform.GetChild(i).gameObject;
        }

        image.GetComponent<Image>().sprite = avatares[indiceAvatares].gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pulsar_BtnDerecho()
    {
        indiceAvatares++;

        if(indiceAvatares > nHijos - 1)
        {
            indiceAvatares = 0;
        }
        image.GetComponent<Image>().sprite = avatares[indiceAvatares].gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void Pulsar_BtnIzquierdo()
    {
        indiceAvatares--;

        if (indiceAvatares < 0)
        {
            indiceAvatares = nHijos-1;
        }
        image.GetComponent<Image>().sprite = avatares[indiceAvatares].gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void PulsarSeleccionarAvatar()
    {
        CtrlConexion.conex.avatarSeleccionado = indiceAvatares;
    }

}

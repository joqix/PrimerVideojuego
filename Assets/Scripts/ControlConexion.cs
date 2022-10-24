using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ControlConexion : MonoBehaviour
{
    [Header("inputField")]
    [SerializeField] private TextMeshProUGUI txtNombreJugador;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI txtBarraDeEstado;
    //[SerializeField] private TextAlignment txtNombre;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pulsar_BtnConectar()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = txtNombreJugador.text;

        txtBarraDeEstado.text = txtNombreJugador.text;

    }

}

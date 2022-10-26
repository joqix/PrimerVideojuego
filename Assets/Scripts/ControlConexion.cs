using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class ControlConexion : MonoBehaviourPunCallbacks
{
    #region variables
    [Header("inputField")]
    [SerializeField] private Text txtNombreJugador;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI txtBarraDeEstado;
    [SerializeField] private TextMeshProUGUI txtBienvenida;
    [SerializeField] private TextMeshProUGUI txtNuevaSala;
    [SerializeField] private TextMeshProUGUI txtUnirseSala;


    [Header("Paneles")]
    [SerializeField] private GameObject panelConexion;
    [SerializeField] private GameObject panelBienvenida;
    [SerializeField] private GameObject panelCrearSala;
    [SerializeField] private GameObject panelUnirseSala;
    [SerializeField] private GameObject panelSala;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        ActivarPanel(panelConexion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pulsar_BtnConectar()
    {
        
        if (!string.IsNullOrEmpty(txtNombreJugador.text) || !string.IsNullOrWhiteSpace(txtNombreJugador.text))
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = txtNombreJugador.text;

            EscribirBarraEstado(txtNombreJugador.text);
        }
        else
        {
            txtBarraDeEstado.text = "Indica un nombre de usuario para conectar";
        }

    }

    private void EscribirBarraEstado(string texto)
    {
        txtBarraDeEstado.text = texto;
    }

    private void ActivarPanel(GameObject panel)
    {
        panelConexion.SetActive(false);
        panelBienvenida.SetActive(false);
        panelCrearSala.SetActive(false);
        panelUnirseSala.SetActive(false);
        panelSala.SetActive(false);

        panel.SetActive(true);
    }

    public void Pulsar_BtnCrearSala()
    {
        ActivarPanel(panelCrearSala);
        EscribirBarraEstado(" Crear una sala nueva");
    }

    public void Pulsar_BtnUnirseSala()
    {
        ActivarPanel(panelUnirseSala);
        EscribirBarraEstado(" Unirse a una sala");
    }

    public void Pulsar_BtnSalir()
    {
        Application.Quit();
    }

    public void Pulsar_BtnDesconectar()
    {
        PhotonNetwork.Disconnect();
        ActivarPanel(panelConexion);
    }

    public void Pulsar_CrearSala()
    {
        RoomOptions opcionesSala = new RoomOptions();
        opcionesSala.MaxPlayers = 2;
        opcionesSala.IsVisible = true;

        PhotonNetwork.CreateRoom(txtNuevaSala.text, opcionesSala, TypedLobby.Default);

    }

    public void Pulsar_BtnVolver()
    {
        ActivarPanel(panelBienvenida);
    }

    public void PulsarUnirseASala()
    {
        if (!string.IsNullOrEmpty(txtUnirseSala.text) || !string.IsNullOrWhiteSpace(txtUnirseSala.text))
        {
            PhotonNetwork.JoinRoom(txtUnirseSala.text);
        }
        else
        {
            EscribirBarraEstado("Introduzca un nombre correcto para la sala");
        }
    }

    #region Callbacks

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        EscribirBarraEstado("OnConnectedToMaster - Conectado a Photon");
        Debug.Log ("OnConnectedToMaster - Conectado a Photon");
        EscribirBarraEstado(PhotonNetwork.NickName + ", bienvenido al juego");
        ActivarPanel(panelBienvenida);

        txtBienvenida.text = PhotonNetwork.NickName;
    }

    public override void OnConnected()
    {
        //base.OnConnected();

        EscribirBarraEstado("OnConnected - Conectado a Photon");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        EscribirBarraEstado("DESCONECTADO PHOTON: " + cause);

        Debug.Log("DESCONECTADO PHOTON: " + cause);
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        string mensaje = PhotonNetwork.NickName + " se ha conectado a " + PhotonNetwork.CurrentRoom.Name;
        EscribirBarraEstado(mensaje);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        EscribirBarraEstado("Sala no creada ERROR " + returnCode + ": " + message);
    }

    public override void OnJoinedRoom()
    {
        EscribirBarraEstado(PhotonNetwork.NickName + " se ha unido a " + PhotonNetwork.CurrentRoom.Name);
        ActivarPanel(panelSala);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        EscribirBarraEstado("Error al unirse a la sala, Error: " + returnCode + ": " + message);
    }


    #endregion


}

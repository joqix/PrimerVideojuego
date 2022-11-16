using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class ControlConexion : MonoBehaviourPunCallbacks
{
    #region variables privadass
    [Header("inputField")]
    [SerializeField] private Text txtNombreJugador;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI txtBarraDeEstado;
    [SerializeField] private TextMeshProUGUI txtBienvenida;
    [SerializeField] private TextMeshProUGUI txtNuevaSala;
    //[SerializeField] private TextMeshProUGUI txtUnirseSala;
    [SerializeField] private Text txtMinJugadores;
    [SerializeField] private Text txtMaxJugadores;

    [SerializeField] private Text txtNombreSala;
    [SerializeField] private Text txtCapacidad;
    [SerializeField] private Text txtListaJugadores;


    [Header("Paneles")]
    [SerializeField] private GameObject panelConexion;
    [SerializeField] private GameObject panelBienvenida;
    [SerializeField] private GameObject panelCrearSala;
    [SerializeField] private GameObject panelUnirseSala;
    [SerializeField] private GameObject panelSala;
    [SerializeField] private GameObject panelSeleccionAvatar;
    #endregion
    [Header("otros")]
    public int avatarSeleccionado;
    public ControlConexion conex;
    [SerializeField] private Button btnComenzarJuego;
    ExitGames.Client.Photon.Hashtable PlayerProperties;

    [Header("ListaJugadores")]
    public GameObject elementoJugador;
    public GameObject contenedor;

    [Header("ListaSala")]
    public GameObject elementoSala;
    public GameObject contenedorSala;




    Dictionary<string, RoomInfo> listaSalas;

    //[Header("Otros")]
    
    

    // Start is called before the first frame update
    void Start()
    {
        PlayerProperties = new ExitGames.Client.Photon.Hashtable();
        listaSalas = new Dictionary<string, RoomInfo>();
        avatarSeleccionado = -1;
        conex = this;
        ActivarPanel(panelConexion);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region botones
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

    public void Pulsar_BtnCrearSala()
    {
        ActivarPanel(panelCrearSala);
        EscribirBarraEstado(" Crear una sala nueva");
    }

    public void Pulsar_BtnUnirseSala()
    {
        ActivarPanel(panelUnirseSala);
        EscribirBarraEstado(" Unirse a una sala");
        ActualizarPanelUnirseSala();
    }

    public void Pulsar_BtnDesconectar()
    {
        PhotonNetwork.Disconnect();
        ActivarPanel(panelConexion);
    }

    public void Pulsar_CrearSala()
    {
        byte minJugadores = byte.Parse(txtMinJugadores.text);
        byte maxJugadores = byte.Parse(txtMaxJugadores.text);


        if (!string.IsNullOrEmpty(txtNuevaSala.text) || !string.IsNullOrWhiteSpace(txtNuevaSala.text))
        {
            if (!(minJugadores > maxJugadores || maxJugadores > 20 || minJugadores > 20 || maxJugadores < 2 || minJugadores < 2))
            {
                RoomOptions opcionesSala = new RoomOptions();
                opcionesSala.MaxPlayers = maxJugadores;
                opcionesSala.IsVisible = true;

                PhotonNetwork.CreateRoom(txtNuevaSala.text, opcionesSala, TypedLobby.Default);
            }
            else
            {
                EscribirBarraEstado("Introduzca los valores correctos para la capacidad de la sala");
            }
        }
        else
        {
            EscribirBarraEstado("Introduzca un nombre correcto para la sala");
        }

    }

    /*public void PulsarUnirseASala()
    {


        if (!string.IsNullOrEmpty(txtUnirseSala.text) || !string.IsNullOrWhiteSpace(txtUnirseSala.text))
        {

            PhotonNetwork.JoinRoom(txtUnirseSala.text);
        }
        else
        {
            EscribirBarraEstado("Introduzca un nombre correcto para la sala");
        }
    }*/

    public void Pulsar_BtnSeleccionAvatar()
    {
        ActivarPanel(panelSeleccionAvatar);
        EscribirBarraEstado(" SeleccionarAvatar");
    }

    public void Pulsar_BtnSalir()
    {
        Application.Quit();
    }

    public void Pulsar_BtnVolver()
    {
        ActivarPanel(panelBienvenida);

    }

    public void Pulsar_BtnVolverAvatar()
    {
        ActivarPanel(panelBienvenida);

        if (avatarSeleccionado >= 0)
        {
            EscribirBarraEstado("Avatar Seleccionado " + avatarSeleccionado);
            panelBienvenida.transform.Find("btnCrearSala").GetComponent<Button>().interactable = true;
            panelBienvenida.transform.Find("btnUnirseSala").GetComponent<Button>().interactable = true;


        }
        else
        {
            EscribirBarraEstado("No ha seleccionado avatar");
        }

    }

    #endregion
    public void ActualizarPanelSala()
    {
        //OLD
        string cadena = "";
        txtNombreSala.text = "SALA: " + PhotonNetwork.CurrentRoom.Name;
        txtCapacidad.text = "Capacidad " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        
        foreach( Player jugador in PhotonNetwork.PlayerList)
        {
            cadena = cadena + jugador.NickName + "\n";
        }

        txtListaJugadores.text = cadena;
        //FIN OLD

        //NEW
        //eliminamos los hijos mientras halla
        while (contenedor.transform.childCount > 0)
        {
            DestroyImmediate(contenedor.transform.GetChild(0).gameObject);
        }

        foreach (Player jugador in PhotonNetwork.PlayerList)
        {
            GameObject nuevoElemento = Instantiate(elementoJugador);
            nuevoElemento.transform.SetParent(contenedor.transform);

            //localizamos textos y actualizamos
            nuevoElemento.transform.Find("txtNombreJugador").GetComponent<TextMeshProUGUI>().text = jugador.NickName;
            nuevoElemento.transform.Find("txtNum").GetComponent<TextMeshProUGUI>().text = jugador.ActorNumber.ToString();

        }
        //comprobar si hay el minimo de jugadores
        if(PhotonNetwork.CurrentRoom.PlayerCount >= int.Parse(txtMinJugadores.text))
        {
            btnComenzarJuego.interactable = true;
        }
        else
        {
            btnComenzarJuego.interactable = false;
        }

    }


    public void ActualizarPanelUnirseSala()
    {
        Debug.Log("AcualizoPanelUnirse");
        while (contenedorSala.transform.childCount > 0)
        {
            DestroyImmediate(contenedorSala.transform.GetChild(0).gameObject);
            Debug.Log("Eliino sala ");
        }

        foreach (RoomInfo sala in listaSalas.Values)
        {
            //if (sala.IsVisible)
            //{
            Debug.Log("creo sala ");
            GameObject nuevoElemento = Instantiate(elementoSala);
                nuevoElemento.GetComponent<Transform>().SetParent(contenedorSala.GetComponent<Transform>());

                //localizamos textos y actualizamos
                nuevoElemento.transform.Find("txtNombreSala").GetComponent<TextMeshProUGUI>().text = sala.Name;
                nuevoElemento.transform.Find("txtNum").GetComponent<TextMeshProUGUI>().text = sala.PlayerCount + "/" + sala.MaxPlayers;
                nuevoElemento.GetComponent<Button>().onClick.AddListener(() => { UnirseASalaDesdeLista(sala.Name); });
            //}
        }
    }

    public void UnirseASalaDesdeLista(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo sala in roomList)
        {
            if (sala.RemovedFromList || !sala.IsOpen || !sala.IsVisible)
            {
                listaSalas.Remove(sala.Name);
            }

            if (listaSalas.ContainsKey(sala.Name))
            {
                if(sala.PlayerCount > 0)
                {
                    listaSalas[sala.Name] = sala;
                }
                else
                {
                    listaSalas.Remove(sala.Name);
                }
            }
            else
            {
                listaSalas.Add(sala.Name,sala);
                    
            }

        }

        ActualizarPanelUnirseSala();

        
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
        panelSeleccionAvatar.SetActive(false);

        panel.SetActive(true);
    }


    #region Callbacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        EscribirBarraEstado("OnConnectedToMaster - Conectado a Photon");
        Debug.Log ("OnConnectedToMaster - Conectado a Photon");
        EscribirBarraEstado(PhotonNetwork.NickName + ", bienvenido al juego");
        ActivarPanel(panelBienvenida);

        txtBienvenida.text = PhotonNetwork.NickName;
       
        ActualizarPanelUnirseSala();
    }

    public override void OnConnected()
    {
        base.OnConnected();

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
        
        EscribirBarraEstado(PhotonNetwork.NickName + " se ha conectado a " + PhotonNetwork.CurrentRoom.Name);
        ActualizarPanelSala();
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
        ActualizarPanelSala();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        EscribirBarraEstado("Error al unirse a la sala, Error: " + returnCode + ": " + message);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ActualizarPanelSala();
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        ActualizarPanelSala();
    }

    #endregion


}
//THE BOSS  CHAD  ROTH
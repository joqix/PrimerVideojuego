using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{

    [Header("Prefabs Jugadores")]
    [SerializeField] private GameObject[] prefabsJugadores;

    private GameObject jugador;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            object avatarJugador = PhotonNetwork.LocalPlayer.CustomProperties["avatar"];

            jugador = PhotonNetwork.Instantiate( prefabsJugadores[(int)avatarJugador].name, new Vector3(0,0,0),Quaternion.identity,0);

            //mover la camara
            Camera.main.transform.SetParent(jugador.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

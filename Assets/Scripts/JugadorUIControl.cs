using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class JugadorUIControl : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshPro nombre_jugador_cabeza;

    // Start is called before the first frame update
    void Start()
    {
        nombre_jugador_cabeza.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

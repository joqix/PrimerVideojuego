using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlJugador : MonoBehaviourPunCallbacks
{


    [Header("Caracterisiticas")]
    [SerializeField] private float velocidad;
    [SerializeField] private float velocidadGiro;
    [SerializeField] private float fuerzaSalto;

    private Rigidbody rig;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& photonView.IsMine)
        {
            Saltar();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {

            Movimiento();
        }
        
    }

    private void Movimiento()
    {
        Vector3 velNormalicada = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        velNormalicada = velNormalicada.normalized;

        rig.velocity = transform.forward * velNormalicada.z * velocidad + transform.right * velNormalicada.x * velocidad + transform.up * rig.velocity.y;

        transform.Rotate(Input.GetAxis("Mouse X") * velocidadGiro * transform.up);

        anim.SetFloat("velocidad", rig.velocity.magnitude);
    }

    private void Saltar()
    {
        Ray rayo = new Ray(transform.position + new Vector3(0, 0.2f, 0), Vector3.down);
        if (Physics.Raycast(rayo, 0.4f))
        {
            anim.SetTrigger("salto");
            rig.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

}

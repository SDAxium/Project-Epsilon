using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReloadGunOnSocketEnter : MonoBehaviour
{
    private XRSocketInteractor socket;
    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    public void Reload()
    {
        //Get the gun script from whatever gun is attached to this socket and run the reload script
        socket.GetOldestInteractableSelected().transform.gameObject.GetComponent<Gun>().Reload();
    }
}

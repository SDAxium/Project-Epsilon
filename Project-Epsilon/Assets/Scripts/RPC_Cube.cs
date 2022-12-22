using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RPC_Cube : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 2f;
    
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            index++;
            if (index > 2) index = 0;
            GetComponent<PhotonView>().RPC("UpdateColor",RpcTarget.All);
            if(PhotonNetwork.IsConnectedAndReady) print(PhotonNetwork.CurrentRoom.Name);
        }
    }

    [PunRPC]
    public void UpdateColor()
    {
        var material = GetComponent<MeshRenderer>().material;
        if (index == 0)
        {
            material.color = new Color(0.75f, 0, 0.75f, 1f);
        }
        else if(index == 1)
        {
            material.color = new Color(0, 0, 0.75f, 1f);
        }
        else if (index == 2)
        {
            material.color = new Color(0.75f,0,0,1f);
        }
        
    }
}

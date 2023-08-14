using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawnHandler : NetworkBehaviour
{
    private Transform BeginSpawnLocation;

    public override void OnNetworkSpawn()
    {
        if (IsClient && IsOwner)
        {
            BeginSpawnLocation = GameObject.Find("NetworkManager").transform;
            transform.root.transform.localPosition = BeginSpawnLocation.position;
        }
    }


}

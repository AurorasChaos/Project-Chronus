using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerStats : NetworkBehaviour
{
    private NetworkVariable<ulong> ClientID;
    private NetworkVariable<int> AmmoCount;


    [SerializeField]private int Def_AmmoCount;

    private int TeamNumber;

    public PlayerStats(int _TeamNumber)
    {
        ClientID.Value = NetworkManager.LocalClientId;
        AmmoCount.Value = Def_AmmoCount;
        TeamNumber = _TeamNumber;
    }


    public void NewRoundResetStats()
    {
        AmmoCount.Value = Def_AmmoCount;
    }
}

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using Unity.Netcode;

public class RecordPlayerPacket : NetworkBehaviour
{
    private int PlayerID;
    public string Round_Number = "000";
    public List<Move_Packet> Movement_Packets = new List<Move_Packet>();
    public List<Action_Packet> Action_Packets = new List<Action_Packet>();
    private Control_Codes control_Codes = new Control_Codes();

    [SerializeField] private ActionHandlerClient AHC;

    [SerializeField] public int TeamNumber;


    private void Start()
    {
        if (IsClient && IsOwner)
        {
            control_Codes.SetupControlCodes();
            PlayerID = Convert.ToInt32(OwnerClientId);
            GameObject.FindObjectOfType<MainEventHandler>().UpdateRPPListServerRpc();
        }
    }

    public int GetPlayerID()
    {
        return PlayerID;
    }

    public void UpdateRoundNumber(bool Increment)
    {
        if (Increment)
        {
            string Round_Number_tmp = (Int32.Parse(Round_Number) + 1).ToString();
            while (Round_Number_tmp.Length < 3) { Round_Number_tmp = "0" + Round_Number_tmp; }
            Round_Number = Round_Number_tmp;
        }
    }

    public void ClearLists()
    {
        Movement_Packets.Clear();
        Action_Packets.Clear();
    }

    public List<Move_Packet> RequestListOfMovePackets()
    {
        return Movement_Packets;
    }

    public List<Action_Packet> RequestListOfActionPackets()
    {
        return Action_Packets;
    }

    [ClientRpc]
    public void RecordNewPacketClientRpc(int Frame_Number)
    {
        int Control_Code;
        if (Frame_Number == 0) { Control_Code = 101; } else { Control_Code = 102; }

        string Frame_Number_str = Frame_Number.ToString();
        while (Frame_Number_str.Length < 5)
        {
            Frame_Number_str = "0" + Frame_Number_str;
        }

        string PacketNumber = Frame_Number_str + Round_Number + PlayerID;

        Quaternion QuatToUse = transform.GetComponentInChildren<Camera>().transform.rotation;

        Movement_Packets.Add(
            new Move_Packet(
                transform.position,
                transform.rotation,
                Control_Code,         
                101,                  //Hard coded for basic implementation.
                PacketNumber,
                QuatToUse
            )
        );

        Action_Packet MRAP = AHC.ReturnMostRecentActionPacket();
        MRAP.Packet_Number = PacketNumber;
        Action_Packets.Add(MRAP);
        if (AHC.CanBeReset)
        {
            AHC.ResetMostRecentActionPacket();
        }

    } 

}

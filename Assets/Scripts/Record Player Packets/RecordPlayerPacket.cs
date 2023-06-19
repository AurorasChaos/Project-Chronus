using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RecordPlayerPacket : NetworkBehaviour
{
    public MovementPacket[,] currentRoundOfPlayerMovementPackets;
    public ActionPacket[,] currentRoundOfPlayerActionPackets;
    private int frameCount = 0;
    private int ActionToMovePacketRatio = 120/30;
    private RefListStorage RLS;

    public void SetupRecordPlayerPackets(int roundLength, int playerCount, RefListStorage _RLS)
    {
        currentRoundOfPlayerMovementPackets = new MovementPacket[roundLength/ActionToMovePacketRatio, playerCount];
        currentRoundOfPlayerActionPackets = new ActionPacket[roundLength, playerCount];
        RLS = _RLS;
    }

    public void RecordNewSetOfPlayerPackets()
    {
        GameObject[] listOfPlayerActions = RLS.getPlayerObjects();

        for (byte i = 0; i < listOfPlayerActions.Length; i++)
        {
            if (frameCount % ActionToMovePacketRatio == 0 || frameCount == 0) //Ensure Movement Packets are only recorded at 30Hz
            {
                currentRoundOfPlayerMovementPackets[i, frameCount/ActionToMovePacketRatio] = new MovementPacket(
                i,
                listOfPlayerActions[i].transform.localPosition,
                listOfPlayerActions[i].transform.localRotation,
                001,
                101
                );
            }
            //Allow Action Packets to be recorded at 120Hz
            currentRoundOfPlayerActionPackets[i, frameCount] = new ActionPacket(
                i,
                GetPlayersRecentActionCode(i),
                001
                );
        }
        
    }

    public byte GetPlayersRecentActionCode(byte playerID)
    {
        byte actionCode = RLS.getMostRecentPlayerAction(playerID);
        if (actionCode == 0)
        {
            return 200;
        } else
        {
            return actionCode;
        }
    }


}

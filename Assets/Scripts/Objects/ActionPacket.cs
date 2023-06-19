using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPacket
{
    public byte playerID;
    public byte actionCode;
    public byte packetCode;

    public ActionPacket(byte playerID, byte actionCode, byte packetCode)
    {
        this.playerID = playerID;
        this.actionCode = actionCode;
        this.packetCode = packetCode;
    }
}

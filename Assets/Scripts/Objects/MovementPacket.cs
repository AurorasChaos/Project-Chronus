using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPacket
{
    public byte playerID;
    public Vector3 localLocation;
    public Quaternion localRotation;
    public byte packetCode;
    public byte movementCode;

    public MovementPacket(byte playerID, Vector3 localLocation, Quaternion localRotation, byte packetCode, byte movementCode)
    {
        this.playerID = playerID;
        this.localLocation = localLocation;
        this.localRotation = localRotation;
        this.packetCode = packetCode;
        this.movementCode = movementCode;
    }
}

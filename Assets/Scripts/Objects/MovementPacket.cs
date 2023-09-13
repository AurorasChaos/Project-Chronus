using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPacket
{
    public byte playerID;
    public ByteBasedVector3 localLocation;
    public Quaternion localRotation;
    public byte packetCode;
    public byte movementCode;

    public MovementPacket(byte playerID, Vector3 localLocation, Quaternion localRotation, byte packetCode, byte movementCode)
    {
        this.playerID = playerID;
        this.localLocation = new ByteBasedVector3(localLocation.x, localLocation.y, localRotation.z);
        this.localRotation = localRotation;
        this.packetCode = packetCode;
        this.movementCode = movementCode;
    }
}

public class ByteBasedVector3
{
    private short X; private short Y; private short Z;

    public ByteBasedVector3(float x, float y, float z) { 
        this.X = (short)(x * (2^8));
        this.Y = (short)(y * (2^8));
        this.Z = (short)(z * (2^8));
    }

    public float x => this.X / (2 ^ 8);
    public float y => this.Y / (2 ^ 8);
    public float z => this.Z / (2 ^ 8);

}

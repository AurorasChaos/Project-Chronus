using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move_Packet 
{
    public Vector3 New_Pos;
    public Quaternion New_Rot;

    public Quaternion Camera_Rot;

    public int Control_Code;
    public int M_Action_ID;

    public string Packet_Number;

    public Move_Packet(Vector3 _New_Pos, Quaternion _New_Rot, int _Control_Code, int _M_Action_ID, string _Packet_Number, Quaternion _Camera_Rot)
    {
        New_Pos = _New_Pos;
        New_Rot = _New_Rot;
        Control_Code = _Control_Code;
        M_Action_ID = _M_Action_ID;
        Packet_Number = _Packet_Number;
        Camera_Rot = _Camera_Rot;
    }

}

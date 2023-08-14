using System;
using UnityEngine;
using System.Runtime.InteropServices;
[Serializable]
public class Action_Packet 
{

    public int A_Action_ID;
    public int Control_Code;
    public string Packet_Number;

    public Action_Packet(int _A_Action_ID, int _Control_Code, string _Packet_Number)
    {
        A_Action_ID = _A_Action_ID;
        Control_Code = _Control_Code;
        Packet_Number = _Packet_Number;
    }


}

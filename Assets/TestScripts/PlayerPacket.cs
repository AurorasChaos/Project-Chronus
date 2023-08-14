using System;
using UnityEngine;


namespace UtilityObjects
{ 
    [Serializable]
    public class PlayerPacket 
    {
        private int FrameNumber;
        private Transform PacketTransform;
        private int ActionID;

        public PlayerPacket(int Frame_Number, Transform Packet_Transform, int Action_ID)
        {
            FrameNumber = Frame_Number;
            PacketTransform = Packet_Transform;
            ActionID = Action_ID;

            PacketTransform.parent = null;
        }

        public int ReturnFrameNumber()
        {
            return FrameNumber;
        }

        public Transform ReturnPacketTransform()
        {
            return PacketTransform;
        }

        public int ReturnActionID()
        {
            return ActionID;
        }

    }

}


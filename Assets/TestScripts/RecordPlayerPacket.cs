using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ClientScripts
{
    public class RecordPlayerPacket : NetworkBehaviour
    {
        private List<UtilityObjects.PlayerPacket> playerPackets = new List<UtilityObjects.PlayerPacket>();
        private NetworkVariable<List<UtilityObjects.PlayerPacket>> serverPlayerPackets = new NetworkVariable<List<UtilityObjects.PlayerPacket>>();

        public override void OnNetworkSpawn()
        {
            //Tell server to update their list of RPPs
        }

        public void ClearList()
        {
            playerPackets.Clear();
        }

        public List<UtilityObjects.PlayerPacket> RequestPlayerPackets()
        {
            return playerPackets;
        }

        [ClientRpc]
        public void RecordNewPacketClientRpc(int FrameNumber)
        {
            int Action_ID = 100;


            UtilityObjects.PlayerPacket playerPacket = new UtilityObjects.PlayerPacket(FrameNumber, transform, Action_ID);

            playerPackets.Add(playerPacket);

        }

        [ClientRpc]
        public List<UtilityObjects.PlayerPacket> GetPlayerPackets()
        {
            return playerPackets;
        }
    }
}


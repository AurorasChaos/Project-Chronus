using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class PlayBackClones : NetworkBehaviour
{

    [SerializeField] private List<Move_Packet> Movement_Packets_Master = new List<Move_Packet>();
    [SerializeField] private List<Action_Packet> Action_Packet_Master  = new List<Action_Packet>();
    [SerializeField] private List<GameObject> ListOfClones = new List<GameObject>();
    [SerializeField] private ActionHandlerServer AHS;
    private Control_Codes control_Codes = new Control_Codes();

    public GameObject clonePrefab;

    public void SetupNeccesaryControlCodes()
    {
        control_Codes.SetupControlCodes();
    }

    public void UpdateMasterPacketLists()
    {
        var ListOfPlayerPacketRecorders = GameObject.FindObjectsOfType<RecordPlayerPacket>();

        foreach (RecordPlayerPacket RPP in ListOfPlayerPacketRecorders)
        {
            Movement_Packets_Master.AddRange(RPP.RequestListOfMovePackets());
            Action_Packet_Master.AddRange(RPP.RequestListOfActionPackets());

            RPP.ClearLists();
        }
        Movement_Packets_Master.Sort((x, y) => x.Packet_Number.CompareTo(y.Packet_Number));
        Action_Packet_Master.Sort((x, y) => x.Packet_Number.CompareTo(y.Packet_Number));

        SortBothPacketLists();

    }

    public void SortBothPacketLists()
    {
        Movement_Packets_Master.OrderBy(packet => packet.Packet_Number);
        Action_Packet_Master.OrderBy(packet => packet.Packet_Number);
    }


    public void PlayCurrentCloneViaFrameNumber(int _Frame_Number)
    {
        int NumOfClones = ListOfClones.Count;
        for (int i = 0; i < NumOfClones; i++)
        {
            if (ListOfClones[i].activeSelf == false)
            {
                //Clone is dead, might add something here.
            } else
            {
                GameObject clone = ListOfClones[i];
                int CurrentCloneNumber = i;

                int index = (CurrentCloneNumber) + (NumOfClones * _Frame_Number);

                clone.transform.position = Movement_Packets_Master[index].New_Pos;
                clone.transform.rotation = Movement_Packets_Master[index].New_Rot;

                clone.GetComponentInChildren<SphereCollider>().transform.rotation = Movement_Packets_Master[index].Camera_Rot;

                AHS.PlayNewActionServerRpc(clone.name, Action_Packet_Master[index].A_Action_ID);

            }

        }

    }

    public void SetupClonesForPlayback()
    {
        foreach(Move_Packet move_Packet in Movement_Packets_Master)
        {
            if (move_Packet.Control_Code == 101)
            {
                var Name = move_Packet.Packet_Number.Substring(5);
                var NewClone = Instantiate(clonePrefab, move_Packet.New_Pos, move_Packet.New_Rot);
                NewClone.name = Name;
                ListOfClones.Add(NewClone);
                NewClone.GetComponent<NetworkObject>().Spawn();
                NewClone.GetComponentInChildren<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
            }
        }
    }

    public void DeleteOldClones()
    {
        foreach(GameObject Clone in ListOfClones)
        {
            Clone.GetComponent<NetworkObject>().Despawn();
        }
        ListOfClones.Clear();
    }
};

//foreach (GameObject clone in ListOfClones)
//{
//    string FrameNumber_str = FrameNumber.ToString();
//    while (FrameNumber_str.Length < 5)
//    {
//        FrameNumber_str = "0" + FrameNumber_str;
//    }
//    //Play Movement first
//    clone.transform.position = Movement_Packets_Master.Find(x => x.Packet_Number == FrameNumber_str + clone.name).New_Pos;
//}
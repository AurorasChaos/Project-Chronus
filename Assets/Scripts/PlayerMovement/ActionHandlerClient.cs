using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActionHandlerClient : NetworkBehaviour
{
    ActionHandlerServer actionHandlerServer;
    [SerializeField]RecordPlayerPacket RPP;

    public bool CanBeReset = false;

    private Action_Packet MostRecentActionPacket = new Action_Packet(0, 0, null);

    public int TeamNumber;


    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
        actionHandlerServer = GameObject.FindObjectOfType<ActionHandlerServer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            actionHandlerServer.PlayNewActionServerRpc(transform.root.name, 201);
            SetMostRecentActionPacket(201, 102);
            CanBeReset = true;
        }
    }


    private void SetMostRecentActionPacket(int _Action_ID, int _Control_Code)
    {
        MostRecentActionPacket.A_Action_ID = _Action_ID;
        MostRecentActionPacket.Control_Code = _Control_Code;
    }

    public void ResetMostRecentActionPacket()
    {
        MostRecentActionPacket = new Action_Packet(0, 0, null);
        CanBeReset = false;
    }

    public Action_Packet ReturnMostRecentActionPacket()
    {
        return MostRecentActionPacket;
    }

}

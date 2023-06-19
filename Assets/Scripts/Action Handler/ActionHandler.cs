using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActionHandler : NetworkBehaviour
{
    private byte[] mostRecentPlayerAction;
    private RefListStorage RLS;

    public void SetupActionHandler(int _playerCount, RefListStorage _RLS)
    {
        mostRecentPlayerAction = new byte[_playerCount];
        RLS = _RLS;
    }

    [ServerRpc]
    public void ProcessNewActionByPlayerServerRpc(byte actionCode, int playerID) // Used for player actions
    {
        mostRecentPlayerAction[playerID] = actionCode;
        RunNewAction(RLS.getPlayerObject(playerID), actionCode);
    }

    [ServerRpc]
    public void ProcessNewActionByCloneServerRpc(byte actionCode, int authorGameObjectID) //Used for Clone actions
    {
        RunNewAction(RLS.getCloneObject(authorGameObjectID) , actionCode);
    }

    private void RunNewAction(GameObject authorGameObject, byte actionCode)
    {
        switch (actionCode)
        {
            case 200:
                break; 

            case 201:
                break;

            case 202:
                break;

            case 203:
                break;
        }
    }

    public void UpdateRecentPlayerActions()
    {
        RLS.setListMostRecentPlayerActions(mostRecentPlayerAction);
    }

    public void ResetMostRecentPlayerAction(int _playerCount)
    {
        mostRecentPlayerAction = new byte[_playerCount];
    }

}

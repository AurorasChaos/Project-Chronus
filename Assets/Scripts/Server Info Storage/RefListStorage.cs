using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RefListStorage : MonoBehaviour 
{
    private GameObject[] playerObjectRefList;
    private List<GameObject> cloneObjectRefList;
    private byte[] mostRecentPlayerAction;

    public GameObject[] getPlayerObjects()
    {
        return playerObjectRefList;
    }
    public GameObject getPlayerObject(int playerID)
    {
        return playerObjectRefList[playerID];
    }
    public void setPlayerObjects(GameObject[] _playerObjects)
    {
        playerObjectRefList= _playerObjects;
    }


    public List<GameObject> getCloneObjects()
    {
        return cloneObjectRefList;
    }
    public GameObject getCloneObject(int cloneID)
    {
        return cloneObjectRefList[cloneID];
    }
    public void addCloneObjects(GameObject[] _cloneObjects)
    {
        cloneObjectRefList.AddRange(_cloneObjects);
    }


    public byte[] getMostRecentPlayerActions()
    {
        return mostRecentPlayerAction;
    }
    public byte getMostRecentPlayerAction(int playerID)
    {
        return mostRecentPlayerAction[playerID];
    }
    public void setListMostRecentPlayerActions(byte[] playerActions)
    {
        mostRecentPlayerAction = playerActions;
    }

}

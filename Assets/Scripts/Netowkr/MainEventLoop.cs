using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainEventLoop : NetworkBehaviour
{
    [SerializeField] private PlayBackClones playBackClones;
    [SerializeField] private List<RecordPlayerPacket> RPP_List;
    [SerializeField] private int CurrentLoopFrameCount = 0;

    [SerializeField] private int RoundLengthInSeconds = 60;
    [SerializeField] private int RoundIntermissionInSeconds = 5;
    private int RoundLengthInFrames;
    private int RoundIntermissionInFrames;
    private int CurrentIntermissionFrameCount = 0;

    [SerializeField] private bool ShouldWeBeRecording = false;

    private bool FirstRunSetup = true;

    void Start()
    {
        if (!IsServer) return;
        playBackClones = GameObject.FindObjectOfType<PlayBackClones>();
        playBackClones.SetupNeccesaryControlCodes();
        RoundLengthInFrames = (int)Math.Floor(RoundLengthInSeconds * (1 / Time.fixedDeltaTime));
        RoundIntermissionInFrames = (int)Math.Floor(RoundIntermissionInSeconds * (1 / Time.fixedDeltaTime));

    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateRPPListServerRpc()
    {
        RPP_List.Clear();
        RPP_List.AddRange(GameObject.FindObjectsOfType<RecordPlayerPacket>());
    }

    void FixedUpdate()
    {
        if (!IsServer) return;

        if (!ShouldWeBeRecording)
        {
            if (NetworkManager.ConnectedClients.Count == 10)
            {
                CurrentIntermissionFrameCount = 0;
                foreach(RecordPlayerPacket RPP in RPP_List)
                {
                    int TeamNumber = 0;
                    RPP.TeamNumber = TeamNumber;
                    if (TeamNumber == 0) { TeamNumber = 1; } else { TeamNumber = 0; }
                }
                SpawnPlayersAtStartLocation();
            }
        }

        if (ShouldWeBeRecording)
        {
            if (CurrentIntermissionFrameCount < RoundIntermissionInFrames)
            {
                CurrentIntermissionFrameCount++;
                return;
            } else if (CurrentIntermissionFrameCount == RoundIntermissionInFrames)
            {
                foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    player.GetComponent<FirstPersonMovement>().canMove = true;
                    CurrentIntermissionFrameCount++;
                }
            }

            if (CurrentLoopFrameCount == 0)
            {
                BeginNewRound();
                CurrentLoopFrameCount = 1;
            }
            else if (CurrentLoopFrameCount < RoundLengthInFrames)
            {
                TellRPPsToRecordPacket();
                PlayNextCloneFrame();
                CurrentLoopFrameCount++;
            }
            else if (CurrentLoopFrameCount == RoundLengthInFrames)
            {
                EndCurrentCloneLoop();
                SpawnPlayersAtStartLocation();
                CurrentLoopFrameCount = 0;
                CurrentIntermissionFrameCount = 0;
            }
        }
    }


    private void BeginNewRound()
    {
        StartNextCloneLoop();
        UpdateRPPRoundCount();
        TellRPPsToRecordPacket();
        PlayNextCloneFrame();
    }

    private static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void SpawnPlayersAtStartLocation()
    {
        BoxCollider Team1SpawnPoint = GameObject.FindGameObjectWithTag("SpawnTeam1").GetComponent<BoxCollider>();
        BoxCollider Team2SpawnPoint = GameObject.FindGameObjectWithTag("SpawnTeam2").GetComponent<BoxCollider>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            BoxCollider colliderToUse;
            if (player.GetComponentInChildren<RecordPlayerPacket>().TeamNumber == 0)
            {
                colliderToUse = Team1SpawnPoint;
            }
            else
            {
                colliderToUse = Team2SpawnPoint;
            }
            player.transform.localPosition = RandomPointInBounds(colliderToUse.bounds);
            player.GetComponent<FirstPersonMovement>().canMove = false;
        }
    }

    private void UpdateRPPRoundCount()
    {
        foreach (RecordPlayerPacket RPP in RPP_List)
        {
            RPP.UpdateRoundNumber(true);
        }
    }

    private void TellRPPsToRecordPacket()
    {
        foreach (RecordPlayerPacket RPP in RPP_List)
        {
            RPP.RecordNewPacketClientRpc(CurrentLoopFrameCount);
        }
    }

    private void PlayNextCloneFrame()
    {
        playBackClones.PlayCurrentCloneViaFrameNumber(CurrentLoopFrameCount);
    }

    private void StartNextCloneLoop()
    {
        playBackClones.SetupClonesForPlayback();
    }

    private void EndCurrentCloneLoop()
    {
        playBackClones.DeleteOldClones();
        ProcessClonesFromLastLoop();
        UpdateRPPRoundCount();
    }

    private void ProcessClonesFromLastLoop()
    {
        playBackClones.UpdateMasterPacketLists();
    }

}

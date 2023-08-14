using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainEventHandler : NetworkBehaviour
{
    [SerializeField] private PlayBackClones playBackClones;
    [SerializeField] private List<RecordPlayerPacket> RPP_List;
    [SerializeField] private int CurrentLoopFrameCount = 0;

    [SerializeField] private int RoundLengthInSeconds;
    private int RoundLengthInFrames;

    [SerializeField] private bool ShouldWeBeRecording;

    void Start()
    {
        playBackClones = GameObject.FindObjectOfType<PlayBackClones>();
        playBackClones.SetupNeccesaryControlCodes();
        RoundLengthInFrames = (int)Math.Floor(RoundLengthInSeconds * (1 / Time.fixedDeltaTime));
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateRPPListServerRpc()
    {
        RPP_List.Clear();
        RPP_List.AddRange(GameObject.FindObjectsOfType<RecordPlayerPacket>());
    }

    private void Update()
    {
        if (IsClient && Input.GetKeyDown(KeyCode.F1)){
            ShouldWeBeRecording = !ShouldWeBeRecording;
        }
    }

    void FixedUpdate()
    {
        if (!IsServer)
        {
            return;
        }

        if (ShouldWeBeRecording)
        {
            if (CurrentLoopFrameCount == 0)
            {
                StartNextCloneLoop();
                TellRPPsToRecordPacket();
                PlayNextCloneFrame();

                CurrentLoopFrameCount = 1;

            } else if (CurrentLoopFrameCount < RoundLengthInFrames)
            {
                TellRPPsToRecordPacket();
                PlayNextCloneFrame();

                CurrentLoopFrameCount++;

            } else if (CurrentLoopFrameCount == RoundLengthInFrames)
            {
                EndCurrentCloneLoop();
                CurrentLoopFrameCount = 0;
                //ShouldWeBeRecording = false;
            }
        }
    }

    private void UpdateRPPRoundCount()
    {
        foreach(RecordPlayerPacket RPP in RPP_List)
        {
            RPP.UpdateRoundNumber(true);
        }
    }

    private void TellRPPsToRecordPacket()
    {
        foreach(RecordPlayerPacket RPP in RPP_List)
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


    public int ReturnNumberOfFrames()
    {
        return RoundLengthInFrames;
    }



}

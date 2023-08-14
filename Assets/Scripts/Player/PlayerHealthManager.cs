using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealthManager : NetworkBehaviour
{
    private float PlayerHealth;
    private float PlayerHealthDef = 100f;

    [SerializeField] private bool isPlayer;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            PlayerHealth = PlayerHealthDef;
        }
    }

    public void Update()
    {
        if (IsOwner)
        {
            if (PlayerHealth == 0)
            {
                if (isPlayer)
                {
                    GetComponent<FirstPersonMovement>().isAlive = false;
                } else
                {
                    transform.root.gameObject.SetActive(false);
                }
            }
        }
    }


    public void DecrementHealth(float DecrementValue)
    {
        PlayerHealth -= DecrementValue;
    }

    public void IncrementHealth(float IncrementValue)
    {
        PlayerHealth += IncrementValue;
    }

    public float GetPlayerHealth()
    {
        return PlayerHealth;
    }

    public void ResetHealth()
    {
        PlayerHealth = PlayerHealthDef;
    }



}

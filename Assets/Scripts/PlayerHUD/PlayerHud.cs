using Unity.Netcode;
using UnityEngine;
using Unity.Collections;
using TMPro;

public class PlayerHud : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();

    private bool overlaySet = false;
    public bool IsPlayer;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            if (transform.tag == "Player")
            {
                playersName.Value = $"Player {OwnerClientId}";
            } else
            {
                playersName.Value = transform.root.name;
            }

        }
        if (IsOwner)
        {

        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlays = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textMeshProUGUI in localPlayerOverlays)
        {
            if (textMeshProUGUI.name == "ID")
            {
                textMeshProUGUI.text = playersName.Value;
            } else if (textMeshProUGUI.name == "Health")
            {
                textMeshProUGUI.text = GetComponentInParent<PlayerHealthManager>().GetPlayerHealth().ToString();
            }
        }
    }

    public int Dampening = 2;
    private Transform target;

    private void Update()
    {
        if (transform.tag == "Clone")
            return;

        if (IsPlayer)
        {
            if (IsOwner && IsClient)
                GetComponent<Canvas>().enabled = false;
        }

        if (!overlaySet && !string.IsNullOrEmpty(playersName.Value))
        {
            SetOverlay();
            overlaySet = false;
        }

        if (!IsServer)
        {
            target = Camera.current.transform;
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Dampening);
        }
    }

}

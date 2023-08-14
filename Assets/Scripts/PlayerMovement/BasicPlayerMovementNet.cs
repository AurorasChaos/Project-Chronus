using Unity.Netcode;
using UnityEngine;

public class BasicPlayerMovementNet : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed = 3.5f;

    [SerializeField]
    private Vector2 defaultPositionrange = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<float> HorizontalFPos = new NetworkVariable<float>();
    [SerializeField]
    private NetworkVariable<float> HorizontalSPos = new NetworkVariable<float>();

    private void Start()
    {
        transform.position = new Vector3(Random.Range(defaultPositionrange.x, defaultPositionrange.y), 0,
            Random.Range(defaultPositionrange.x, defaultPositionrange.y));
    }

    private void Update()
    {
        if (IsServer)
        {
            UpdateServer();
        }

        if (IsClient && IsOwner)
        {
            UpdateClient();
        }

    }

    private void UpdateServer()
    {
        transform.position = new Vector3(transform.position.x + HorizontalSPos.Value, transform.position.y,
            transform.position.z + HorizontalFPos.Value);
    }

    private void UpdateClient()
    {
        float forwardBackward = 0;
        float leftRight = 0;

        forwardBackward += Input.GetAxis("Vertical") * walkSpeed;
        leftRight += Input.GetAxis("Horizontal") * walkSpeed;

        UpdateClientPositionServerRpc(forwardBackward, leftRight);
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float forwardBackward, float leftRight)
    {
        HorizontalFPos.Value = forwardBackward;
        HorizontalSPos.Value = leftRight;
    }


}

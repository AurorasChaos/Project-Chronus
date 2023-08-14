using UnityEngine;
using Unity.Netcode;

public class FirstPersonLook : NetworkBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    NetworkVariable<Quaternion> _local_Rotation_Trans = new NetworkVariable<Quaternion>();
    NetworkVariable<Quaternion> _local_Rotation_Char = new NetworkVariable<Quaternion>();

    void Reset()
    {
        if (IsServer)
        {
            // Get the character from the FirstPersonMovement in parents.
            character = GetComponentInParent<FirstPersonMovement>().transform;
        }
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
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

    private void UpdateClient()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        UpdateServerRpc(Quaternion.AngleAxis(-velocity.y, Vector3.right), Quaternion.AngleAxis(velocity.x, Vector3.up));
    }

    [ServerRpc]
    private void UpdateServerRpc(Quaternion LocalRotTrans, Quaternion LocalRotChar)
    {
        _local_Rotation_Trans.Value = LocalRotTrans;
        _local_Rotation_Char.Value = LocalRotChar;
    }

    private void UpdateServer()
    {
        transform.localRotation = _local_Rotation_Trans.Value;
        character.localRotation = _local_Rotation_Char.Value;

    }

}

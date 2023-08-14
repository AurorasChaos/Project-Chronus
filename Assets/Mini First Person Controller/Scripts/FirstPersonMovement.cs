using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FirstPersonMovement : NetworkBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    public bool isAlive = true;
    public bool canMove = false;

    [SerializeField] bool SpawnInPlayAreaDebug;


    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    [SerializeField]
    NetworkVariable<Vector2> _Target_Velocity = new NetworkVariable<Vector2>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            if (SpawnInPlayAreaDebug)
            {
                transform.root.position = new Vector3(0, 4, 100);
            }
        }
    }

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
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
        rigidbody.velocity = transform.rotation * new Vector3(_Target_Velocity.Value.x, rigidbody.velocity.y, _Target_Velocity.Value.y);
    }
    
    private void UpdateClient()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!isAlive || !canMove)
        {
            GetComponentInChildren<Renderer>().material.color = Color.red;
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.

        var Cam = GetComponentInChildren<Camera>();
        Cam.enabled = true;

        UpdateServerRpc(targetVelocity);

    }

    [ServerRpc]
    private void UpdateServerRpc(Vector2 targetVelocity)
    {
        _Target_Velocity.Value = targetVelocity;
    }

}
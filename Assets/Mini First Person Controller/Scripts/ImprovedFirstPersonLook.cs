using UnityEngine;
using Unity.Netcode;

public class ImprovedFirstPersonLook : NetworkBehaviour
{
    NetworkVariable<Quaternion> xServerQuat = new NetworkVariable<Quaternion>();
    NetworkVariable<Quaternion> yServerQuat = new NetworkVariable<Quaternion>();
    [SerializeField] Camera ThisPlayersCamera;
    [SerializeField] Transform MainParentForRot;

    public float Sensitivity
    {
        get { return Sensitivity; }
        set { Sensitivity = value; }
    }

    [Range(0.1f, 9f)] [SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents flipping when above 90 rotation")]
    [Range(0f, 90f)] [SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";


    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ThisPlayersCamera.enabled = true;
            //Destroy(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>());
            UpdateClient();
        }
        if (IsServer)
        {
            UpdateServer();
        }
        else
        {
            GetComponent<Camera>().enabled = false;
        }
    }

    private void UpdateClient()
    {
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        UpdateCameraLookRotServerRpc(xQuat, yQuat);
        ThisPlayersCamera.transform.localRotation = yQuat;
        MainParentForRot.localRotation = xQuat;
    }

    [ServerRpc]
    private void UpdateCameraLookRotServerRpc(Quaternion xQuat, Quaternion yQuat)
    {
        xServerQuat.Value = xQuat;
        yServerQuat.Value = yQuat;
    }

    private void UpdateServer()
    {
        ThisPlayersCamera.transform.localRotation = yServerQuat.Value;
        MainParentForRot.localRotation = xServerQuat.Value;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActionHandlerServer : NetworkBehaviour
{
    public void SetupActionHandlerServer()
    {
        //Might need this so gonna leave it here.
    }

    [ClientRpc]
    public void FireBulletTrailClientRpc(string owner, Ray ray)
    {
        GameObject Owner = GameObject.Find(owner);
        Debug.DrawRay(ray.origin, ray.direction * 500f , Color.red, 1);
        Owner.GetComponentInChildren<ParticleSystem>().Clear();
        Owner.GetComponentInChildren<ParticleSystem>().Play();
    }


    [ServerRpc(RequireOwnership = false)]
    public void PlayNewActionServerRpc(string owner, int A_Action_ID)
    {
        GameObject Owner = GameObject.Find(owner);
        switch (A_Action_ID)
        {
            case 201:
                //Gun 1
                Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f);
                float rayLength = 500f;

                Ray ray;
                if (Owner.transform.root.tag == "Clone")
                {
                    var StartPos = Owner.GetComponentInChildren<SphereCollider>().transform.position;
                    ray = new Ray(StartPos, Owner.GetComponentInChildren<SphereCollider>().transform.forward); 
                } else
                {
                    ray = Owner.GetComponentInChildren<Camera>().ViewportPointToRay(rayOrigin);
                }

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, rayLength))
                {
                    if (hit.collider.tag == "Player_Col" || hit.collider.tag == "Clone_Col")
                    {
                        Debug.LogWarning(owner + " hit a player/clone");
                        hit.transform.root.GetComponentInChildren<PlayerHealthManager>().DecrementHealth(25);
                    }

                    FireBulletTrailClientRpc(owner, ray);
                }

                break;


            default:
                break;
        }
    }
}

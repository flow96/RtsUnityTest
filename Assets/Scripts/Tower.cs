using RtsNetworkingLibrary.networking.messages.game;
using RtsNetworkingLibrary.unity.@base;
using UnityEngine;

public class Tower : BaseTower
{
    public float rotationSpeed = 5;
    public ParticleSystem particleSystem;
    public LayerMask layerMask;
    
    private GameObject turret;
    private GameObject target;

    public override void OnStart()
    {
        turret = transform.Find("turret").gameObject;
    }

    public override void ClientUpdate(bool isLocalPlayer)
    {
        if (isLocalPlayer)
        {
            if (attackCoolDown >= 0)
                attackCoolDown -= 1;
            if (target == null)
            {
                if (turret.transform.rotation.x > .01f || turret.transform.rotation.x <= 0)
                {
                    // Reset turret after shooting at airplanes
                    Vector3 rot = turret.transform.rotation.eulerAngles;
                    rot.x = 0;
                    turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, Quaternion.Euler(rot), Time.deltaTime * 10);
                }
                turret.transform.Rotate(0, 40 * Time.deltaTime, 0);
                CheckForEnemy();
            }
            else
            {
                if (Vector3.Distance(target.transform.position, transform.position) <= attackRange)
                {
                    //find the vector pointing from our position to the target
                    Vector3 _direction = (target.transform.position - transform.position).normalized;
 
                    //create the rotation we need to be in to look at the target
                    Quaternion _lookRotation = Quaternion.LookRotation(_direction);
 
                    //rotate us over time according to speed until we are in the required rotation
                    turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, _lookRotation, Time.deltaTime * 10);

                    if (attackCoolDown <= 0)
                    {
                        attackCoolDown = 60f / attackSpeed;
                        Rpc("Shoot", RpcInvokeMessage.RpcTarget.All);   
                    }
                }
                else
                {
                    target = null;
                }
            }
        }
    }

    public override void ServerUpdate()
    {
        
    }

    private void CheckForEnemy()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, attackRange,transform.forward, attackRange, layerMask.value);
        foreach (RaycastHit hit in hits)
        {
            NetworkMonoBehaviour networkMonoBehaviour;
            if (hit.transform != null && Vector3.Distance(hit.transform.position, transform.position) <= attackRange && hit.transform.TryGetComponent(out networkMonoBehaviour))
            {
                if (networkMonoBehaviour.clientId != clientId && networkMonoBehaviour is AttackableUnit)
                {
                    target = hit.transform.gameObject;
                    return;
                }
            }
        }
        target = null;
    }

    public void Shoot()
    {
        Debug.Log("Shooting");
        particleSystem.Play();
        if (IsLocalPlayer)
        {
            // Also trigger enemy hit
            if (target != null)
            {
                target.GetComponent<AttackableUnit>().Rpc("TakeDamage", RpcInvokeMessage.RpcTarget.All, attackDamage);
            }
        }
    }

}

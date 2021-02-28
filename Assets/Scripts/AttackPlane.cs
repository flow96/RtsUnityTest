using RtsNetworkingLibrary.networking.messages.game;
using RtsNetworkingLibrary.unity.@base;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlane : BaseUnit
{
    
    
    public float rotationSpeed = 5;
    private GameObject target;
    public ParticleSystem particleSystem;
    public LayerMask layerMask;

    private float initialHealthBarSize;
    
    public override void OnStart()
    {
        if (!IsLocalPlayer)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            //GetComponent<Rigidbody>().detectCollisions = false;
            healthBarCanvas.SetActive(false);
            if (foregroundBar != null)
            {
                initialHealthBarSize = foregroundBar.rect.width;
            }
        }
    }


    public override void ClientUpdate(bool isLocalPlayer)
    {
        if (isLocalPlayer)
        {
            if(healthBarCanvas.activeSelf != IsSelected)
                healthBarCanvas.SetActive(IsSelected);
            if (attackCoolDown >= 0)
                attackCoolDown -= 1;
            if (target == null)
            {
                if (transform.rotation.x > .01f || transform.rotation.x <= 0)
                {
                    // Reset yaw after shooting at ground enemies
                    Vector3 rot = transform.rotation.eulerAngles;
                    rot.x = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * 10);
                }
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
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);

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
        if (foregroundBar != null)
        {
            //Rect rect = foregroundBar.rect;
            //float width = initialHealthBarSize / maxHealth * health;
            Vector3 scale = foregroundBar.localScale;
            scale.x = (health / maxHealth);
            foregroundBar.localScale = scale;
        }
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

    public override void ServerUpdate()
    {
        
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

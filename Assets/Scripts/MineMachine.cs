using RtsNetworkingLibrary.networking.manager;
using RtsNetworkingLibrary.unity.@base;
using UnityEngine;

public class MineMachine : AttackableUnit
{
    private PlayerStats _playerStats;

    [Header("Mining Stats")]
    public int miningSpeed = 1;

    private float _mineCoolDown = 0;
    
    public override void OnStart()
    {
        base.OnStart();
        attackDamage = 0;
        attackRange = 0;
        attackSpeed = 0;
        _playerStats = GameObject.Find("PlayerStats_" + NetworkManager.Instance.ClientId).GetComponent<PlayerStats>();
    }

    public override void ClientUpdate(bool isLocalPlayer)
    {
        if (isLocalPlayer)
        {
            _mineCoolDown += Time.deltaTime;
            if (_mineCoolDown > .4f)
            {
                _mineCoolDown = 0;
                _playerStats.gold += miningSpeed;
            }
        }
    }

    public override void ServerUpdate()
    {
        
    }

}

using RtsNetworkingLibrary.unity.@base;
using UnityEngine;

public class PlayerStats : NetworkMonoBehaviour
{

    public int gold = 100;

    private float _coolDown = 0;
    
    public override void ClientUpdate(bool isLocalPlayer)
    {
        _coolDown += Time.deltaTime;
        if (_coolDown > .8f)
        {
            gold++;
            _coolDown = 0;
        }
    }

    public override void ServerUpdate()
    {
        
    }
}

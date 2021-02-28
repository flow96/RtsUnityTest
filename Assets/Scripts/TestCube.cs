using RtsNetworkingLibrary.unity.@base;
using UnityEngine;

public class TestCube : NetworkMonoBehaviour
{
    
    public override void ClientUpdate(bool isLocalPlayer)
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-transform.right * Time.deltaTime * 10);
            }else if(Input.GetKey(KeyCode.D))
                transform.Translate(transform.right * Time.deltaTime * 10);
        }
    }

    public override void ServerUpdate()
    {
        
    }
}

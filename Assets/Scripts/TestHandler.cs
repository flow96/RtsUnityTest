using RtsNetworkingLibrary.networking.manager;
using UnityEngine;

public class TestHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NetworkManager.Instance.Instantiate("test",null);
        }
    }
}

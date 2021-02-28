using RtsNetworkingLibrary.networking.manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class VehiclePanel : MonoBehaviour
{
    public GameObject[] vehicles;
    private PlayerStats _playerStats;
    private bool ignoreFirstClick = true;

    public Vector3 spawnPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        ignoreFirstClick = true;
    }

    private void OnEnable()
    {
        ignoreFirstClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerStats == null)
        {
            _playerStats = GameObject.Find("PlayerStats_" + NetworkManager.Instance.ClientId)?
                .GetComponent<PlayerStats>();
            if (_playerStats == null)
                return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!ignoreFirstClick && !EventSystem.current.IsPointerOverGameObject())
            {
                gameObject.SetActive(false);
            }
            ignoreFirstClick = false;    
        }
    }

    public void Instantiate(int index)
    {
        if (_playerStats == null)
            return;
        if (index == 0 && _playerStats.gold >= 350)
        {
            _playerStats.gold -= 350;
            for (int i = 0; i < 50; i++)
            {
                NetworkManager.Instance.Instantiate("Plane1", spawnPosition);
                Debug.Log("Spawned at: " + spawnPosition);    
            }
        }
    }
}

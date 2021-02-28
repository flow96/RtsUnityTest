using RtsNetworkingLibrary.networking.manager;
using UnityEngine;
using UnityEngine.UI;

public class GoldCanvasManager : MonoBehaviour
{
    public Text lblGold;

    private int lastGold;
    private PlayerStats _playerStats;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerStats == null)
        {
            _playerStats = GameObject.Find("PlayerStats_" + NetworkManager.Instance.ClientId)?.GetComponent<PlayerStats>();
            if (_playerStats == null)
                return;
        }
        if (lastGold != _playerStats.gold)
        {
            lblGold.text = "Gold: " + _playerStats.gold;
            lastGold = _playerStats.gold;
        }
    }
}

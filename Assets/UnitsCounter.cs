using System.Collections;
using System.Collections.Generic;
using RtsNetworkingLibrary.networking.manager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UnitsCounter : MonoBehaviour
{

    private Text lblUnits;
    // Start is called before the first frame update
    void Start()
    {
        lblUnits = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        lblUnits.text = "Units: " + NetworkManager.Instance.SpawnedObjects.Values.Count;
    }
}

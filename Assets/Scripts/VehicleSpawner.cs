using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    private GameObject vehiclePanel;
    
    private void Start()
    {
        vehiclePanel = GameObject.Find("Canvas").transform.Find("VehiclePanel").gameObject;
    }


    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnMouseUp()
    {
        Debug.Log("Showing panel");
        vehiclePanel.SetActive(true);
        vehiclePanel.GetComponent<VehiclePanel>().spawnPosition = transform.position;
    }

}

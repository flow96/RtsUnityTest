using System;
using RtsNetworkingLibrary.networking.manager;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float cameraSpeed = 15;
    public float scrollSpeed = 60;
    private float multiplier = 1;
    private Vector3 nextPos;
    private Vector3 mouseStartPos;
    private Transform _spawnPoint;

    private void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("spawn");
        if (spawnPoints != null)
        {
            if (spawnPoints.Length > NetworkManager.Instance.ClientId)
            {
                Debug.Log("Using: " + NetworkManager.Instance.ClientId);
                _spawnPoint = spawnPoints[NetworkManager.Instance.ClientId].transform;
                transform.position = _spawnPoint.position;
                transform.rotation = _spawnPoint.rotation;
            }
        }
        nextPos = transform.position;
        NetworkManager.Instance.Instantiate("PlayerStats", g =>
        {
            g.name = "PlayerStats_" + NetworkManager.Instance.ClientId;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(Input.GetAxis("Vertical")) > 0)
        {
            nextPos += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed;
        }
        if (Math.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            nextPos += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed;
        }
        if (Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            nextPos -= transform.up * (Input.GetAxis("Mouse ScrollWheel") * 10) * Time.deltaTime * scrollSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            multiplier = 2.5f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            multiplier = 1;

        transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * cameraSpeed * .5f * multiplier);

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            mouseStartPos = Input.mousePosition;
        }
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Delta: " + (mouseStartPos - Input.mousePosition));
            float deltaX = (float)Math.Round((mouseStartPos.x - Input.mousePosition.x) * -.002f, 1);
            transform.Rotate(transform.up * deltaX, Space.World);
        }
    }
}

using RtsNetworkingLibrary.networking.manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] industries;

    [Header("UI")] public GameObject[] uiButtons;
    
    private int buildIndex = -1;
    private GameObject activeGameObject;
    private BuildPreview _buildPreview;

    private PlayerStats _playerStats;
    private bool[] uiButtonEnabled;
    
    public void SetIndex(int index)
    {
        Debug.Log("Setting build index to: " + index);
        if (index == buildIndex || index > industries.Length - 1 || index < 0)
        {
            buildIndex = -1;
            Destroy(activeGameObject);
            activeGameObject = null;
            _buildPreview = null;
        }
        else
        {
            if (activeGameObject != null)
            {
                Destroy(activeGameObject);
            }
            buildIndex = index;
            activeGameObject = Instantiate(industries[buildIndex]);
            _buildPreview = activeGameObject.GetComponent<BuildPreview>();
        }
    }

    private bool CheckBuildCondition(int index)
    {
        if (index >= 0 && activeGameObject != null)
            return _playerStats.gold >= _buildPreview.price;
        return true;
    }

    private void Start()
    {
        uiButtonEnabled = new bool[uiButtons.Length];
        for (int i = 0; i < uiButtons.Length; i++)
        {
            uiButtonEnabled[i] = true;
        }
    }

    private void Update()
    {
        if (_playerStats == null)
        {
            _playerStats = GameObject.Find("PlayerStats_" + NetworkManager.Instance.ClientId)?
                .GetComponent<PlayerStats>();
            if (_playerStats == null)
                return;
        }
        if (activeGameObject != null && _playerStats != null)
        {
            if (!CheckBuildCondition(buildIndex))
            {
                SetIndex(-1);
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.point = new Vector3((int)hit.point.x, 0, (int)hit.point.z);
                activeGameObject.transform.position = hit.point;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && CheckBuildCondition(buildIndex))
            {
                _playerStats.gold -= _buildPreview.price;
                NetworkManager.Instance.Instantiate(industries[buildIndex].name, activeGameObject.transform.position, activeGameObject.transform.rotation);
            }else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetIndex(-1);
            }
        }

        if (_playerStats != null)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_playerStats.gold < industries[i].GetComponent<BuildPreview>().price)
                {
                    if (uiButtonEnabled[i])
                    {
                        uiButtonEnabled[i] = false;
                        uiButtons[i].GetComponent<Image>().color = Color.grey;
                    }
                }
                else
                {
                    if (!uiButtonEnabled[i])
                    {
                        uiButtonEnabled[i] = true;
                        uiButtons[i].GetComponent<Image>().color = Color.white;
                    }
                }
            }
        }
    }
}

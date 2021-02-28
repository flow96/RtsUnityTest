using System.Collections.Generic;
using System.Linq;
using RtsNetworkingLibrary.networking.manager;
using RtsNetworkingLibrary.unity.@base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/*
 * With the help of: https://gamedevacademy.org/rts-unity-tutorial/
 */
public class SelectionHandler : MonoBehaviour
{

    public RectTransform selectionBox;
    public RectTransform canvas;
    public GameObject vehiclePanel;
    private Vector2 _startPos;
    private Camera _camera;
    private float _scaleX, _scaleY;
    
    private readonly List<NetworkMonoBehaviour> _selectionList = new List<NetworkMonoBehaviour>();

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _scaleX = canvas.rect.width / _camera.pixelRect.width;
        _scaleY = canvas.rect.height / _camera.pixelRect.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _startPos = new Vector2(Input.mousePosition.x * _scaleX, Input.mousePosition.y * _scaleY);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        if (Input.GetMouseButtonUp(1) && _selectionList.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.point = new Vector3((int)hit.point.x, 0, (int)hit.point.z);
                int offsetX = (int)(_selectionList.Count > 5 ? 2.5 : _selectionList.Count / 2) * 4;
                int offsetY = (int)(_selectionList.Count / 5 / 2) * 4;
                for (int i = 0; i < _selectionList.Count; i++)
                {
                    if (_selectionList[i] != null && _selectionList[i].gameObject != null)
                    {
                        _selectionList[i].gameObject.GetComponent<NavMeshAgent>().destination 
                            = new Vector3((int)hit.point.x - offsetX + ((i % 5) * 4), 0, (int)hit.point.z - offsetY + ((int)(i / 5) * 4));    
                    }
                    else
                    {
                        _selectionList.RemoveAt(i--);
                        continue;
                    }
                }
            }
        }

        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            UpdateSelectionBox(new Vector2(Input.mousePosition.x * _scaleX, Input.mousePosition.y * _scaleY));
    }
    
    // called when we are creating a selection box
    void UpdateSelectionBox (Vector2 curMousePos)
    {
        if(!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);
 
        float width = (curMousePos.x - _startPos.x);
        float height = (curMousePos.y - _startPos.y);

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = _startPos + new Vector2(width / 2, height / 2);
    }
    
    // called when we release the selection box
    void ReleaseSelectionBox ()
    {
        foreach (NetworkMonoBehaviour monoBehaviour in _selectionList)
        {
            monoBehaviour.IsSelected = false;
        }
        _selectionList.Clear();
        
        selectionBox.gameObject.SetActive(false);
 
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        if (NetworkManager.Instance == null)
            return;
        foreach(NetworkMonoBehaviour unit in NetworkManager.Instance.SpawnedObjects.Values.Where(behaviour => behaviour is BaseUnit))
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
        
            if(screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                _selectionList.Add(unit);
                unit.IsSelected = true;
                Debug.Log("Selecting: " + unit);
            }
        }
    }
    
}

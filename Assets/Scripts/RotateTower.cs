using UnityEngine;

public class RotateTower : MonoBehaviour
{

    public GameObject turret;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turret.transform.Rotate(0, 10 * Time.deltaTime, 0);
    }
}

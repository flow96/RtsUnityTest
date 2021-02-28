using UnityEngine;

public class BuildPreview : MonoBehaviour
{

    public int price = 100;
    
    private Color colorGreen;
    private Color colorRed;

    void Start()
    {
        colorGreen = Color.grey;
        colorRed = new Color(.8f,0, 0, .03f);   
        UpdateColor(colorGreen);
    }

    private void OnTriggerEnter(Collider other)
    {
        UpdateColor(colorRed);
    }

    private void OnTriggerStay(Collider other)
    {
        UpdateColor(colorRed);
    }

    private void OnTriggerExit(Collider other)
    {
        UpdateColor(colorGreen);
    }

    private void UpdateColor(Color color)
    {
        foreach (Renderer re in GetComponents<Renderer>())
        {
            re.material.color = color;
        }
        foreach (Renderer re in GetComponentsInChildren<Renderer>())
        {
            re.material.color = color;
            foreach (Renderer re2 in re.GetComponentsInChildren<Renderer>())
            {
                re2.material.color = color;
            }
        }
    }
}

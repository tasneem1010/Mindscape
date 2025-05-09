using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    public float pulseSpeed = 2f;
    private Material mat;
    private bool isPulsing = false;
    private Color baseColor;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        baseColor = mat.color; // Start with the cube's initial color
    }

    void Update()
    {
        if (isPulsing)
        {
            float emission = Mathf.PingPong(Time.time * pulseSpeed, 1.5f) + 0.5f;
            mat.SetColor("_EmissionColor", baseColor * emission);
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black); // Turn off emission when not pulsing
        }
    }

    public void SetPulsing(bool isPulsing)
    {
        this.isPulsing = isPulsing;
    }

    public void SetBaseColor(Color color)
    {
        if (mat == null)
            mat = GetComponent<Renderer>().material;
        
        baseColor = color; // Update the base color for pulsing
        mat.color = color; // Update the material color to reflect the new base color
    }
}

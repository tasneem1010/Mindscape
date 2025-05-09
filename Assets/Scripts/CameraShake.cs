using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f; // Duration of the shake
    public float shakeAmount = 0.1f;    // Magnitude of the shake

    private Vector3 originalPos;
    private float shakeTimeRemaining;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        shakeTimeRemaining = shakeDuration;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector3 randomPoint = originalPos + (Random.insideUnitSphere * shakeAmount);
            transform.localPosition = new Vector3(randomPoint.x, randomPoint.y, originalPos.z);

            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }
}

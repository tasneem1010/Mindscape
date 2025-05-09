using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastDetector : MonoBehaviour
{
    // Create a UnityEvent
    public UnityEvent onHit;

    public void OnRaycastHit()
    {
        onHit?.Invoke();
    }
}

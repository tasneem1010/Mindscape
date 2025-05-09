using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    public float animationDuration = 0.2f; // Duration of the scale animation
    public float scaleMultiplier = 1.2f;   // How much the cube scales up

    private Vector3 originalScale;
    private float currentTime = 0f;
    private bool isAnimating = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void Animate()
    {
        if (!isAnimating)
            StartCoroutine(AnimateScale());

    }

    private System.Collections.IEnumerator AnimateScale()
    {
        isAnimating = true;

        // Scale up
        while (currentTime < animationDuration / 2f)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / (animationDuration / 2f);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * scaleMultiplier, t);
            yield return null;
        }

        // Scale down
        currentTime = 0f;
        while (currentTime < animationDuration / 2f)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / (animationDuration / 2f);
            transform.localScale = Vector3.Lerp(originalScale * scaleMultiplier, originalScale, t);
            yield return null;
        }

        // Reset
        transform.localScale = originalScale;
        currentTime = 0f;
        isAnimating = false;
    }
}

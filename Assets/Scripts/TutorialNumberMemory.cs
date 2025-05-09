using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNumberMemory : MonoBehaviour
{
    private Image[] cubes; // Array of Image components to represent the cubes
    public float highlightDuration = 1.5f; // Duration to highlight each cube
    public float timeBetweenHighlights = 0.3f; // Time between highlighting each cube
    public float numberDisplayTime = 3f;
    public bool isLose = false;
    public Image cursorImage;
    public TextMeshProUGUI numText;
    private string num = "954";
    void Start()
    {
        cubes = GetComponentsInChildren<Image>(); // Get all Image components representing cubes
       
        StartCoroutine(ShowSequence()); // Start showing the sequence
    }


    IEnumerator MoveCursorToCube(int index)
    {
        Image targetCube = cubes[index]; // Get the cube by its index
        Vector3 targetPosition = targetCube.transform.position; // Get the target position

        float elapsedTime = 0f;
        float moveDuration = 0.5f; // Duration to move the cursor to the cube

        // Move the cursor to the cube's position
        Vector3 initialPosition = cursorImage.transform.position;
        while (elapsedTime < moveDuration)
        {
            cursorImage.transform.position = Vector3.Lerp(initialPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cursorImage.transform.position = targetPosition; // Ensure it reaches the final position
    }

    IEnumerator ShowSequence()
    {
        numText.text = num;
        numText.gameObject.SetActive(true);
        yield return new WaitForSeconds(numberDisplayTime);
        numText.gameObject.SetActive(false);

        cursorImage.gameObject.SetActive(true);
        int[] sequence = new int[3] { 8, 4, 3 };
        if (!isLose)
        {
            
            foreach (int index in sequence)
            {
                yield return StartCoroutine(MoveCursorToCube(index)); // Move cursor to cube
                yield return StartCoroutine(HighlightCube(index, Color.green)); // Highlight each cube in sequence
                yield return new WaitForSeconds(timeBetweenHighlights); // Wait before highlighting the next one
            }
        }
        else
        {
            
            yield return StartCoroutine(MoveCursorToCube(sequence[0])); // Move cursor to cube
            yield return StartCoroutine(HighlightCube(sequence[0], Color.green)); // Highlight each cube in sequence
            yield return new WaitForSeconds(timeBetweenHighlights); // Wait before highlighting the next one

            yield return StartCoroutine(MoveCursorToCube(9)); // Move cursor to cube
            yield return StartCoroutine(HighlightCube(9, Color.red)); // Highlight each cube in sequence
            yield return new WaitForSeconds(timeBetweenHighlights); // Wait before highlighting the next one
        }
        cursorImage.gameObject.SetActive(false);
        
        StartCoroutine(ShowSequence()); // Start showing the sequence again
    }

    // Highlight a specific cube by changing its color
    IEnumerator HighlightCube(int index, Color highlightColor)
    {
        Image cube = cubes[index]; // Get the cube by its index
        Color originalColor = cube.color; // Save the original color
        cube.color = highlightColor; // Set the cube to the highlight color

        // Wait for the highlight duration
        yield return new WaitForSeconds(highlightDuration);

        cube.color = originalColor; // Reset the cube's color to its original
    }
}

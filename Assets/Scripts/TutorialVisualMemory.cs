using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialVisualMemory : MonoBehaviour
{
    private Image[] cubes; // Array of Image components to represent the cubes
    public float highlightDuration = 1.5f; // Duration to highlight each cube
    public float timeBetweenHighlights = 0.3f; // Time between highlighting each cube
    public float timeBetweenRounds = 1.5f; // Time between rounds for repeating the pattern

    private List<int> sequence = new List<int>(); // Sequence of highlighted cubes (represented by their indices)
    private int currentStep = 0; // Current step in the sequence
    private bool isShowingSequence = false; // Whether the sequence is being shown or not
    public bool showIndefinitely = true;
    public bool isLose = false;
    public Image cursorImage;

    void Start()
    {
        cubes = GetComponentsInChildren<Image>(); // Get all Image components representing cubes
        sequence = new List<int>()
        {
            6, 3, 0
        };
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
        isShowingSequence = true;
        do
        {
            Color originalColor = cubes[0].color; // Save the original color

            foreach (int index in sequence)
            {
                Image cube = cubes[index]; // Get the cube by its index
                cube.color = Color.white; // Set the cube to the highlight color

            }
            yield return new WaitForSeconds(highlightDuration);

            foreach (int index in sequence)
            {
                Image cube = cubes[index]; // Get the cube by its index
                cube.color = originalColor; // Reset the cube's color to its original

            }

            // Wait between rounds before repeating the sequence
            yield return new WaitForSeconds(timeBetweenRounds);
        }
        while (showIndefinitely); // Repeating the pattern indefinitely for the tutorial

        cursorImage.gameObject.SetActive(true);
        if (!isLose)
        {
            foreach (int index in sequence.Reverse<int>())
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

            yield return StartCoroutine(MoveCursorToCube(4)); // Move cursor to cube
            yield return StartCoroutine(HighlightCube(4, Color.red)); // Highlight each cube in sequence
            yield return new WaitForSeconds(timeBetweenHighlights); // Wait before highlighting the next one
        }
        cursorImage.gameObject.SetActive(false);
        
        isShowingSequence = false;
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

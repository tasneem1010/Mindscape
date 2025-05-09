using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NumberMemory : MonoBehaviour
{
    public GameObject cubePrefab;
    public int currentLength = 1; // Start with at least 1 digit
    public float highlightDuration = 1f;
    public float numberDisplayDuration = 3f; // Duration before hiding the number
    public float timeBetweenRounds = 1.5f;
    public float timeBeforeReset = 3.0f;
    public GameObject LivesPrefab;

    private LivesManager livesManager;

    public TextMeshProUGUI numText;
    public TextMeshProUGUI Level;

    private List<GameObject> cubes = new List<GameObject>();
    private float spacing = 0.5f;
    private string targetNumber = ""; // String representation of the number
    private string playerInput = "";
    private bool isInputAllowed = false; // Lock input until number is hidden


    public Color defaultColor = Color.gray;
    public Color highlightColor = Color.gray;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    private const int AverageScore = 7;
    private int HighScore = 0;
    private int CurrentScore = 0; 
    void Start()
    {
        livesManager = LivesPrefab.GetComponent<LivesManager>(); // Get lives manager script
        GenerateKeyPad();
        StartNewRound();
    }

    void GenerateKeyPad()
    {
        int num = 1;
        float offsetX = (3 - 1) * spacing / 2f; // Center the 3 columns
        float offsetY = (4 - 1) * spacing / 2f; // Center the 4 rows

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                GameObject cube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity, transform);
                cube.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                cube.transform.localPosition = new Vector3(x * spacing - offsetX, -y * spacing + offsetY, 0);
                cube.transform.localRotation = Quaternion.identity;
                cube.transform.GetChild(1).GetComponent<TextMeshPro>().text = num.ToString();

                int index = num; // Local copy for lambda
                cube.GetComponent<RaycastDetector>().onHit.AddListener(() => StartCoroutine(CheckCube(index)));
                cubes.Add(cube);

                num++;
            }
        }

        // Add zero to the keypad
        GameObject zeroCube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity, transform);
        zeroCube.transform.localPosition = new Vector3(0, -(3 * spacing) + offsetY, 0);
        zeroCube.transform.localRotation = Quaternion.identity;
        zeroCube.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        zeroCube.transform.GetChild(1).GetComponent<TextMeshPro>().text = "0";
        zeroCube.GetComponent<RaycastDetector>().onHit.AddListener(() => StartCoroutine(CheckCube(0)));
        cubes.Add(zeroCube);
    }

    void StartNewRound()
    {
        GenerateNumber();
        StartCoroutine(HideNumberAfterDelay());
    }

    void GenerateNumber()
    {
        // Ensure the generated number matches the current length
        int power = (int)Mathf.Pow(10, currentLength - 1);
        targetNumber = "" + Random.Range(power, power * 10);
        numText.text = targetNumber.ToString();

        playerInput = string.Empty;
        isInputAllowed = false; // Lock input
        Debug.Log($"Generated Number: {targetNumber}");
    }

    IEnumerator HideNumberAfterDelay()
    {
        yield return new WaitForSeconds(numberDisplayDuration);

        // Hide the number
        numText.text = string.Empty;
        isInputAllowed = true; // Allow input
        Debug.Log("Number is now hidden. Player can start input.");
    }

    IEnumerator CheckCube(int number)
    {
        if (!isInputAllowed) yield break;

        // Highlight the selected cube
        GameObject cube = cubes.Find(c => c.transform.GetChild(1).GetComponent<TextMeshPro>().text == number.ToString());
        if (cube != null)
        {
            // Highlight cube based on correctness
            Color feedbackColor = (number.ToString()[0] == targetNumber[playerInput.Length]) ? correctColor : incorrectColor;
            StartCoroutine(HighlightCube(cube, feedbackColor));

            // Check if the current input matches the corresponding digit
            char expectedDigit = targetNumber[playerInput.Length]; // Current digit to match
            if (number.ToString()[0] == expectedDigit)
            {
                playerInput += number.ToString();
                Debug.Log($"Correct so far: {playerInput}");
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayCorrect();

                // Check if the input is complete
                if (playerInput.Length == targetNumber.Length)
                {
                    Debug.Log("Correct! Moving to the next round.");
                    StartCoroutine(EndRound(true));

                }
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayIncorrect();

                Debug.Log($"Wrong Input! Expected {expectedDigit}, but got {number}.");
                StartCoroutine(EndRound(false));

            }
        }
    }

    IEnumerator HighlightCube(GameObject cube, Color feedbackColor)
    {
        // Change the color of the cube
        Renderer renderer = cube.GetComponent<Renderer>();

        renderer.material.color = feedbackColor;
        // Wait for the highlight duration
        yield return new WaitForSeconds(highlightDuration);

        // Reset the color back to white after the duration
        renderer.material.color = defaultColor;
    }

    IEnumerator EndRound(bool isCompleted)
    {
        isInputAllowed = false;
        yield return new WaitForSeconds(0.3f);

        // De-highlight all cubes
        foreach (var cube in cubes)
        {
            var pulse = cube.GetComponent<PulseEffect>();
            pulse.SetPulsing(false);
            cube.GetComponent<Renderer>().material.color = defaultColor;
        }


        // Show message and wait for it to complete
        string message;
        if (isCompleted)
        {
            message = "ROUND COMPLETE!";
        }
        else if (livesManager.lives > 0)
        {
            message = "TRY AGAIN!";

        }
        else
        {
            message = "GAME OVER!";
        }
        DynamicMessageManager.Instance.ShowMessage(message, timeBetweenRounds);
        yield return new WaitForSeconds(timeBetweenRounds);


        // Proceed to next round or reset game
        if (isCompleted)
        {
            CurrentScore++;
            currentLength++;
           
        }
        else
        {
            if (livesManager.lives > 0)
            {
                livesManager.TakeHeart();
            }
            else // End game
            {
                Debug.Log("Resetting Game...");


                if (CurrentScore > HighScore)
                {
                    HighScore = CurrentScore;
                }

                string s1 = "Your current Score: " + CurrentScore + "\nYour high Score: " + HighScore;
                DynamicMessageManager.Instance.ShowMessage(s1, timeBeforeReset);
                yield return new WaitForSeconds(timeBeforeReset);

                string s = "Your High Score: " + HighScore + "\nAverage Score: " + AverageScore;
                DynamicMessageManager.Instance.ShowMessage(s, timeBeforeReset);
                yield return new WaitForSeconds(timeBeforeReset);

                CurrentScore = 0;
                currentLength = 1;
                livesManager.ResetHearts();
            }
        }
        Level.text = "Level: " + CurrentScore;
        StartNewRound();
        isInputAllowed = true;
    }
}

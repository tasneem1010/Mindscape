using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject LivesPrefab;

    public int gridSize = 3;
    public float spacing = 0.33f;
    public float highlightDuration = 1.5f;
    public float timeBetweenRounds = 1.5f;
    public float timeBeforeReset = 3.0f;

    public TextMeshProUGUI Level;


    public Color defaultColor = Color.gray;
    public Color highlightColor = Color.blue; // Use a different highlight color for this game mode
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    private List<GameObject> cubes = new List<GameObject>();
    private List<int> tiles = new List<int>();
    private int currentStep = 0;
    private int round = 0;
    private bool isHighlighting = false;
    private bool isInputLocked = false;
    private Vector3 originalPos;

    private const int AverageScore = 10;
    private int CurrentScore = 0;
    private int HighScore = 0;
    private LivesManager livesManager;
    private int tries = 3;
    private int HightlightCount = 3;

    void Start()
    {
        livesManager = LivesPrefab.GetComponent<LivesManager>();
        originalPos = transform.position;
        GenerateGrid();
        StartCoroutine(ShowHighlightedCubes());
    }

    void GenerateGrid()
    {
        Debug.Log("Generating grid: " + gridSize);

        // Clear any existing cubes
        foreach (var cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();

        // Calculate the offset for positioning cubes so that they are centered
        float offset = (gridSize - 1) * spacing / 2;

        // Generate new cubes for the updated grid size
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = y * gridSize + x;
                GameObject cube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity, transform);

                // Position the cube based on the grid size and spacing
                cube.transform.localPosition = new Vector3(x * spacing - offset, -y * spacing + offset, 0);
                cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                // Set the default color of the cube
                Renderer renderer = cube.GetComponent<Renderer>();
                renderer.material.color = defaultColor;

                // Add raycast detection for cube clicks
                cube.GetComponent<RaycastDetector>().onHit.AddListener(() => StartCoroutine(CheckCube(index)));
                cubes.Add(cube);
            }
        }

        // Adjust the grid's position to ensure it fits the new cubes and is centered in the scene
        AdjustGridPosition();
    }

    // Adjust the position of the entire grid to be centered
    void AdjustGridPosition()
    {
        // Calculate the correct offset based on the new grid size
        float offset = (gridSize - 1) * spacing / 2;

        // Set the position of the entire grid object to make sure it is centered
        transform.position = originalPos;

        transform.position += new Vector3(0, 4.7f, 0);
    }

    // Highlight random cubes at the start of each round
    IEnumerator ShowHighlightedCubes()
    {
        isHighlighting = true;

        // Select random cubes to highlight
        tiles.Clear();
        for (int i = 0; i < HightlightCount; i++)
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, cubes.Count);
            } while (tiles.Contains(newIndex)); // Ensure no duplicates

            tiles.Add(newIndex);
        }

        // Highlight the cubes
        foreach (int index in tiles)
        {
            var pulse = cubes[index].GetComponent<PulseEffect>();
            Renderer renderer = cubes[index].GetComponent<Renderer>();

            pulse.SetBaseColor(highlightColor); // Update the pulse base color
            pulse.SetPulsing(true);
        }

        yield return new WaitForSeconds(highlightDuration);

        // De-highlight the cubes
        foreach (int index in tiles)
        {
            var pulse = cubes[index].GetComponent<PulseEffect>();
            Renderer renderer = cubes[index].GetComponent<Renderer>();
            renderer.material.color = defaultColor; // Reset to default color
            pulse.SetBaseColor(defaultColor); // Reset pulse base color
            pulse.SetPulsing(false);
        }

        currentStep = 0;
        isHighlighting = false;
    }

    // Check if the player clicked the correct cube
    IEnumerator CheckCube(int cubeIndex)
    {
        if (isHighlighting || isInputLocked)
        {
            yield break;
        }

        isInputLocked = true;

        if (tiles.Contains(cubeIndex))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayCorrect();
            // Correct selection
            StartCoroutine(HighlightCube(cubeIndex, correctColor));
            currentStep++;

            if (currentStep >= tiles.Count)
            {
                Debug.Log("Round Complete!");
                StartCoroutine(EndRound(true));
            }
        }
        else
        {

            // Incorrect selection
            StartCoroutine(HighlightCube(cubeIndex, incorrectColor));

            if (tries > 1)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayIncorrect();

                tries--;
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayLose();

                StartCoroutine(EndRound(false));
                Debug.Log("Incorrect Cube! Try Again.");
            }
        }

        isInputLocked = false;
    }

    // Highlight the cube for feedback (correct or incorrect)
    IEnumerator HighlightCube(int index, Color feedbackColor)
    {
        var pulse = cubes[index].GetComponent<PulseEffect>();
        Renderer renderer = cubes[index].GetComponent<Renderer>();

        pulse.SetBaseColor(feedbackColor); // Update the pulse base color
        pulse.SetPulsing(true);

        yield return new WaitForSeconds(highlightDuration);
        if (renderer == null) yield break;
        renderer.material.color = defaultColor; // Reset to default color
        pulse.SetBaseColor(defaultColor); // Reset pulse base color
        pulse.SetPulsing(false);
    }

    // Handle end of round (successful or failed)
    IEnumerator EndRound(bool isCompleted)
    {
        
        isInputLocked = true;

        // Reset cubes
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

        // Proceed to the next round or reset the game
        if (isCompleted)
        {
            CurrentScore++;
            round++;
            HightlightCount++;
            if (round % 2 == 0)
            {
                gridSize++;
                

                // Generate the new grid of cubes
                GenerateGrid();
            }
        }
        else 
        {
            if (livesManager.lives > 0)
            {
                livesManager.TakeHeart(); //decrement heartss
            }
            else // End game
            {
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

                livesManager.ResetHearts();
                gridSize = 3; // Reset grid size
                HightlightCount = 3;
     
            }
        }
        Level.text = "Level: " + CurrentScore;
        tries = 3;
        GenerateGrid();
        StartCoroutine(ShowHighlightedCubes());
        isInputLocked = false;
    }

}

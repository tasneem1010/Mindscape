using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject LivesPrefab;

    public int gridSize = 4;
    public float spacing = 0.33f;
    public int sequenceLength = 1;
    public float highlightDuration = 1.5f;
    public float timeBetweenRounds = 1.5f;
    public float timeBeforeReset = 3.0f;


    public Color defaultColor = Color.gray;
    public Color highlightColor = Color.gray;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    public TextMeshProUGUI Level;

    private List<GameObject> cubes = new List<GameObject>();
    private List<int> sequence = new List<int>();
    private int currentStep = 0;
    private bool isShowingSequence = false;
    private bool isInputLocked = false;

    private const int AverageScore = 9;
    private int CurrentScore = 0;
    private int HighScore = 0;
    private LivesManager livesManager;



    void Start()
    {
        livesManager = LivesPrefab.GetComponent<LivesManager>();
        GenerateGrid();
        GenerateSequence();
        StartCoroutine(ShowSequence());
    }

    void GenerateGrid()
    {
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
    }

    void GenerateSequence()
    {
        sequence.Clear();
        int previousIndex = -1;

        for (int i = 0; i < sequenceLength; i++)
        {
            int newIndex;

            // Ensure the new index is not the same as the previous one
            do
            {
                newIndex = Random.Range(0, cubes.Count);
            } while (newIndex == previousIndex);

            sequence.Add(newIndex);
            previousIndex = newIndex; // Update the previous index
        }
    }


    IEnumerator ShowSequence()
    {
        isShowingSequence = true;
        yield return new WaitForSeconds(1.5f);

        foreach (int index in sequence)
        {
            yield return StartCoroutine(HighlightCube(index, highlightColor));
            yield return new WaitForSeconds(0.3f);
        }

        currentStep = 0;
        isShowingSequence = false;
    }


    IEnumerator CheckCube(int cubeIndex)
    {
        if (isShowingSequence || isInputLocked || currentStep == sequence.Count)
            yield break;
        

        isInputLocked = true;

        if (sequence[currentStep] == cubeIndex)
        {
            StartCoroutine(HighlightCube(cubeIndex, correctColor));
            currentStep++;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayCorrect();

            if (currentStep >= sequence.Count)
            {
                Debug.Log("Sequence Completed!");
                StartCoroutine(EndRound(true));
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayIncorrect();

            StartCoroutine(HighlightCube(cubeIndex, incorrectColor));
            Debug.Log("Wrong Cube!");
            StartCoroutine(EndRound(false));
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

        renderer.material.color = defaultColor; // Reset to default color
        pulse.SetBaseColor(defaultColor); // Reset pulse base color
        pulse.SetPulsing(false);
    }

    // Handle end of round (successful or failed)
    IEnumerator EndRound(bool isCompleted)
    {
        isInputLocked = true;
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
            sequenceLength++;
            int newIndex;
            do
            {
                newIndex = Random.Range(0, cubes.Count);
            } while (newIndex == sequence.Last());

            sequence.Add(newIndex);
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
                sequenceLength = 1;
                GenerateSequence();
            }
        }

        Level.text = "Level: " + CurrentScore;
        currentStep = 0;
        StartCoroutine(ShowSequence());
        isInputLocked = false;
    }
}

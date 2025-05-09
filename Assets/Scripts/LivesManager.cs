using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public int lives = 3; // Initial number of lives
    public GameObject heartPrefab; // Reference to the heart prefab
    public GameObject heartsContainer; // Container for the hearts UI

    private RectTransform containerRectTransform;

    private void Start()
    {
        containerRectTransform = heartsContainer.GetComponent<RectTransform>();
        SetupHeartsUI();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeHeart();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddHeart();
        }
    }
    private void SetupHeartsUI()
    {
        // Clear the existing children if any
        foreach (Transform child in heartsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Set the size of the container based on the number of lives
        UpdateContainerSize();

        // Instantiate hearts
        for (int i = 0; i < lives; i++)
        {
            AddHeartToUI(i);
        }
    }

    private void UpdateContainerSize()
    {
        float containerWidth = lives * 30; // 25 width per heart + 5 spacing
        containerRectTransform.sizeDelta = new Vector2(containerWidth, 20); // Height is fixed
    }

    private void AddHeartToUI(int index)
    {
        GameObject newHeart = Instantiate(heartPrefab, heartsContainer.transform);
        RectTransform heartRect = newHeart.GetComponent<RectTransform>();

        // Set heart position
        float xPosition = 15 + index * 30; // Spacing starts at 15
        heartRect.anchoredPosition = new Vector2(xPosition, 10f);

        // Set size of the heart
        heartRect.sizeDelta = new Vector2(25, 25);
    }

    public void TakeHeart()
    {
        if (lives <= 0) return;

        lives--;
        DecreaseHeartUI();
    }

    public void AddHeart()
    {
        if (lives >= heartsContainer.transform.childCount)
            return; // Prevent exceeding the max number of hearts in the container

        Transform heartToFill = heartsContainer.transform.GetChild(lives); // Get the next unfilled heart
        Image fillImage = heartToFill.GetChild(0).GetComponent<Image>(); // Assuming the fill image is the first child
        fillImage.fillAmount = 1; // Fill the heart

        lives++;
    }
    private void DecreaseHeartUI()
    {
        if (heartsContainer.transform.childCount > lives)
        {
            // Find the heart to "deactivate"
            Transform heartToDeactivate = heartsContainer.transform.GetChild(lives);
            Image fillImage = heartToDeactivate.GetChild(0).GetComponent<Image>(); // Assuming the fill image is the first child
            fillImage.fillAmount = 0;
        }
    }
    public void ResetHearts()
    {
        lives = 3;
        SetupHeartsUI();
    }
}

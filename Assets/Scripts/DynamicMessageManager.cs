using TMPro;
using UnityEngine;

public class DynamicMessageManager : MonoBehaviour
{
    public static DynamicMessageManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI messageText;

    private void Awake()
    {
        // Singleton pattern to ensure only one manager exists across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
      //  DontDestroyOnLoad(gameObject); // Persist this across scenes
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messageText == null)
        {
            Debug.LogError("DynamicMessageText is not assigned!");
            return;
        }

        messageText.text = message; // Update the text
        messageText.gameObject.SetActive(true); // Ensure the text is visible
        Invoke(nameof(HideMessage), duration); // Hide after the duration
    }

    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}

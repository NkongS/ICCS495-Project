using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIControllerGuide : MonoBehaviour
{
    public Button forwardButton;
    public Button homeButton;

    public AudioSource backgroundSound;
    public AudioSource HoverSound;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        forwardButton = root.Q<Button>("forward-button");
        homeButton = root.Q<Button>("home-button");

        forwardButton.clicked += forwardButtonPressed;
        homeButton.clicked += homeButtonPressed;

        AddHoverSound(forwardButton);
        AddHoverSound(homeButton);  
    }

    void AddHoverSound(VisualElement button)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => HoverSoundOn());
    }

    void forwardButtonPressed()
    {
        SceneManager.LoadScene("NextGuideScene");
    }

    void homeButtonPressed()
    {
        SceneManager.LoadScene("MenuScene");
    }


    void HoverSoundOn(float startTime = 0f)
    {
    if (HoverSound != null)
    {
        if (startTime > 0f && startTime < HoverSound.clip.length)
        {
            HoverSound.time = startTime;
        }
        HoverSound.Play();
    }
    else
    {
        Debug.LogError("Hover sound is not assigned!");
    }
    }

}

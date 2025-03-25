using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIControllerGuide2 : MonoBehaviour
{
    public Button backwardButton;
    public Button homeButton;

    public AudioSource backgroundSound;
    public AudioSource HoverSound;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        backwardButton = root.Q<Button>("backward-Button");
        homeButton = root.Q<Button>("home-button");

        backwardButton.clicked += backwardButtonPressed;
        homeButton.clicked += homeButtonPressed;

        AddHoverSound(backwardButton);
        AddHoverSound(homeButton);  
    }

    void AddHoverSound(VisualElement button)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => HoverSoundOn());
    }

    void backwardButtonPressed()
    {
        SceneManager.LoadScene("GuideScene");
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

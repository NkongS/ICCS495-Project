using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{

    public Button selectButton;
    public Button backButton;

    public AudioSource backgroundSound;
    public AudioSource HoverSound;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        selectButton = root.Q<Button>("selectOne-button");
        backButton = root.Q<Button>("home-button");

        selectButton.clicked += selectButtonPressed;
        backButton.clicked += homeButtonPressed;

        AddHoverSound(selectButton);
        AddHoverSound(backButton);  
    }

    void AddHoverSound(VisualElement button)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => HoverSoundOn());
    }

    void selectButtonPressed()
    {
        SceneManager.LoadScene("LevelSelectionScene");
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

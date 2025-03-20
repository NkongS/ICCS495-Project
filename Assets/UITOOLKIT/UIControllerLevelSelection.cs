using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIControllerLevelSelection : MonoBehaviour
{
    public Button levelOneButton;
    public Button levelTwoButton;
    public Button levelThreeButton;
    public Button SoundOnButton;
    public Button SoundOffButton;
    public Button HomeButton;
    public Button BackButton;

    public AudioSource backgroundSound;
    public AudioSource HoverSound;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        levelOneButton = root.Q<Button>("levelOne-button");
        levelTwoButton = root.Q<Button>("levelTwo-button");
        levelThreeButton = root.Q<Button>("levelThree-button");

        HomeButton = root.Q<Button>("home-button");
        BackButton = root.Q<Button>("back-button");

        SoundOnButton = root.Q<Button>("soundOn-button");
        SoundOffButton = root.Q<Button>("soundOff-button");


        levelOneButton.clicked += levelOneButtonPressed;
        levelTwoButton.clicked += levelTwoButtonPressed;
        levelThreeButton.clicked += levelThreeButtonPressed;

        HomeButton.clicked += homeButtonPressed;
        BackButton.clicked += backButtonPressed;

        SoundOnButton.clicked += SoundOnButtonPressed;
        SoundOffButton.clicked += SoundOffButtonPressed;

        AddHoverSound(levelOneButton);
        AddHoverSound(levelTwoButton);
        AddHoverSound(levelThreeButton);
        AddHoverSound(HomeButton);
        AddHoverSound(BackButton);
        AddHoverSound(SoundOnButton);
        AddHoverSound(SoundOffButton);
    }

    void AddHoverSound(VisualElement button)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => HoverSoundOn());
    }

    void levelOneButtonPressed() 
    {
        SceneManager.LoadScene("Main");
        Debug.Log("Level One Button Pressed");
    }

    void levelTwoButtonPressed() 
    {
        SceneManager.LoadScene("LevelTwo");
    }

    void levelThreeButtonPressed() 
    {
        SceneManager.LoadScene("LevelThree");
    }

    void homeButtonPressed() 
    {
        SceneManager.LoadScene("MenuScene");
    }

    void backButtonPressed() 
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    void SoundOnButtonPressed()
    {
        if (backgroundSound != null && !backgroundSound.isPlaying)
        {
            backgroundSound.Play();
        }
    }

    void SoundOffButtonPressed()
    {
        if (backgroundSound != null && backgroundSound.isPlaying)
        {
            backgroundSound.Stop();
        }
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

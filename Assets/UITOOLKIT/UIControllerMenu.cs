using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button playButton;
    public Button guideButton;
    public Button quitButton;
    public Button IncreaseVolButton;
    public Button DecreaseVolButton;

    public AudioSource backgroundSound;
    public AudioSource buttonSound;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        playButton = root.Q<Button>("play-button");
        guideButton = root.Q<Button>("guide-button");
        quitButton = root.Q<Button>("quit-button");
        IncreaseVolButton = root.Q<Button>("increaseVol-button");
        DecreaseVolButton = root.Q<Button>("decreaseVol-button");

        playButton.clicked += playButtonPressed;
        guideButton.clicked += guideButtonPressed;
        quitButton.clicked += quitButtonPressed;
        IncreaseVolButton.clicked += IncreaseVolume;
        DecreaseVolButton.clicked += DecreaseVolume;

        AddHoverSound(playButton);
        AddHoverSound(guideButton);
        AddHoverSound(quitButton);
        AddHoverSound(IncreaseVolButton);
        AddHoverSound(DecreaseVolButton);
    }

    void AddHoverSound(VisualElement button)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => PlayButtonSound(0.2f));
    }

    void playButtonPressed() 
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    void guideButtonPressed() 
    {
        SceneManager.LoadScene("GuideScene");
    }

    void quitButtonPressed() 
    {
        Application.Quit();
    }

    void IncreaseVolume()
    {
        if (backgroundSound != null && !backgroundSound.isPlaying)
        {
            backgroundSound.Play();
        }
    }

    void DecreaseVolume()
    {
        if (backgroundSound != null && backgroundSound.isPlaying)
        {
            backgroundSound.Stop();
        }
    }

    void PlayButtonSound(float startTime = 0f)
    {
    if (buttonSound != null)
    {
        if (startTime > 0f && startTime < buttonSound.clip.length)
        {
            buttonSound.time = startTime;
        }
        buttonSound.Play();
    }
    else
    {
        Debug.LogError("Button sound is not assigned!");
    }
}
}

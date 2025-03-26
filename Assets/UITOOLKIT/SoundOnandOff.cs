using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SoundOnandOff : MonoBehaviour
{
    public Button SoundOnButton;
    public Button SoundOffButton;

    public AudioSource backgroundSound;
    public AudioSource HoverSound;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        SoundOnButton = root.Q<Button>("SoundOn-button");
        SoundOffButton = root.Q<Button>("SoundOff-button");

        SoundOnButton.clicked += SoundOnButtonPressed;
        SoundOffButton.clicked += SoundOffButtonPressed;

        AddHoverSound(SoundOnButton);
        AddHoverSound(SoundOffButton);
    }

    void AddHoverSound(VisualElement button)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => HoverSoundOn());
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

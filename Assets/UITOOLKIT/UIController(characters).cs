using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{

    public Button selectButtonOne;
    public Button selectButtonTwo;
    public Button selectButtonThree;
    public Button backButton;

    public AudioSource backgroundSound;
    public AudioSource HoverSound;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        selectButtonOne = root.Q<Button>("selectOne-button");
        selectButtonTwo = root.Q<Button>("selectTwo-button");
        selectButtonThree = root.Q<Button>("selectThree-button");
        backButton = root.Q<Button>("home-button");

        selectButtonOne.clicked += () => selectButtonPressed(0); // Character 1
        selectButtonTwo.clicked += () => selectButtonPressed(1); // Character 2
        selectButtonThree.clicked += () => selectButtonPressed(2); // Character 3
        backButton.clicked += homeButtonPressed;

        AddHoverSound(selectButtonOne);
        AddHoverSound(selectButtonTwo);
        AddHoverSound(selectButtonThree);
        AddHoverSound(backButton);  
    }

    void AddHoverSound(VisualElement button)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => HoverSoundOn());
    }

    void selectButtonPressed(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        PlayerPrefs.Save();

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

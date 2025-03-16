using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        playButton = root.Q<Button>("play-button");
        optionsButton = root.Q<Button>("options-button");
        exitButton = root.Q<Button>("exit-button");

        playButton.clicked += playButtonPressed;
        optionsButton.clicked += optionsButtonPressed;
        exitButton.clicked += exitButtonPressed;
    }

    void playButtonPressed() 
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    void optionsButtonPressed() 
    {
        SceneManager.LoadScene("Options");
    }

    void exitButtonPressed() 
    {
        Application.Quit();
    }
}

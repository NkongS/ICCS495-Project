using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    // public GameObject[] characters; 
    public Button character1Button;
    public Button character2Button;
    public Button character3Button;
    public Button selectButton;
    public Button backButton;

    private int selectedCharacterIndex = 0;

    void Start()
    {
        // character1Button.onClick.AddListener(() => SelectCharacter(0));
        // character2Button.onClick.AddListener(() => SelectCharacter(1));
        // character3Button.onClick.AddListener(() => SelectCharacter(2));
        selectButton.onClick.AddListener(ConfirmSelection);
        backButton.onClick.AddListener(BackToMainMenu);

        // UpdateCharacterDisplay();
    }

    void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
        Debug.Log("Selected Character: " + (index + 1));
        // UpdateCharacterDisplay(); 
    }

    // void UpdateCharacterDisplay()
    // {
    //     for (int i = 0; i < characters.Length; i++)
    //     {
    //         characters[i].SetActive(i == selectedCharacterIndex);
    //     }
    // }

    void ConfirmSelection()
    {
        // PlayerPrefs.SetInt("SelectedCharacter", selectedCharacterIndex);
        // PlayerPrefs.Save();
        // Debug.Log("Character " + (selectedCharacterIndex + 1) + " Selected!");
        // Optionally load the game scene
        SceneManager.LoadScene("LevelSelectionScene");
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

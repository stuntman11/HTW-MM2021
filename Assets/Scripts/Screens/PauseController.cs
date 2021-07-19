using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages elements and voice input of the pause screen
/// </summary>
public class PauseController : MonoBehaviour
{
    /// <summary>Reference to the scenes PauseScreen object</summary>
    public GameObject PauseScreen;

    private KeywordRecognizer recognizer;
    
    private void Awake()
    {
        string[] keywords = new string[] { "fortsetzen", "verlassen" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;
        
        Button btnResume = PauseScreen.transform.Find("ContinueBtn").GetComponent<Button>();
        btnResume.onClick.AddListener(OnPauseToggle);

        Button btnReturnToMenu = PauseScreen.transform.Find("ReturnBtn").GetComponent<Button>();
        btnReturnToMenu.onClick.AddListener(ReturnToMenu);

        CommandController command = GetComponent<CommandController>();
        command.OnCommand += OnCommand;

        PauseScreen.SetActive(false);
    }

    private void OnCommand(string command)
    {
        if (command.Equals("pause"))
        {
            recognizer.Start();
            OnPauseToggle();
        }
    }

    private void OnRecognition(PhraseRecognizedEventArgs args) {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));

        if (command.Equals("fortsetzen")) OnPauseToggle();
        else if (command.Equals("verlassen")) ReturnToMenu();
    }

    private void OnPauseToggle()
    {
        CommandController command = GetComponent<CommandController>();
        PauseScreen.SetActive(!PauseScreen.activeSelf);

        if (PauseScreen.activeSelf)
        {
            command.PauseVoiceInput();
            Time.timeScale = 0;
        }
        else
        {
            recognizer.Stop();
            command.ResumeVoiceInput();
            Time.timeScale = 1;
        }
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy() => recognizer.Dispose();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages elements and voice input of the game over screen
/// </summary>
public class GameOverController : MonoBehaviour
{
    private KeywordRecognizer recognizer;

    void Awake()
    {
        string[] keywords = new string[] { "neustart", "zurück" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;

        Button btnRestart = GameObject.Find("BtnRestartGame").GetComponent<Button>();
        btnRestart.onClick.AddListener(Restart);

        Button btnReturn = GameObject.Find("BtnReturnToMenu").GetComponent<Button>();
        btnReturn.onClick.AddListener(Return);
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));

        if (command.Equals("neustart")) Restart();
        else if (command.Equals("zurück")) Return();
    }

    private void Restart()
    {
        MakeNoSound.LoadLevel(MakeNoSound.ActiveLevel);
    }

    private void Return()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

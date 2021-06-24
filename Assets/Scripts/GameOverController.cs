using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private KeywordRecognizer recognizer;
    void Awake()
    {
        string[] keywords = new string[] { "restart", "return" };
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

        if (command.Equals("restart")) Restart();
        else if (command.Equals("return")) Return();
    }

    private void Restart()
    {
        SceneManager.LoadScene("Level1");
    }

    private void Return()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

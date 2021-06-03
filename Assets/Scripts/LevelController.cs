using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public delegate void TickEventHandler(string command);

    public event TickEventHandler OnTick;
    private KeywordRecognizer recognizer;

    void Awake()
    {
        string[] keywords = new string[] { "vor", "zurück", "links", "rechts", "pause" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));

        if (command.Equals("pause"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (IsTickSource(command))
        {
            OnTick.Invoke(command);
        }
    }

    private bool IsTickSource(string command)
    {
        return command.Equals("vor") || command.Equals("zurück") || command.Equals("links") || command.Equals("right");
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

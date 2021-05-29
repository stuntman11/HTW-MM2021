using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    private KeywordRecognizer recognizer;

    void Awake()
    {
        string[] keywords = new string[] { "start", "beenden" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;

        Button btnExit = GameObject.Find("BtnExit").GetComponent<Button>();
        btnExit.onClick.AddListener(Exit);
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        Debug.Log(string.Format("Recognized: '{0}'", args.text));
        if (args.text.Equals("beenden")) Exit();
    }

    void OnEnable()
    {
        recognizer.Start();
    }

    void OnDisable()
    {
        recognizer.Stop();
    }

    private void Exit()
    {
        UnityEditor.EditorApplication.ExitPlaymode();
        Application.Quit();
    }
}

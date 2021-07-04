using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private KeywordRecognizer recognizer;

    void Awake()
    {
        string[] keywords = new string[] { "laden", "beenden", "neu" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;

        Button btnLoad = GameObject.Find("BtnLoad").GetComponent<Button>();
        btnLoad.onClick.AddListener(MakeNoSound.LoadSave);

        Button btnExit = GameObject.Find("BtnExit").GetComponent<Button>();
        btnExit.onClick.AddListener(Exit);

        Button btnNew = GameObject.Find("BtnNew").GetComponent<Button>();
        btnNew.onClick.AddListener(MakeNoSound.NewSave);
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));

        if (command.Equals("beenden")) Exit();
        else if (command.Equals("laden")) MakeNoSound.LoadSave();
        else if (command.Equals("neu")) MakeNoSound.NewSave();
    }

    private void Exit()
    {
        UnityEditor.EditorApplication.ExitPlaymode();
        Application.Quit();
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

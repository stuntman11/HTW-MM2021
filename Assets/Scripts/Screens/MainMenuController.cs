using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages elements and voice input of the main menu screen
/// </summary>
public class MainMenuController : MonoBehaviour
{
    private KeywordRecognizer recognizer;

    void Awake()
    {
        string[] keywords = new string[] { "laden", "beenden", "neu" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;

        Button btnLoad = GameObject.Find("BtnLoad").GetComponent<Button>();
        btnLoad.onClick.AddListener(Load);
        btnLoad.interactable = MakeNoSound.HasSaveState;

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
        else if (command.Equals("laden")) Load();
        else if (command.Equals("neu")) MakeNoSound.NewSave();
    }

    private void Exit()
    {
        /// <summary>UnityEditor.EditorApplication.ExitPlaymode();</summary>
        Application.Quit();
    }

    private void Load()
    {
        if (MakeNoSound.HasSaveState)
        {
            MakeNoSound.LoadSave();
            SceneManager.LoadScene("LevelSelection");
        }
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

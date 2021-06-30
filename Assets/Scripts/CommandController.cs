using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class CommandController : MonoBehaviour
{
    public delegate void CommandEventHandler(string command);
    
    public event CommandEventHandler OnCommand;

    private PhraseRecognizer recognizer;
    private bool isPaused = false;

    void Awake()
    {
        recognizer = new GrammarRecognizer(Application.streamingAssetsPath + "/grammar.xml", ConfidenceLevel.Rejected);
        recognizer.OnPhraseRecognized += OnRecognition;
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        if (!isPaused)
        {
            OnCommand(args.text);
        }
    }

    public void ResumeVoiceInput() => isPaused = false;

    public void PauseVoiceInput() => isPaused = true;

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

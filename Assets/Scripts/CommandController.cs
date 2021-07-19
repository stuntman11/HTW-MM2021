using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// Manages the voice input for command grammar
/// </summary>
public class CommandController : MonoBehaviour
{
    /// <summary>Describes a handler for the CommandEvent</summary>
    public delegate void CommandEventHandler(string command);

    /// <summary>Event that is executed when a voice command was recognized</summary>
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

    /// <summary>
    /// Resumes the world command recognizer
    /// </summary>
    public void ResumeVoiceInput() => isPaused = false;

    /// <summary>
    /// Pauses the world command recognizer
    /// </summary>
    public void PauseVoiceInput() => isPaused = true;

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

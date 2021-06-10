using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private static string[] NUMBERS = new string[]
    {
        "zwei",
        "drei",
        "vier",
        "fünf",
        "sechs",
        "sieben",
        "acht",
        "neun"
    };

    private static string[] COMMANDS = new string[]
    {
        "hoch",
        "runter",
        "links",
        "rechts",
        "warten",
        "pause"
    };
    
    public delegate void TickEventHandler(string command);

    private Queue<string> commands = new Queue<string>();
    private double timer = 0;
    private int multiply = 1;
    public event TickEventHandler OnTick;
    private KeywordRecognizer recognizer;

    void Awake()
    {
        string[] keywords = new string[COMMANDS.Length + NUMBERS.Length];
        COMMANDS.CopyTo(keywords, 0);
        NUMBERS.CopyTo(keywords, COMMANDS.Length);
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (commands.Count > 0 && timer >= 1)
        {
            timer = 0;
            string command = commands.Dequeue();
            OnTick.Invoke(command);
        }
    }

    private int GetMultiplier(string command)
    {
        for (int i = 0; i < NUMBERS.Length; i++)
        {
            if (command.Equals(NUMBERS[i]))
            {
                return i + 2;
            }
        }
        return -1;
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));
        int multiply = GetMultiplier(command);

        if (command.Equals("pause"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (multiply != -1)
        {
            this.multiply = multiply;
        }
        else if (IsTickSource(command))
        {
            for (int i = 0; i < this.multiply; i++)
            {
                commands.Enqueue(command);
            }
            this.multiply = 1;
        }
    }

    private bool IsTickSource(string command)
    {
        return command.Equals("hoch") || command.Equals("runter") || command.Equals("links") || command.Equals("rechts") || command.Equals("warten");
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

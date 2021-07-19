using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages elements and voice input of the level finished screen
/// </summary>
public class NextLevelController : MonoBehaviour
{
    private KeywordRecognizer recognizer;
    
    void Awake()
    {
        string[] keywords = new string[] { "weiter", "zurück" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;

        int level = MakeNoSound.ActiveLevel;
        int levelScore = MakeNoSound.Score;
        int lastHighscore = MakeNoSound.GetHighscore(MakeNoSound.ActiveLevel);
        MakeNoSound.FinishLevel();

        Text finishedText = GameObject.Find("LevelFinishedText").GetComponent<Text>();
        finishedText.text = string.Format("Level {0} Finished", level + 1);

        Text scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        string highscoreTxt = FormatHighscore(levelScore, lastHighscore);
        scoreText.text = string.Format("Dein Score: {0}\n\nHighscore: {1}", levelScore, highscoreTxt);

        Button nextLevelBtn = GameObject.Find("NextLvlButton").GetComponent<Button>();
        nextLevelBtn.onClick.AddListener(EnterNextLevel);
        nextLevelBtn.interactable = MakeNoSound.HasNextLevel;

        Button exitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(Exit);
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));

        if (command.Equals("zurück")) Exit();
        else if (command.Equals("weiter")) EnterNextLevel();
    }

    private string FormatHighscore(int score, int lastHighscore)
    {
        if (score > lastHighscore)
        {
            return string.Format("*{0} (Last: {1})", score, lastHighscore);
        }
        return lastHighscore.ToString();
    }

    private void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void EnterNextLevel()
    {
        MakeNoSound.LoadLevel(MakeNoSound.ActiveLevel);
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

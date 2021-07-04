using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevelController : MonoBehaviour
{
    private KeywordRecognizer recognizer;
    
    private void Awake()
    {
        string[] keywords = new string[] { "weiter, zurück" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;

        Text highScoreText = GameObject.Find("HighscoreText").GetComponent<Text>();
        highScoreText.text = "Best Score of Level " + (MakeNoSound.Level + 1).ToString() + ": " + MakeNoSound.GetHighscore(MakeNoSound.Level);

        Button nextLevelBtn = GameObject.Find("NextLvlButton").GetComponent<Button>();
        nextLevelBtn.onClick.AddListener(EnterNextLevel);

        Button exitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(Exit);

        MakeNoSound.AdvanceLevel();

    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));

        if (command.Equals("zurück")) Exit();
        else if (command.Equals("weiter")) EnterNextLevel();
    }

    private void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void EnterNextLevel()
    {
        int nextLevel = MakeNoSound.Level;

        if (nextLevel < MakeNoSound.LevelCount)
        {
            MakeNoSound.LoadLevel(nextLevel);
        }
    }
}

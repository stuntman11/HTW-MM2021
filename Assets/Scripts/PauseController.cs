using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject PauseScreen;
    private KeywordRecognizer recognizer;
    

    private void Awake()
    {
        string[] keywords = new string[] {"fortsetzen", "pause", "verlassen" };
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;
        

        Button btnResume = GameObject.Find("Panel/ContinueBtn").gameObject.GetComponent<Button>();
        btnResume.onClick.AddListener(onPauseToggle);

        Button btnReturnToMenu = GameObject.Find("Panel/ReturnToMenuBtn").gameObject.GetComponent<Button>();
        btnReturnToMenu.onClick.AddListener(returnToMenu);
        PauseScreen.SetActive(false);
    }

    private void OnRecognition(PhraseRecognizedEventArgs args) {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));
        if (command.Equals("pause")  || (command.Equals("fortsetzen") && PauseScreen.activeSelf)) onPauseToggle();
        if (command.Equals("verlassen")) returnToMenu();
    }

     private void onPauseToggle()
    {
        PauseScreen.SetActive(!PauseScreen.activeSelf);
        if (PauseScreen.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

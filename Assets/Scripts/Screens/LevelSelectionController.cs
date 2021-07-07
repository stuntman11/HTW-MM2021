using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class LevelSelectionController : MonoBehaviour
{
    private readonly string[] NUMERIC = new string[]
    {
        "eins", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun"
    };

    public RectTransform LevelButton;

    private PhraseRecognizer recognizer;

    private void Awake()
    {
        recognizer = new GrammarRecognizer(Application.streamingAssetsPath + "/level_select.xml", ConfidenceLevel.Rejected);
        recognizer.OnPhraseRecognized += OnRecognition;
        GameObject btnContainer = GameObject.Find("BtnContainer");
        
        for(int i = 0; i <= MakeNoSound.UnlockedLevels; i++)
        {
            Vector3 position = new Vector3(0, i * -100, -1);
            RectTransform btnLevelTransform = Instantiate(LevelButton, btnContainer.transform);
            Button btnLevel = btnLevelTransform.GetComponent<Button>();
            Text btnTxt = btnLevelTransform.GetComponentInChildren<Text>();
            string levelTxt = string.Format("Level {0}", i + 1);

            btnLevelTransform.name = levelTxt;
            btnLevelTransform.anchoredPosition = position;
            btnLevelTransform.anchorMin = new Vector2(0.5f, 1);
            btnLevelTransform.anchorMax = new Vector2(0.5f, 1);
            btnLevelTransform.pivot = new Vector2(0.5f, 1);

            btnLevel.onClick.AddListener(() => MakeNoSound.LoadLevel(i));
            btnTxt.text = levelTxt;
        }
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;

        if (command.Equals("zurück"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            for (int i = 0; i <= MakeNoSound.UnlockedLevels; i++)
            {
                if (command.Contains(NUMERIC[i]))
                {
                    MakeNoSound.LoadLevel(i);
                    break;
                }
            }
        }
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}

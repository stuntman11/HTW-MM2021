using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishTrigger : EntityBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int levelFinishedScore = MakeNoSound.Score;

        if (MakeNoSound.GetHighscore(MakeNoSound.Level) < levelFinishedScore)
        {
            MakeNoSound.SetHighscore(MakeNoSound.Level, levelFinishedScore);
        }
        SceneManager.LoadScene("LevelFinished");
    }
}

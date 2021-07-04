using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MakeNoSound
{
    public static readonly string SaveStatePath = Application.streamingAssetsPath + "/save.bin";
    public static readonly int LevelCount = 2;

    public static int Score = 0;

    private static int[] highscores;
    private static int unlockedLevels = -1;
    private static int activeLevel = -1;

    public static int UnlockedLevels
    {
        get { return unlockedLevels; }
    }

    public static int ActiveLevel
    {
        get { return activeLevel; }
    }

    public static bool HasNextLevel
    {
        get { return activeLevel < LevelCount; }
    }

    public static bool HasSaveState
    {
        get { return File.Exists(SaveStatePath); }
    }

    public static void LoadSave()
    {
        using FileStream stream = File.OpenRead(SaveStatePath);
        using BinaryReader reader = new BinaryReader(stream);
        unlockedLevels = reader.ReadInt32();
        highscores = new int[LevelCount];

        for (int i = 0; i < LevelCount; i++)
        {
            highscores[i] = reader.ReadInt32();
        }
    }

    public static void NewSave()
    {
        unlockedLevels = 0;
        highscores = new int[LevelCount];
        WriteSave();
        LoadLevel(0);
    }

    private static void WriteSave()
    {
        using FileStream stream = File.OpenWrite(SaveStatePath);
        using BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(unlockedLevels);

        for (int i = 0; i < LevelCount; i++)
        {
            writer.Write(highscores[i]);
        }
    }

    public static void LoadLevel(int level)
    {
        activeLevel = level;
        SceneManager.LoadScene("Level" + (level + 1).ToString());
    }

    public static int GetHighscore(int level)
    {
        if (level >= LevelCount) return 0;
        return highscores[level];
    }

    public static void FinishLevel()
    {
        if (highscores[activeLevel] < Score)
        {
            highscores[activeLevel] = Score;
        }
        activeLevel++;
        unlockedLevels = Mathf.Max(activeLevel, unlockedLevels);
        WriteSave();
    }
}

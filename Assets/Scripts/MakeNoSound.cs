using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameManager
{
    public static readonly string SaveStatePath = Application.streamingAssetsPath + "/save.bin";
    public static readonly int LevelCount = 5;

    private static int[] highscores = new int[LevelCount];
    private static int level = -1;

    public static int Level
    {
        get { return level; }
    }

    public static bool HasSaveState
    {
        get { return level != -1; }
    }

    public static void LoadSave()
    {
        using FileStream stream = File.OpenRead(SaveStatePath);
        using BinaryReader reader = new BinaryReader(stream);
        level = reader.ReadInt32();

        for (int i = 0; i < LevelCount; i++)
        {
            highscores[i] = reader.ReadInt32();
        }
    }

    public static void WriteSave()
    {
        using FileStream stream = File.OpenWrite(SaveStatePath);
        using BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(level);

        for (int i = 0; i < LevelCount; i++)
        {
            writer.Write(highscores[i]);
        }
    }

    public static void NewSave()
    {
        level = 0;
        highscores = new int[LevelCount];
        WriteSave();
    }

    public static void SetHighscore(int level, int highscore)
    {
        highscores[level] = highscore;
    }

    public static int GetHighscore(int level)
    {
        if (highscores.Length < level) return 0;
        return highscores[level];
    }

    public static void AdvanceLevel()
    {
        level++;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages and provides the game state
/// </summary>
public static class MakeNoSound
{
    /// <summary>Path to the save file</summary>
    public static readonly string SaveStatePath = Application.streamingAssetsPath + "/save.bin";
    /// <summary>Count of unlockable levels</summary>
    public static readonly int LevelCount = 5;

    /// <summary>Score of the last completed level</summary>
    public static int Score = 0;

    private static int[] highscores;
    private static int unlockedLevels = -1;
    private static int activeLevel = -1;

    /// <summary>Count of all currently unlocked levels</summary>
    public static int UnlockedLevels
    {
        get { return unlockedLevels; }
    }

    /// <summary>Index of the active level (Is -1 if no level is active).</summary>
    public static int ActiveLevel
    {
        get { return activeLevel; }
    }

    /// <summary>True if there is a next playable level</summary>
    public static bool HasNextLevel
    {
        get { return activeLevel < LevelCount; }
    }

    /// <summary>True if a save file exists that can be loaded</summary>
    public static bool HasSaveState
    {
        get { return File.Exists(SaveStatePath); }
    }

    /// <summary>
    /// Loades the progression data of a save file into this game instance
    /// </summary>
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

    /// <summary>
    /// Resets the progression and overrides the save file
    /// </summary>
    public static void NewSave()
    {
        unlockedLevels = 0;
        highscores = new int[LevelCount];
        WriteSave();
        LoadLevel(0);
    }

    /// <summary>
    /// Encodes and stores the progression data in a save file
    /// </summary>
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

    /// <summary>
    /// Loades a level scene by the level index
    /// </summary>
    /// <param name="level">Level index</param>
    public static void LoadLevel(int level)
    {
        activeLevel = level;
        SceneManager.LoadScene("Level" + (level + 1).ToString());
    }

    /// <summary>
    /// Returns the highscore of a level by its index
    /// </summary>
    /// <param name="level">Level index</param>
    /// <returns>Highscore</returns>
    public static int GetHighscore(int level)
    {
        if (level >= LevelCount) return 0;
        return highscores[level];
    }

    /// <summary>
    /// Marks a level as finished and potentially overrides the highscore
    /// </summary>
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

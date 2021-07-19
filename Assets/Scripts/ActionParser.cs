using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an action in the level
/// </summary>
public enum Move { Up, Down, Left, Right, Wait, Lure, Activate };

/// <summary>
/// Parses a string of commands to actions
/// </summary>
public class ActionParser
{
    private readonly string[] MULTIPLIER = new string[]
    {
        "eins", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun"
    };

    private bool FindMultiplier(string raw, ref int multiplier)
    {
        for (int i = 0; i < MULTIPLIER.Length; i++)
        {
            if (MULTIPLIER[i].Equals(raw))
            {
                multiplier = 1 + i;
                return true;
            }
        }
        return false;
    }

    private Move FindMove(string move)
    {
        if (move.Equals("hoch")) return Move.Up;
        else if (move.Equals("runter")) return Move.Down;
        else if (move.Equals("links")) return Move.Left;
        else if (move.Equals("rechts")) return Move.Right;
        else if (move.Equals("warten")) return Move.Wait;
        else if (move.Equals("anlocken")) return Move.Lure;
        else if (move.Equals("aktivieren")) return Move.Activate;

        throw new ArgumentException("illegal action");
    }

    /// <summary>
    /// Translates a string of commands to the corresponding list of moves
    /// </summary>
    /// <param name="command">String of commands</param>
    /// <returns>List of Moves</returns>
    public List<Move> Parse(string command)
    {
        try
        {
            List<Move> moves = new List<Move>();
            string[] subcommands = command.Split(' ');
            int multiplier = 1;

            foreach (string subcommand in subcommands)
            {
                if (!FindMultiplier(subcommand, ref multiplier))
                {
                    Move move = FindMove(subcommand);
                    for (int i = 0; i < multiplier; i++) moves.Add(move);
                    multiplier = 1;
                }
            }
            return moves;
        }
        catch
        {
            return null;
        }
    }
}

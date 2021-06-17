using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Move { Up, Down, Left, Right, Wait };

public class ActionParser
{
    private readonly string[] MULTIPLIER = new string[]
    {
        "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun"
    };

    private bool FindMultiplier(string raw, ref int multiplier)
    {
        for (int i = 0; i < MULTIPLIER.Length; i++)
        {
            if (MULTIPLIER[i].Equals(raw))
            {
                multiplier = 2 + i;
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

        throw new ArgumentException("illegal action");
    }

    public List<Move> Parse(string command)
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
}

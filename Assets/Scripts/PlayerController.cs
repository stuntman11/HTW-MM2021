using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Awake()
    {
        LevelController level = GameObject.Find("Controller").GetComponent<LevelController>();
        level.OnTick += OnTick;
    }

    private void OnTick(string command)
    {
        Debug.Log("Player ticked!");
    }
}

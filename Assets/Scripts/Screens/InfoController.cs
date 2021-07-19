using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the info popup that displays all controls
/// </summary>
public class InfoController : MonoBehaviour
{
    /// <summary>A reference to the scenes InfoScreen object</summary>
    public GameObject InfoScreen;

    private Button infoBtn;
    private GameObject infoPopup;

    void Awake()
    {
        infoPopup = InfoScreen.transform.Find("InfoPopup").gameObject;
        infoPopup.SetActive(false);

        infoBtn = InfoScreen.transform.Find("InfoBtn").GetComponent<Button>();
        infoBtn.onClick.AddListener(OnInfoToggle);

        CommandController command = GetComponent<CommandController>();
        command.OnCommand += OnCommand;
    }

    private void OnCommand(string command)
    {
        if (command.Equals("pause"))
        {
            infoPopup.SetActive(false);
        }
    }

    private void OnInfoToggle()
    {
        infoPopup.SetActive(!infoPopup.activeSelf);
    }
}

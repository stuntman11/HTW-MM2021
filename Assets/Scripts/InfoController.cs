using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    public Button InfoBtn;
    public GameObject InfoList;

    void Awake()
    {
        InfoBtn.onClick.AddListener(OnInfoToggle);
        InfoList.SetActive(false);
    }

    private void OnInfoToggle()
    {
        InfoList.SetActive(!InfoList.activeSelf);
    }
}

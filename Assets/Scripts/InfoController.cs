using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    private GameObject infoList;

    void Awake()
    {
        Button infoBtn = GameObject.Find("InfoBtn").GetComponent<Button>();
        infoBtn.onClick.AddListener(OnInfoToggle);

        infoList = GameObject.Find("InfoList");
        infoList.SetActive(false);
    }

    private void OnInfoToggle()
    {
        infoList.SetActive(!infoList.activeSelf);
    }
}

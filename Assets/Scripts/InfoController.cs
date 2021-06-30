using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    public GameObject InfoScreen;

    private Button infoBtn;
    private GameObject infoPopup;

    void Awake()
    {
        infoPopup = InfoScreen.transform.Find("InfoPopup").gameObject;
        infoPopup.SetActive(false);

        infoBtn = InfoScreen.transform.Find("InfoBtn").GetComponent<Button>();
        infoBtn.onClick.AddListener(OnInfoToggle);
    }

    private void OnInfoToggle()
    {
        infoPopup.SetActive(!infoPopup.activeSelf);
    }
}

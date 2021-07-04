using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{
    public RectTransform LevelButton;
    private void Awake()
    {
        GameObject btnContainer = GameObject.Find("BtnContainer");
        
        for(int i = 0; i <= MakeNoSound.Level; i++)
        {
            Vector3 position = new Vector3(0, i*100, -1);
            RectTransform currentLevelButton = Instantiate(LevelButton, btnContainer.transform);

            currentLevelButton.anchoredPosition = position;
            currentLevelButton.anchorMin = new Vector2(0.5f, 0.5f);
            currentLevelButton.anchorMax = new Vector2(0.5f, 0.5f);

            currentLevelButton.name = "Level " + (i+1).ToString();
        }
    }
}

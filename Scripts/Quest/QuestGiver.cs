using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;

    private void Start()
    {
        questWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OpenCloseQuestWindow();
        }
    }

    public void OpenCloseQuestWindow()
    {
        questWindow.SetActive(!questWindow.activeSelf);

        if (questWindow.activeSelf)
        {
            titleText.text = quest.title;
            descriptionText.text = quest.description;
        }
    }
}

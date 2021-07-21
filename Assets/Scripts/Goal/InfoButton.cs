using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoButton : BehaviourButton
{
    [TextArea]
    public string[] languages = new string[(int)AppManager.Languages.ENUM_END];
    private TaskManager taskManager = null;

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

  

    protected override void OnTouch()
    {
            switch (AppManager.currentLanguage)
            {
                case AppManager.Languages.English:
                taskManager.ShowInfoPanel(languages[0]);
                    break;
                case AppManager.Languages.Magyar:
                taskManager.ShowInfoPanel(languages[1]);
                break;
                case AppManager.Languages.Deutsch:
                taskManager.ShowInfoPanel(languages[2]);
                break;
                default:
                taskManager.ShowInfoPanel("");
                break;
            }
    }
}

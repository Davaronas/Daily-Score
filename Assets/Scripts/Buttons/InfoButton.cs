using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoButton : BehaviourButton
{
    [TextArea]
    public string[] languages = new string[(int)AppManager.Languages.ENUM_END];

    public string[] headers = new string[(int)AppManager.Languages.ENUM_END];
    private TaskManager taskManager = null;

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

  

    protected override void OnTouch()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

            switch (AppManager.currentLanguage)
            {
                case AppManager.Languages.English:
                taskManager.ShowInfoPanel(headers[0],languages[0]);
                    break;
                case AppManager.Languages.Magyar:
                taskManager.ShowInfoPanel(headers[1],languages[1]);
                break;
                case AppManager.Languages.Deutsch:
                taskManager.ShowInfoPanel(headers[2],languages[2]);
                break;
                default:
                taskManager.ShowInfoPanel("","");
                break;
            }
    }
}

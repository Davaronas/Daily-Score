using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SettingsNotificationHandler : MonoBehaviour
{


    [SerializeField] private GameObject notificationPrefab = null;
    private List<SettingsNotificationPrefabUtility> notifications = new List<SettingsNotificationPrefabUtility>();
    private NotificationManager notificationManager = null;

    private GoalManager goalManager = null;
    private TaskManager taskManager = null;


    private void Awake()
    {
        notificationManager = FindObjectOfType<NotificationManager>();
        goalManager = FindObjectOfType<GoalManager>();
        taskManager = FindObjectOfType<TaskManager>();

        AppManager.OnNewTaskAdded += UpdateTextCallback;
        AppManager.OnTaskEdited += UpdateTextCallback;
    }

    private void Start()
    {
       Invoke(nameof(UpdateTextCallback),Time.deltaTime*2);
    }

    private void OnDestroy()
    {
        AppManager.OnNewTaskAdded -= UpdateTextCallback;
        AppManager.OnTaskEdited -= UpdateTextCallback;
    }

    private void UpdateTextCallback(AppManager.Languages _l)
    {
        UpdateTextCallback();
    }

    private void UpdateTextCallback()
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            Destroy(notifications[i].gameObject);
        }

        notifications.Clear();


        NotificationManager.NotificationData[] _datas = notificationManager.GetNotifcationDatas();

        for (int i = 0; i < _datas.Length; i++)
        {
            SettingsNotificationPrefabUtility _snpu = Instantiate(notificationPrefab, transform).GetComponent<SettingsNotificationPrefabUtility>();

            DateTime _date = Convert.ToDateTime(_datas[i].fireTime);
          


            if (_datas[i].resetIntervalDays == 7)
            {
                _snpu.SetData(_datas[i].ownerTask, _date.DayOfWeek, _date.Hour, _date.Minute, goalManager, taskManager);
            }
            else
            {
                _snpu.SetData(_datas[i].ownerTask, _datas[i].resetIntervalDays, _date.Hour, _date.Minute, goalManager,taskManager);
            }

            notifications.Add(_snpu);
        }
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationHolder : MonoBehaviour
{

    [SerializeField] private GameObject notificationPrefab;
    private List<NotificationPrefabUtility> notifications = new List<NotificationPrefabUtility>();
    private List<DayOfWeek> daysSelected = new List<DayOfWeek>();
    
  

    public void CreateNewNotification(DayOfWeek _day)
    {
      NotificationPrefabUtility  _npu =  Instantiate(notificationPrefab, transform.position, Quaternion.identity, transform).GetComponent<NotificationPrefabUtility>();
        _npu.SetSelectedDay(_day);
        daysSelected.Add(_day);
    }

    public void CreateNewNotification()
    {
        NotificationPrefabUtility _npu = Instantiate(notificationPrefab, transform.position, Quaternion.identity, transform).GetComponent<NotificationPrefabUtility>();
        _npu.SetSelectedDay(DateTime.Today.DayOfWeek);
        notifications.Add(_npu);
        daysSelected.Add(DateTime.Today.DayOfWeek);
    }

    public void DeleteNotification(DayOfWeek _day)
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            if(notifications[i].daySelected == _day)
            Destroy(notifications[i].gameObject);
            notifications.Remove(notifications[i]);
            break;
        }
    }




    public void Clear()
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            Destroy(notifications[i].gameObject);
        }

        notifications.Clear();
        daysSelected.Clear();
    }

    public List<NotificationPrefabUtility> GetNotifications()
    {
        return notifications;
    }

    public bool HasDay(DayOfWeek _day)
    {
        for (int i = 0; i < daysSelected.Count; i++)
        {
            if(daysSelected[i] == _day)
            {
                return true;
            }
        }

        return false;
    }
}

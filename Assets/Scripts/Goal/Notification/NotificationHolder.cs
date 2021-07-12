using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationHolder : MonoBehaviour
{

    [SerializeField] private GameObject notificationPrefab;
    private List<NotificationPrefabUtility> notifications = new List<NotificationPrefabUtility>();
    private List<DayOfWeek> daysSelected = new List<DayOfWeek>();
    [SerializeField] private RectTransform createTaskScrollContent;

    private VerticalLayoutGroup layoutGroup;

    private float notificationPrefab_y = 0;
    private RectTransform parentRT;
    private Vector2 parentOriginalPos;

    // interval holder will reset the size of create task scroll on  disable, no need to do it here


    private void Awake()
    {
        layoutGroup = GetComponent<VerticalLayoutGroup>();
        notificationPrefab_y = (notificationPrefab.GetComponent<RectTransform>().sizeDelta.y + (layoutGroup.spacing * 2));
        parentRT = transform.parent.GetComponent<RectTransform>();
        parentOriginalPos = parentRT.position;
    }

    public NotificationPrefabUtility CreateNewNotification(DayOfWeek _day)
    {
      NotificationPrefabUtility  _npu =  Instantiate(notificationPrefab, transform.position, Quaternion.identity, transform).GetComponent<NotificationPrefabUtility>();
        _npu.SetSelectedDay(_day);
        notifications.Add(_npu);
        daysSelected.Add(_day);

        ScrollSizer.AddSize(createTaskScrollContent, notificationPrefab_y);
        parentRT.anchoredPosition += new Vector2(0, notificationPrefab_y);
        return _npu;
    }

    public NotificationPrefabUtility CreateNewNotification(int _reset)
    {
        NotificationPrefabUtility _npu = Instantiate(notificationPrefab, transform.position, Quaternion.identity, transform).GetComponent<NotificationPrefabUtility>();
        _npu.SetSelectedDay(_reset);
        notifications.Add(_npu);
        daysSelected.Add(DateTime.Today.DayOfWeek);

        ScrollSizer.AddSize(createTaskScrollContent, notificationPrefab_y);
        parentRT.anchoredPosition += new Vector2(0, notificationPrefab_y);
        return _npu;
    }

    public void DeleteNotification(DayOfWeek _day)
    {
        print("Delete one");

        daysSelected.Remove(_day);

        GameObject _toDestroy = null;
        for (int i = 0; i < notifications.Count; i++)
        {
            if (notifications[i].daySelected == _day)
            {
                _toDestroy = notifications[i].gameObject;
                notifications.Remove(notifications[i]);
                break;
            }
        }

        if (_toDestroy != null)
        {
            Destroy(_toDestroy);
            ScrollSizer.ReduceSize(createTaskScrollContent, notificationPrefab_y);
            parentRT.anchoredPosition -= new Vector2(0, notificationPrefab_y);
        }

       
    }




    public void Clear()
    {

        for (int i = 0; i < notifications.Count; i++)
        {
            Destroy(notifications[i].gameObject);
            ScrollSizer.ReduceSize(createTaskScrollContent, notificationPrefab_y);
            parentRT.anchoredPosition -= new Vector2(0, notificationPrefab_y);
        }

        notifications.Clear();
        daysSelected.Clear();
    }

    public List<NotificationPrefabUtility> GetNotifications()
    {
        return notifications;
    }

    public int GetNotificationAmount()
    {
        return notifications.Count;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif


#if UNITY_IOS
public class NotificationManagerIOS : MonoBehaviour

{



    private GoalManager goalManager = null;
    private TaskManager taskManager = null;

    [System.Serializable]
    public struct NotificationDataIOS
    {
        public string identifier;
        public string title;
        public string text;
        public string fireTime;
        public int resetIntervalDays;
        public string ownerTask;

        public override bool Equals(object obj)
        {
            return obj is NotificationDataIOS data &&
                   identifier == data.identifier &&
                   title == data.title &&
                   text == data.text &&
                   fireTime == data.fireTime &&
                   resetIntervalDays == data.resetIntervalDays &&
                   ownerTask == data.ownerTask;
        }

        public override int GetHashCode()
        {
            int hashCode = -1631183905;
            hashCode = hashCode * -1521134295 + identifier.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(identifier);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(text);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(fireTime);
            hashCode = hashCode * -1521134295 + resetIntervalDays.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ownerTask);
            return hashCode;
        }

        public static bool operator ==(NotificationDataIOS _data1, NotificationDataIOS _data2)
        {
            return (_data1.ownerTask == _data2.ownerTask &&
               Convert.ToDateTime(_data1.fireTime).DayOfWeek == Convert.ToDateTime(_data2.fireTime).DayOfWeek);


        }

        public static bool operator !=(NotificationDataIOS _data1, NotificationDataIOS _data2)
        {
            return (_data1.ownerTask != _data2.ownerTask ||
               Convert.ToDateTime(_data1.fireTime).DayOfWeek != Convert.ToDateTime(_data2.fireTime).DayOfWeek);
        }


        public static explicit operator NotificationManager.NotificationData(NotificationDataIOS _nd_ios)
        {
            NotificationManager.NotificationData _nd = new NotificationManager.NotificationData();
            _nd.id = -1;
            _nd.title = _nd_ios.title;
            _nd.text = _nd_ios.text;
            _nd.ownerTask = _nd_ios.ownerTask;
            _nd.resetIntervalDays = _nd_ios.resetIntervalDays;
            _nd.fireTime = _nd_ios.fireTime;
            _nd.spriteId = -1;

            return _nd;
        }
    }

    public static List<NotificationDataIOS> notifications = new List<NotificationDataIOS>();


    void Start()
    {
#if UNITY_IOS
        string path = Path.Combine(Application.persistentDataPath, "notificationdata_ios");
        if (File.Exists(path))
        {
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.IsReadOnly = false;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            notifications = formatter.Deserialize(stream) as List<NotificationDataIOS>;
            stream.Close();
            fileInfo.IsReadOnly = true;
        }


        iOSNotificationCenter.RemoveAllDeliveredNotifications();
        iOSNotificationCenter.RemoveAllScheduledNotifications();

        for (int i = 0; i < notifications.Count; i++)
        {
            SendNotification(notifications[i].title, notifications[i].text, Convert.ToDateTime(notifications[i].fireTime), notifications[i].resetIntervalDays,notifications[i].ownerTask, "");
        }

        Invoke(nameof(CheckNotValidNotifications), 1f);
#endif
    }

    private void CheckNotValidNotifications()
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            if (!goalManager.SearchTaskByName(notifications[i].ownerTask))
            {
                ClearNotification(notifications[i]);
            }
        }
    }


    public static void ClearNotification(NotificationDataIOS _nd_ios)
    {
        iOSNotificationCenter.RemoveScheduledNotification(_nd_ios.identifier);
        iOSNotificationCenter.RemoveDeliveredNotification(_nd_ios.identifier);

        notifications.Remove(_nd_ios);
    }



    public static void SendNotification(string _title, string _text, DateTime _fireTime, int _resetDays, string _owner, string _spriteId)
    {

        if (_fireTime < DateTime.Now)
        {
            _fireTime = _fireTime.AddDays(_resetDays);
        }

        TimeSpan _distance = _fireTime - DateTime.Now;


        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = _distance,
            Repeats = false
        };


        var notification = new iOSNotification()
        {
            Title = _title,
            Subtitle = "A Daily Score notification",
            Body = _text,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };


        NotificationDataIOS _newData = new NotificationDataIOS();
        _newData.identifier = notification.Identifier;
        _newData.fireTime = _fireTime.ToString();
        _newData.title = _title;
        _newData.text = _text;
        _newData.resetIntervalDays = _resetDays;
        _newData.ownerTask = _owner;

        if (!notifications.Contains(_newData))
        {
            notifications.Add(_newData);
        }


        iOSNotificationCenter.ScheduleNotification(notification);

        SaveNotificationData();
    }

    private static void SaveNotificationData()
    {
        string path = Path.Combine(Application.persistentDataPath, "notificationdata_ios");
        if (File.Exists(path))
        {
            FileInfo fileInfoIfAlreadyExists = new FileInfo(path);
            fileInfoIfAlreadyExists.IsReadOnly = false;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, notifications);
        stream.Close();
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.IsReadOnly = true;
    }


    public static bool GetNotificationData(DayOfWeek _day, string _task, out NotificationDataIOS _data)
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            if (Convert.ToDateTime(notifications[i].fireTime).DayOfWeek == _day && notifications[i].ownerTask == _task)
            {
                _data = notifications[i];
                return true;
            }
        }
        _data = new NotificationDataIOS();
        return false;
    }

    public static bool GetNotificationData_NoDayCheck(string _owner, out NotificationDataIOS _data)
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            if (notifications[i].ownerTask == _owner)
            {
                _data = notifications[i];
                return true;
            }
        }

        _data = new NotificationDataIOS();
        return false;
    }


    /*
    public static void SendNotificationWithAddingResetTime(NotificationDataIOS _data)
    {

        _data.fireTime = Convert.ToDateTime(_data.fireTime).AddDays(_data.resetIntervalDays).ToString();

        if (Convert.ToDateTime(_data.fireTime) < DateTime.Now)
        {
            _data.fireTime = Convert.ToDateTime(_data.fireTime).AddDays(_data.resetIntervalDays).ToString();
        }

        TimeSpan _distance = Convert.ToDateTime(_data.fireTime) - DateTime.Now;

        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = _distance,
            Repeats = false
        };


        var notification = new iOSNotification()
        {
            Identifier = _data.identifier,
            Title = _data.title,
            Subtitle = "A Daily Score notification",
            Body = _data.text,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        NotificationDataIOS _newData = new NotificationDataIOS();
        _newData.identifier = _data.identifier;
        _newData.fireTime = _data.fireTime;
        _newData.title = _data.title;
        _newData.text = _data.text;
        _newData.resetIntervalDays = _data.resetIntervalDays;
        _newData.ownerTask = _data.ownerTask;


        SaveNotificationData();
    }
    */
}
#endif

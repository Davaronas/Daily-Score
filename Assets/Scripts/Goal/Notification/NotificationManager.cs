using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class NotificationManager : MonoBehaviour
{

   private GoalManager goalManager = null;
    private TaskManager taskManager = null;
    #if UNITY_IOS
    private NotificationManagerIOS nmIOS = null;
#endif

    [System.Serializable]
    public struct NotificationData
    {
        public int id;
        public string title;
        public string text;
        public string fireTime;
        public int resetIntervalDays;
        public string ownerTask;
        public int spriteId;

        public override bool Equals(object obj)
        {
            return obj is NotificationData data &&
                   id == data.id &&
                   title == data.title &&
                   text == data.text &&
                   fireTime == data.fireTime &&
                   resetIntervalDays == data.resetIntervalDays &&
                   ownerTask == data.ownerTask;
        }

        public override int GetHashCode()
        {
            int hashCode = 1679236910;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(text);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(fireTime);
            hashCode = hashCode * -1521134295 + resetIntervalDays.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ownerTask);
            return hashCode;
        }

        public static bool operator ==(NotificationData _data1,NotificationData _data2)
        {
            return (_data1.ownerTask == _data2.ownerTask &&
               Convert.ToDateTime(_data1.fireTime).DayOfWeek == Convert.ToDateTime(_data2.fireTime).DayOfWeek);
           

        }

        public static bool operator !=(NotificationData _data1, NotificationData _data2)
        {
            return (_data1.ownerTask != _data2.ownerTask ||
               Convert.ToDateTime(_data1.fireTime).DayOfWeek != Convert.ToDateTime(_data2.fireTime).DayOfWeek);
        }
    }

    public static List<NotificationData> notifications = new List<NotificationData>();


    //public static event AndroidNotificationCenter.NotificationReceivedCallback OnNotificationReceived;

    //   public static Dictionary<int, NotificationData> notificationIds = new Dictionary<int, NotificationData>();

    // public delegate void NotificationReceivedCallback(AndroidNotificationIntentData data);


    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        taskManager = FindObjectOfType<TaskManager>();
        
    }

    private void Start()
    {


#if UNITY_ANDROID
        string path = Path.Combine(Application.persistentDataPath, "notificationdata");
        if (File.Exists(path))
        {
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.IsReadOnly = false;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            notifications = formatter.Deserialize(stream) as List<NotificationData>;
            stream.Close();
            fileInfo.IsReadOnly = true;

            for (int i = 0; i < notifications.Count; i++)
            {

              

                NotificationStatus _status = AndroidNotificationCenter.CheckScheduledNotificationStatus(notifications[i].id);

                if (_status == NotificationStatus.Delivered)
                {
                  //  if(notifications[i].ownerTask) // find if owner exists
                    NotificationData _replace = notifications[i];
                    ClearNotification(notifications[i].id);
                    SendNotificationWithAddingResetTime(_replace);
                }



            }
        }

        CreateChannel();
        Invoke(nameof(CheckNotValidNotifications), 1f);
#endif



#if UNITY_IOS
         
#endif
    }


    private void CheckNotValidNotifications()
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            if (!goalManager.SearchTaskByName(notifications[i].ownerTask))
            {
                ClearNotification(notifications[i].id);
            }
        }
    }



    public static bool GetNotificationData(int _id, out NotificationData _data)
    {

        for (int i = 0; i < notifications.Count; i++)
        {
            if (notifications[i].id == _id)
            {
                _data = notifications[i];
                return true;
            }
        }
        _data = new NotificationData();
        return false;
    }

    public static bool GetNotificationData(DayOfWeek _day,string _task, out NotificationData _data)
    {
#if UNITY_ANDROID
        for (int i = 0; i < notifications.Count; i++)
        {
            if (Convert.ToDateTime(notifications[i].fireTime).DayOfWeek == _day && notifications[i].ownerTask == _task)
            {
                _data = notifications[i];
                return true;
            }
        }

      

        _data = new NotificationData();
        return false;
#endif




#if UNITY_IOS
  if (NotificationManagerIOS.GetNotificationData(_day, _task, out NotificationManagerIOS.NotificationDataIOS _dataIOS))
        {
            _data = (NotificationData)_dataIOS;
            return true;
        }
        else
        {
            _data = new NotificationData();
            return false;
        }
#endif
    }




    public static bool GetNotificationData(string _owner, DayOfWeek _day, out NotificationData _data)
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            if(notifications[i].ownerTask == _owner && Convert.ToDateTime(notifications[i].fireTime).DayOfWeek == _day)
            {
                _data = notifications[i];
                return true;
            }
        }

        _data = new NotificationData();
        return false;
    }

    public static bool GetNotificationData_NoDayCheck(string _owner, out NotificationData _data)
    {

#if UNITY_ANDROID
        for (int i = 0; i < notifications.Count; i++)
        {
            if (notifications[i].ownerTask == _owner)
            {
                _data = notifications[i];
                return true;
            }
        }

        _data = new NotificationData();
        return false;
#endif



#if UNITY_IOS
   if (NotificationManagerIOS.GetNotificationData_NoDayCheck(_owner, out NotificationManagerIOS.NotificationDataIOS _dataIOS))
        {
            _data = (NotificationData)_dataIOS;
            return true;
        }
        else
        {
            _data = new NotificationData();
            return false;
        }
#endif
    }


    public static void CreateChannel()
    {
        AndroidNotificationChannel channel = AndroidNotificationCenter.GetNotificationChannel("dailyscore_id");

        if (channel.Id != "dailyscore_id")
        {
            channel = new AndroidNotificationChannel(
                "dailyscore_id",
                "Daily Score",
                "A channel made for the Daily Score task notifications",
                Importance.High);

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }
    }

    public static void SendNotification(string _title, string _text, DateTime _fireTime, int _resetDays, string _owner, string _spriteId)
    {
        
#if UNITY_ANDROID
        if (_fireTime < DateTime.Now )
        {
            _fireTime = _fireTime.AddDays(_resetDays);
        }

        AndroidNotification notification = new AndroidNotification();
        notification.Title = _title;
        notification.Text = _text;
        notification.FireTime = _fireTime;
        notification.SmallIcon = "icon_" + _spriteId;
        notification.LargeIcon = "iconl_" + _spriteId;
        

        NotificationData notificationData = new NotificationData();
        notificationData.title = notification.Title;
        notificationData.text = notification.Text;
        notificationData.fireTime = notification.FireTime.ToString();
        notificationData.resetIntervalDays = _resetDays;
        notificationData.ownerTask = _owner;
        notificationData.spriteId =int.Parse(_spriteId);

        notificationData.id = AndroidNotificationCenter.SendNotification(notification, "dailyscore_id");

        notifications.Add(notificationData);



        SaveNotificationData();
#endif



#if UNITY_IOS
        
        NotificationManagerIOS.SendNotification(_title, _text, _fireTime, _resetDays, _owner, _spriteId);

#endif

    }

    public static void SendNotificationWithAddingResetTime(NotificationData _data)
    {
#if UNITY_ANDROID
        AndroidNotification notification = new AndroidNotification();
        notification.Title = _data.title;
        notification.Text = _data.text;
        notification.FireTime = Convert.ToDateTime(_data.fireTime).AddDays(_data.resetIntervalDays);
        notification.SmallIcon = "icon_" + _data.spriteId;
        notification.LargeIcon = "iconl_" + _data.spriteId;



        _data.id = AndroidNotificationCenter.SendNotification(notification, "dailyscore_id");
        _data.fireTime = notification.FireTime.ToString();




        SaveNotificationData();
#endif



#if UNITY_IOS
         
#endif

    }

    public static void ClearAllNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }



    public static void ClearNotification(int _id)
    {

        for (int i = 0; i < notifications.Count; i++)
        {
            if(notifications[i].id == _id)
            {
                AndroidNotificationCenter.CancelNotification(_id);
                notifications.Remove(notifications[i]);
                SaveNotificationData();
                break;
            }
        }

        
        
            Debug.LogError($"Id does not exist in the notification center: {_id}");
        
    }

    public static void DeleteNotification(NotificationData _data)
    {

        for (int i = 0; i < notifications.Count; i++)
        {
            if(notifications[i] == _data)
            {
                AndroidNotificationCenter.CancelNotification(notifications[i].id);
                notifications.Remove(notifications[i]);
                break;
            }
        }

        SaveNotificationData();
    }

    public static void DeleteNotificationAttachedToTask(string _taskName)
    {
        List<NotificationData> _notificationsToDelete = new List<NotificationData>();

        for (int i = 0; i < notifications.Count; i++)
        {

            if (notifications[i].ownerTask == _taskName)
            {
                AndroidNotificationCenter.CancelNotification(notifications[i].id);
                _notificationsToDelete.Add(notifications[i]);
            }
        }

        foreach(NotificationData j in _notificationsToDelete)
        {
            notifications.Remove(j);
        }


        SaveNotificationData();
    }
        



    private static void SaveNotificationData()
    {
        string path = Path.Combine(Application.persistentDataPath, "notificationdata");
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






    private void OnApplicationQuit()
    {
        if (notifications == null) { return; }

        for (int i = 0; i < notifications.Count; i++)
        {

            NotificationStatus _status = AndroidNotificationCenter.CheckScheduledNotificationStatus(notifications[i].id);
            if (_status == NotificationStatus.Delivered)
            {
                NotificationData _replace = notifications[i];
                ClearNotification(notifications[i].id);
                SendNotificationWithAddingResetTime(_replace);
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            if(notifications == null) { return; }

            for (int i = 0; i < notifications.Count; i++)
            {

                NotificationStatus _status = AndroidNotificationCenter.CheckScheduledNotificationStatus(notifications[i].id);
                if (_status == NotificationStatus.Delivered)
                {
                    NotificationData _replace = notifications[i];
                    ClearNotification(notifications[i].id);
                    SendNotificationWithAddingResetTime(_replace);
                }
            }
        }
    }

    public NotificationData[] GetNotifcationDatas()
    {
        return notifications.ToArray();
    }
}

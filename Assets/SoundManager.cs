using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static AudioSource ac;

    
    [SerializeField] private  AudioClip click1;
    [SerializeField] private  AudioClip click2;
    [SerializeField] private  AudioClip achievement;
    [SerializeField] private  AudioClip goal_task_added;
    [SerializeField] private  AudioClip message;
    [SerializeField] private  AudioClip delete;
    

     private static AudioClip achievement_s;
    private static AudioClip click1_s;
     private static AudioClip click2_s;
     private static AudioClip goal_task_added_s;
     private static AudioClip message_s;
    private static AudioClip delete_s;


    private void Awake()
    {
        ac = GetComponent<AudioSource>();

        click1_s = click1;
        click2_s = click2;
        achievement_s = achievement;
        goal_task_added_s = goal_task_added;
        message_s = message;
        delete_s = delete;
    }

    public static void PlaySound1()
    {
        
        ac.PlayOneShot(achievement_s);
    }

    public static void PlaySound2()
    {
        ac.PlayOneShot(click1_s,0.4f);
    }

    public static void PlaySound3()
    {
        ac.PlayOneShot(click2_s);
    }

    public static void PlaySound4()
    {
        ac.PlayOneShot(goal_task_added_s);
    }

    public static void PlaySound5()
    {
        ac.PlayOneShot(message_s);
    }

    public static void PlaySound6()
    {
        ac.PlayOneShot(delete_s);
    }
}

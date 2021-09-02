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

    private static bool allow = true;
    private static MonoBehaviour mb = null;

    [SerializeField] private GameObject onButtonLine;
    [SerializeField] private GameObject offButtonLine;

    private void Awake()
    {
        ac = GetComponent<AudioSource>();
        mb = GetComponent<MonoBehaviour>();

        click1_s = click1;
        click2_s = click2;
        achievement_s = achievement;
        goal_task_added_s = goal_task_added;
        message_s = message;
        delete_s = delete;

        switch(PlayerPrefs.GetInt("Audio",1))
        {
            case 0:
                ac.enabled = false;
                onButtonLine.SetActive(false);
                offButtonLine.SetActive(true);
                break;
            case 1:
                ac.enabled = true;
                onButtonLine.SetActive(true);
                offButtonLine.SetActive(false);
                break;
        }
    }

    public static void PlaySound1()
    {
        if (ac.enabled)
        {
            ac.clip = achievement_s;
            ac.Play();
          //  ac.PlayOneShot(achievement_s);
        }
    }

    public static void PlaySound2()
    {
        if (ac.enabled)
        {
            if (allow)
            {
               // ac.PlayOneShot(click1_s);

                ac.clip = click1_s;
                ac.Play();

                allow = false;
                mb.Invoke(nameof(AllowClickSoundAgain), Time.deltaTime);
            }
        }
       
    }

    public static void PlaySound3()
    {
        if (ac.enabled)
        {
           // ac.PlayOneShot(click2_s);

            ac.clip = click2_s;
            ac.Play();
        }
    }

    public static void PlaySound4()
    {
        if (ac.enabled)
        {
           // ac.PlayOneShot(goal_task_added_s);

            ac.clip = goal_task_added_s;
            ac.Play();
        }
    }

    public static void PlaySound5()
    {
        if (ac.enabled)
        {
            // ac.PlayOneShot(message_s);

            ac.clip = message_s;
            ac.Play();
        }
    }

    public static void PlaySound6()
    {
        if (ac.enabled)
        {
           // ac.PlayOneShot(delete_s);

            ac.clip = delete_s;
            ac.Play();
        }
    }


    private void AllowClickSoundAgain()
    {
        allow = true;
    }

    public void RemoteCall_TurnOffAudio()
    {
        ac.enabled = false;
        PlayerPrefs.SetInt("Audio", 0);
        onButtonLine.SetActive(false);
        offButtonLine.SetActive(true);
    }

    public void RemoteCall_TurnOnAudio()
    {
        ac.enabled = true;
        PlayerPrefs.SetInt("Audio", 1);
        onButtonLine.SetActive(true);
        offButtonLine.SetActive(false);

        SoundManager.PlaySound2();
    }
}

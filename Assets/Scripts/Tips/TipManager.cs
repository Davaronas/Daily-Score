using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TipManager : MonoBehaviour
{
    [SerializeField] private Transform savedTipsLayoutGroup;
    [SerializeField] private GameObject tipPrefab;
    [SerializeField] private TMP_Text savedTipsAmountText;

    [SerializeField] private TMP_Text dailyTipHeader;
    [SerializeField] private TMP_Text dailyTipContent;
    [SerializeField] private RectTransform tipSubmenuScrollContent;


    [SerializeField] private DisplayedTip mainTip;
    [SerializeField] private DisplayedTip secondTip;
    [SerializeField] private GameObject secondTipOverlay;

    [SerializeField] private GameObject savedTipPanel;
    [SerializeField] private DisplayedTip savedTip;

    [SerializeField] private GameObject askToDeleteTipPanel;
    [SerializeField] private TMP_Text askToDeleteTipText;


  
    private int secondTipId = -1;

    private List<SavedTip> savedTips = new List<SavedTip>();
    private float tipPrefab_Y_Size = 0;
    private TipHandler tipHandler;

    private int allowedCount;
    private int deleteTipId = -1;

   [HideInInspector] public int secondTipUnlockedToday = 0;

    private void Awake()
    {
        tipPrefab_Y_Size = tipPrefab.GetComponent<RectTransform>().rect.height;
        tipHandler = FindObjectOfType<TipHandler>();

        if(AppManager.isGold)
        {
            allowedCount = AppManager.SAVEDTIPAMOUNT_GOLD;
            UnlockSecondTip();
        }
        else
        {
            allowedCount = AppManager.SAVEDTIPAMOUNT_FREE;
        }
        ChangeSavedTipAmountText();


        savedTipPanel.SetActive(false);
        askToDeleteTipPanel.SetActive(false);
       

      
    }

  

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.SetInt("SecondTipUnlocked", 0);
        }
    }

    private void Start()
    {

        if (AppManager.lastLogin.Date != DateTime.Now.Date)
        {
            secondTipUnlockedToday = 0;
            PlayerPrefs.SetInt("SecondTipUnlocked", 0);
        }
         else
           {
               secondTipUnlockedToday = PlayerPrefs.GetInt("SecondTipUnlocked", 0);
           }
       


        if (secondTipUnlockedToday == 1 && !AppManager.isGold)
        {
            UnlockSecondTip();
        }
    }

    public void UnlockSecondTip()
    {
        string _h;
        string _c;
        int _id = tipHandler.AskForSecondTip(out _h, out _c);
        if ( _id != -1)
        {
            secondTip.SetData(_id, _h, _c);
            secondTipOverlay.SetActive(false);
            PlayerPrefs.SetInt("SecondTipUnlocked", 1);
        }

    }

    public void RemoteCall_WatchAdButtonPressed()
    {
        UnlockSecondTip();
        PlayerPrefs.SetInt("SecondTipUnlocked", 1);

        SoundManager.PlaySound2();
    }

    public void GoldStatusChanged(bool _state)
    {
        // Subscribe to GoldStatusChanged in AppManager (not done yet)
        if (_state)
        {
            allowedCount = AppManager.SAVEDTIPAMOUNT_GOLD;
        }
        else
        {
            allowedCount = AppManager.SAVEDTIPAMOUNT_FREE;
        }
    }

    private void ChangeSavedTipAmountText()
    {
        if (AppManager.isGold)
            savedTipsAmountText.text = "(" + savedTips.Count +" / " + allowedCount + ")";
        else
            savedTipsAmountText.text = "(" + savedTips.Count + " / " + allowedCount + ")";
    }

    public bool IsFull()
    {
        if (savedTips.Count >= allowedCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddSavedTip(int _id, string _header)
    {
        for (int i = 0; i < savedTips.Count; i++)
        {
            if (savedTips[i].id == _id)
            {
                return;
            }
        }


        
        if (savedTips.Count >= allowedCount)
        {
            // do some animation to the save button to tell the player what's the problem
            AppManager.ErrorHappened(ErrorMessages.SavedTipContainerIsFull());
            return;
        }
        

    

        SavedTip _newSavedTip = Instantiate(tipPrefab, transform.position, Quaternion.identity, savedTipsLayoutGroup).GetComponent<SavedTip>();
        if(_newSavedTip == null) { Debug.LogError("Tip prefab doesn't contain SavedTip component"); return; }

        savedTips.Add(_newSavedTip);
        _newSavedTip.SetData(_id, _header);
        ChangeSavedTipAmountText();
        ScrollSizer.AddSize(tipSubmenuScrollContent, tipPrefab_Y_Size);



    }

    public void BlockFirstTipSaveButton()
    {
        mainTip.SetSaveButtonState(false);
    }

    public void BlockSecondTipSaveButton()
    {
        secondTip.SetSaveButtonState(false);
    }

    public void DisplayDeleteTipPanel(int _id,string _header)
    {
        deleteTipId = _id;
        askToDeleteTipText.text = _header;
        askToDeleteTipPanel.SetActive(true);
    }

    public void RemoteCall_HideDeleteTipPanel()
    {
        askToDeleteTipPanel.SetActive(false);

        SoundManager.PlaySound6();
    }

    public int GetSecondTipId()
    {
       return secondTip.GetHeldTipId();
    }

    public void RemoteCall_RemoveSavedTip()
    {
        SavedTip _tipToDelete = null;
        for(int i = 0; i < savedTips.Count;i++)
        {
            if(savedTips[i].id == deleteTipId)
            {
                _tipToDelete = savedTips[i];
                break;
            }
        }

        if (_tipToDelete == null) { Debug.LogError("Tip id could not be found in the savedTips list: " + deleteTipId); return; }

        askToDeleteTipPanel.SetActive(false);
        savedTips.Remove(_tipToDelete);
        Destroy(_tipToDelete.gameObject);
        tipHandler.SetTipSaved(_tipToDelete.id, false);
        AlreadyContainsTipsCheck();

        ChangeSavedTipAmountText();
        ScrollSizer.ReduceSize(tipSubmenuScrollContent, tipPrefab_Y_Size);

        SoundManager.PlaySound5();
    }

    public void LoadDailyTip(int _id, string _header, string _content)
    {
        mainTip.SetData(_id, _header, _content);
    }


    public void AlreadyContainsTipsCheck()
    {
        if (tipHandler.IsIdSaved(mainTip.GetHeldTipId()))
        {
            mainTip.SetSaveButtonState(false);
        }
        else
        {
            mainTip.SetSaveButtonState(true);
        }

        if(tipHandler.IsIdSaved(secondTip.GetHeldTipId()))
        {
            secondTip.SetSaveButtonState(false);
        }
        else
        {
            secondTip.SetSaveButtonState(true);
        }
    }

    public void ShowSavedTip(int _id)
    {
        string _h;
        string _c;
        if (tipHandler.FetchHeaderAndContent(_id, out _h, out _c))
        {
            savedTip.SetData(_id, _h, _c);
            savedTipPanel.SetActive(true);

            SoundManager.PlaySound2();
        }


    }

    public void RemoteCall_HideSavedTipPanel()
    {
        savedTipPanel.SetActive(false);

        SoundManager.PlaySound3();
    }
  
}

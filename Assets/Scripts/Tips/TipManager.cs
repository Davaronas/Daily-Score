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



  
    private int secondTipId = -1;

    private List<SavedTip> savedTips = new List<SavedTip>();
    private float tipPrefab_Y_Size = 0;
    private TipMain tipMain;

    private int allowedCount;

   [HideInInspector] public int secondTipUnlockedToday = 0;

    private void Awake()
    {
        tipPrefab_Y_Size = tipPrefab.GetComponent<RectTransform>().rect.height;
        tipMain = FindObjectOfType<TipMain>();

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



       

        if(AppManager.lastLogin.Date != DateTime.Now.Date)
        {
            secondTipUnlockedToday = 0;
            PlayerPrefs.SetInt("SecondTipUnlocked", 0);
        }
        else
        {
            secondTipUnlockedToday = PlayerPrefs.GetInt("SecondTipUnlocked", 0);
        }

        if(secondTipUnlockedToday == 1)
        {
            UnlockSecondTip();
        }

    }

    public void UnlockSecondTip()
    {


        secondTip.SetData(tipMain.AskForSecondTip(out string _h, out string _c), _h, _c);
        secondTipOverlay.SetActive(false);
        SetSaveButtonState(secondTip.tipId);

        
    }

    public void RemoteCall_WatchAdButtonPressed()
    {
        UnlockSecondTip();
        PlayerPrefs.SetInt("SecondTipUnlocked", 1);
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

    public void AddSavedTip(int _id, string _header)
    {
        if(savedTips.Count >= allowedCount)
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

        SetSaveButtonState(mainTip.tipId);
        SetSaveButtonState(secondTip.tipId);

    }

    

    public void RemoveSavedTip(int _id)
    {
        SavedTip _tipToDelete = null;
        for(int i = 0; i < savedTips.Count;i++)
        {
            if(savedTips[i].id == _id)
            {
                _tipToDelete = savedTips[i];
                break;
            }
        }

        if (_tipToDelete == null) { Debug.LogError("Tip id could not be found in the savedTips list: " + _id); return; }


        savedTips.Remove(_tipToDelete);
        Destroy(_tipToDelete.gameObject);
        tipMain.DeleteTipButtonPressed(_id);
        SetSaveButtonState(_id);
        ChangeSavedTipAmountText();
        ScrollSizer.ReduceSize(tipSubmenuScrollContent, tipPrefab_Y_Size);
    }

    public void LoadDailyTip(int _id, string _header, string _content)
    {
        mainTip.SetData(_id, _header, _content);
        SetSaveButtonState(mainTip.tipId);


    }

    public void SetSaveButtonState(int _id)
    {
        if (tipMain.Loaded.Contains(_id))
        {
            if (mainTip.tipId == _id)
            {
                mainTip.SetSaveButtonState(false);
            }

            if (secondTip.tipId == _id)
            {
                secondTip.SetSaveButtonState(false);
            }
            print($"Contains id: {_id}");
        }
        else
        {
            if (mainTip.tipId == _id)
            {
                mainTip.SetSaveButtonState(true);
            }

            if (secondTip.tipId == _id)
            {
                secondTip.SetSaveButtonState(true);
            }

            print($"Does not contain id: {_id}");
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TipManager : MonoBehaviour
{
    [SerializeField] private Transform savedTipsLayoutGroup;
    [SerializeField] private GameObject tipPrefab;
    [SerializeField] private TMP_Text savedTipsAmountText;

    [SerializeField] private TMP_Text dailyTipHeader;
    [SerializeField] private TMP_Text dailyTipContent;
    [SerializeField] private RectTransform tipSubmenuScrollContent;

    [SerializeField] private GameObject saveTipButton;
    [SerializeField] private GameObject tip;



    private GameObject saveTipButton2;
    private TMP_Text dailyTipHeader2;
    private TMP_Text dailyTipContent2;



    private int dailyTipId = -1;

    private List<SavedTip> savedTips = new List<SavedTip>();
    private float tipPrefab_Y_Size = 0;
    private TipMain tipMain;

    private int allowedCount;

    public int secondTipUnlockedToday = 0;

    private void Awake()
    {
        tipPrefab_Y_Size = tipPrefab.GetComponent<RectTransform>().rect.height;
        tipMain = FindObjectOfType<TipMain>();

        if(AppManager.isGold)
        {
            allowedCount = AppManager.SAVEDTIPAMOUNT_GOLD;
        }
        else
        {
            allowedCount = AppManager.SAVEDTIPAMOUNT_FREE;
        }
        ChangeSavedTipAmountText();

        

    }

    public static void UnlockSecondTip()
    {

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
    }

    public void DisableSaveButton()
    {
        if (tipMain.Saved_ID.Contains(dailyTipId))
        {
            saveTipButton.SetActive(false);
        }
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
        ChangeSavedTipAmountText();
        ScrollSizer.ReduceSize(tipSubmenuScrollContent, tipPrefab_Y_Size);
    }

    public void LoadDailyTip(int _id, string _header, string _content)
    {
        dailyTipId = _id;
        dailyTipHeader.text = _header;
        dailyTipContent.text = _content;
        EnableSaveButton(_id);
    }

    public void EnableSaveButton(int _id)
    {
        if (!tipMain.Loaded.Contains(_id))
        {
            saveTipButton.SetActive(true);
        }
    }

    public int GetHeldTipId()
    {
        return dailyTipId;
    }

}

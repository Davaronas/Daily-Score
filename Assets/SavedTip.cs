using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SavedTip : MonoBehaviour
{
    [HideInInspector] public int id = -1;
    [HideInInspector] public string headerString;
    public TMP_Text headerText;
    private TipManager tipManager;

    public void SetData(int _id, string _header)
    {
        id = _id;
        headerString = _header;
        headerText.text = headerString;
        tipManager = FindObjectOfType<TipManager>();
    }

    public void RemoteCall_RemoveSavedTip()
    {
        tipManager.RemoveSavedTip(id);
    }
}

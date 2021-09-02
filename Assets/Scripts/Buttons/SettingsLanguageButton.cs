using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsLanguageButton : BehaviourButton
{
    [SerializeField] private AppManager.Languages language;
    private TMP_Text text;

    private void Awake()
    {
        AppManager.OnLanguageChanged += LanguageChanged;
        text = GetComponent<TMP_Text>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AppManager.OnLanguageChanged -= LanguageChanged;
    }

    private void LanguageChanged(AppManager.Languages _l)
    {
        if(_l == language)
        {
            text.fontStyle = FontStyles.Bold;
        }
        else
        {
            text.fontStyle = FontStyles.Normal;
        }
    }

    protected override void OnTouch()
    {
        base.OnTouch();

        AppManager.SetLanguage(language);

        SoundManager.PlaySound2();
    }

}

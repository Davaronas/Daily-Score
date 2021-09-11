using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetValuesReachedBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text percentText;
    [Space]
    [SerializeField] private float intervalTime = 0.1f;


    private float percent = 0;

    private int currentPercent_ = 0;
    private int percentInt_ = 0;
    private Coroutine lastWarp = null;



    private void OnDisable()
    {
        fill.fillAmount = percent;
        percentText.text = Mathf.RoundToInt(percent * 100).ToString() + " %";
    }

    public void ChangePercent(float _p)
    {
        percent = _p;

        if(lastWarp != null)
        {
            StopCoroutine(lastWarp);
        }

        if (gameObject.activeInHierarchy)
        {
            lastWarp = StartCoroutine(WarpPercent());
        }
        else
        {
            fill.fillAmount = percent;
            percentText.text = Mathf.RoundToInt(percent * 100).ToString() + " %";
        }
    }

    

    IEnumerator WarpPercent()
    {
        percentInt_ = Mathf.RoundToInt(percent * 100);

        while (currentPercent_ != percentInt_)
        {
            yield return new WaitForSeconds(intervalTime);
            currentPercent_ = Mathf.RoundToInt(fill.fillAmount * 100);


            if (currentPercent_ < percentInt_)
            {
                percentText.text = (currentPercent_ + 1).ToString() + " %";
                currentPercent_++;
            }
            else if(currentPercent_ > percentInt_)
            {
                percentText.text = (currentPercent_ - 1).ToString() + " %";
                currentPercent_--;
            }

            fill.fillAmount = (float)currentPercent_ / 100;
        }

        yield return new WaitForEndOfFrame();
    }
}

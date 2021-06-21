using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntervalPrefabUtility : MonoBehaviour
{
   
    public TMP_InputField from;
    public TMP_InputField to;
    public TMP_InputField amount;
    public TMP_Text header;

    private IntervalSummaryPrefabUtility intervalSummaryInstance;
    private TaskTypeComponents taskTypeComponents;
    private IntervalHolder intervalHolder;

    private Interval intervalConverted;

    int serialNumber = 1;

    public static explicit operator Interval(IntervalPrefabUtility _ipu)
    {
        return new Interval(int.Parse(_ipu.from.text), int.Parse(_ipu.to.text), int.Parse(_ipu.amount.text));
    }

    private void Awake()
    {
        taskTypeComponents = FindObjectOfType<TaskTypeComponents>();
        intervalHolder = FindObjectOfType<IntervalHolder>();
    }

    

    public void SetIntervalSerialNumber(int _number)
    {
        serialNumber = _number;
        header.text = serialNumber + ". " + RuntimeTranslator.TranslateIntervalWord();
    }

    public void ReduceIntervalSerialNumberByOne()
    {
        serialNumber--;
        header.text = serialNumber + ". " + RuntimeTranslator.TranslateIntervalWord();
    }

    public void RemoteCall_FinishedEditing()
    {
        if(from.text == "" || to.text == "" || amount.text == "") 
        {
            if(intervalSummaryInstance != null)
            {
                intervalHolder.SummaryRemoved();
                Destroy(intervalSummaryInstance.gameObject);
            }

            return;
        }
        else
        {
            // input fields are valid

                intervalConverted = (Interval)this;

            if (intervalSummaryInstance == null)
            {
                intervalHolder.AddIntervalSummary(this);
            }
            else
            {
                intervalSummaryInstance.UpdateNumbers(intervalConverted.from, intervalConverted.to, intervalConverted.points);
            }

        }
    }

    public void RemoteCall_Delete()
    {
        if(intervalSummaryInstance != null)
        {
            taskTypeComponents.RemoveIntervalSummary(intervalSummaryInstance);
            Destroy(intervalSummaryInstance.gameObject);
        }

        // tell task type components that this interval is dead

        intervalHolder.IntervalRemoved(this);
        taskTypeComponents.RemoveInterval(this);
        Destroy(gameObject);
    }



    public void SetIntervalSummaryInstance(IntervalSummaryPrefabUtility _intervalSummaryInstance)
    {
        intervalSummaryInstance = _intervalSummaryInstance;
    }

    public bool HasIntervalSummaryInstance()
    {
        return  intervalSummaryInstance != null;
    }

    public Interval GetIntervalData()
    {
        Interval _thisInterval = new Interval();
        _thisInterval.from = int.Parse(from.text);
        _thisInterval.to = int.Parse(to.text);
        _thisInterval.points = int.Parse(amount.text);
        return _thisInterval;
    }

    public void GetRange(out int _from, out int _to)
    {
       
        _from = intervalConverted.from;
        _to = intervalConverted.to;
    }

    public int GetSerialNumber()
    {
        return serialNumber;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IntervalHolderItemType {Interval, Summary };

public class IntervalHolder : MonoBehaviour
{
  //  public float y_size { get; private set; } = 0;

    [SerializeField] private GameObject intervalPrefab;
    [SerializeField] private GameObject intervalSummaryPrefab;
    [SerializeField] private RectTransform createTaskScrollContent;

    [SerializeField] private RectTransform addIntervalButton;

    private float intervalPrefab_y = 0;
    private float intervalSummaryPrefab_y = 0;
    private float originalScrollContentSize_y;
    private float scrollContentSizeAfterUse_y;

  //  private RectTransform parent_taskTypeTextHolder;
  //  private RectTransform rectTransform;

  


    private TaskTypeComponents taskTypeComponents;

    private void Awake()
    {
     //   parent_taskTypeTextHolder = transform.parent.GetComponent<RectTransform>();
      //  rectTransform = GetComponent<RectTransform>();
        originalScrollContentSize_y = createTaskScrollContent.sizeDelta.y;
       

        intervalPrefab_y = intervalPrefab.GetComponent<RectTransform>().sizeDelta.y;
        intervalSummaryPrefab_y = intervalSummaryPrefab.GetComponent<RectTransform>().sizeDelta.y;

        taskTypeComponents = FindObjectOfType<TaskTypeComponents>();
    }

    private void OnDisable()
    {
        scrollContentSizeAfterUse_y = createTaskScrollContent.sizeDelta.y;
        createTaskScrollContent.sizeDelta = new Vector2(createTaskScrollContent.sizeDelta.x, originalScrollContentSize_y);
        

    }

    private void OnEnable()
    {
      //  createTaskScrollContent.sizeDelta = new Vector2(createTaskScrollContent.sizeDelta.x, scrollContentSizeAfterUse_y);
    }

    public void EditMode_AddInterval(int _serialNumber, int _from, int _to, int _amount)
    {
        GameObject _newInterval = Instantiate(intervalPrefab, transform.position, Quaternion.identity, transform);
        IntervalPrefabUtility _ipu = _newInterval.GetComponent<IntervalPrefabUtility>();
        _ipu.SetIntervalSerialNumber(taskTypeComponents.GetIntervalAmount() + 1);
        _ipu.SetNumbers(_from, _to, _amount, this);
        _newInterval.transform.SetSiblingIndex(taskTypeComponents.GetIntervalAmount()); // put this after the last interval, but beofre the button
        taskTypeComponents.AddInterval(_ipu);



        intervalPrefab_y = intervalPrefab.GetComponent<RectTransform>().sizeDelta.y;

        ScrollSizer.AddSize(createTaskScrollContent, intervalPrefab_y);
    }

    public void RemoteCall_AddInterval()
    {
       
            GameObject _newInterval = Instantiate(intervalPrefab, transform.position, Quaternion.identity, transform);
        IntervalPrefabUtility _ipu = _newInterval.GetComponent<IntervalPrefabUtility>();
        _ipu.SetIntervalSerialNumber(taskTypeComponents.GetIntervalAmount() + 1);
        _newInterval.transform.SetSiblingIndex(taskTypeComponents.GetIntervalAmount()); // put this after the last interval, but beofre the button
            taskTypeComponents.AddInterval(_ipu);

      

        intervalPrefab_y = intervalPrefab.GetComponent<RectTransform>().sizeDelta.y;
       
            ScrollSizer.AddSize(createTaskScrollContent, intervalPrefab_y);
        
    }

    public void AddIntervalSummary(IntervalPrefabUtility _ipu)
    {
       
            GameObject _newIntervalSummary = Instantiate(intervalSummaryPrefab, transform.position, Quaternion.identity, transform);
        IntervalSummaryPrefabUtility _newIspu = _newIntervalSummary.GetComponent<IntervalSummaryPrefabUtility>();
        _newIspu.FillOutTexts(int.Parse(_ipu.from.text), int.Parse(_ipu.to.text),taskTypeComponents.GetIntervalMetricType() ,int.Parse(_ipu.amount.text));
            _ipu.SetIntervalSummaryInstance(_newIspu);
            taskTypeComponents.AddIntervalSummary(_newIspu);

        _newIntervalSummary.transform.SetSiblingIndex(transform.childCount); // put this last
        

        intervalSummaryPrefab_y = intervalSummaryPrefab.GetComponent<RectTransform>().sizeDelta.y;
        
            ScrollSizer.AddSize(createTaskScrollContent, intervalSummaryPrefab_y);
        
    }

    public void IntervalRemoved(IntervalPrefabUtility _ipu)
    {
        intervalPrefab_y = intervalPrefab.GetComponent<RectTransform>().sizeDelta.y;
        intervalSummaryPrefab_y = intervalSummaryPrefab.GetComponent<RectTransform>().sizeDelta.y;
        if (_ipu.HasIntervalSummaryInstance())
        {
            ScrollSizer.ReduceSize(createTaskScrollContent, intervalPrefab_y + intervalSummaryPrefab_y);
        }
        else
        {
            ScrollSizer.ReduceSize(createTaskScrollContent, intervalPrefab_y);
        }
    }

    public void SummaryRemoved()
    {
        intervalSummaryPrefab_y = intervalSummaryPrefab.GetComponent<RectTransform>().sizeDelta.y;
        ScrollSizer.ReduceSize(createTaskScrollContent,intervalSummaryPrefab_y);
    }


    public void Clear()
    {
        scrollContentSizeAfterUse_y = originalScrollContentSize_y;


    }

    
    

}

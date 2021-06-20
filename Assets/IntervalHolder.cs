using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IntervalHolderItemType {Interval, Summary };

public class IntervalHolder : MonoBehaviour
{
    public float y_size { get; private set; } = 0;

    [SerializeField] private GameObject intervalPrefab;
    [SerializeField] private GameObject intervalSummaryPrefab;
    [SerializeField] private RectTransform createTaskScrollContent;

    private float intervalPrefab_y = 0;
    private float originalScrollContentSize_y;

    private RectTransform parent_taskTypeTextHolder;
    private RectTransform rectTransform;

    private void Awake()
    {
        parent_taskTypeTextHolder = transform.parent.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        originalScrollContentSize_y = createTaskScrollContent.sizeDelta.y;
    }

    private void OnDisable()
    {
        createTaskScrollContent.sizeDelta = new Vector2(createTaskScrollContent.sizeDelta.x, originalScrollContentSize_y);
    }

    public void AddItem(IntervalHolderItemType _item)
    {
        intervalPrefab_y = intervalPrefab.GetComponent<RectTransform>().sizeDelta.y;
        y_size += intervalPrefab_y;
        if(y_size > parent_taskTypeTextHolder.sizeDelta.y)
        {
            ScrollSizer.AddSize(createTaskScrollContent, intervalPrefab_y);
        }
    }

    public void IntervalRemoved()
    {
        intervalPrefab_y = intervalPrefab.GetComponent<RectTransform>().sizeDelta.y;
        y_size -= intervalPrefab_y;
        if (y_size > parent_taskTypeTextHolder.sizeDelta.y)
        {
            ScrollSizer.ReduceSize(createTaskScrollContent, intervalPrefab_y);
        }
    }

    public void Clear()
    {
        y_size = 0;
    }

    
    

}

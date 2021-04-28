using UnityEngine;
using UnityEngine.UI;

public class AddGoalButton : BehaviourButton
{
    [SerializeField] private RectTransform goalsScrollContentRectTransform = null;
    [SerializeField] private GameObject goalPrefab = null;

    private ScrollRect goalScrollRect = null;

    protected override void Start()
    {
        base.Start();

        goalScrollRect = goalsScrollContentRectTransform.GetComponent<ScrollRect>();
    }

    protected override void OnTouch()
    {
       if(goalsScrollContentRectTransform == null)
        {
            Debug.LogError("The object the prefab needs to be parented to is null");
        }

        // scale scrollrect content

        Instantiate(goalPrefab, Vector3.zero, Quaternion.identity, goalsScrollContentRectTransform.transform);
        AppManager.NewGoalAdded();

       
    }
}

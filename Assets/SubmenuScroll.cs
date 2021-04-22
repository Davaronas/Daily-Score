using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmenuScroll : BehaviourButton
{
    private List<Vector2> submenuPositions = new List<Vector2>();

    public void Initialize()
    {
        for(int i = 0;i < transform.childCount;i++)
        {
            submenuPositions.Add(transform.GetChild(i).position);
        }
    }
}

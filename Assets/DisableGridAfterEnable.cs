using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DisableGridAfterEnable : MonoBehaviour
{
    private GridLayoutGroup glg = null;
    private bool ignoreEnableForStart = true;

    void Awake()
    {
        glg = GetComponent<GridLayoutGroup>();
    }

    private void OnEnable()
    {
        if (!ignoreEnableForStart)
        {
            Invoke(nameof(DisableGrid), Time.deltaTime * 3);
        }
        else
        {
            ignoreEnableForStart = false;
        }
    }

    private void DisableGrid()
    {
        glg.enabled = false;
    }

    private void OnDisable()
    {
        glg.enabled = true;
    }
}

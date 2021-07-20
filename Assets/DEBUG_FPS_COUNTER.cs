using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DEBUG_FPS_COUNTER : MonoBehaviour
{
    private TMP_Text fpsCounter = null;
    void Start()
    {
        fpsCounter = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
    }
}

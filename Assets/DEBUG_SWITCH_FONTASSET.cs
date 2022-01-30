using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//[ExecuteInEditMode]
public class DEBUG_SWITCH_FONTASSET : MonoBehaviour
{

    TMP_Text t;

    [SerializeField] TMP_FontAsset medium;
    [SerializeField] TMP_FontAsset regular;

    void Update()
    {
        if(t == null)
        {
            t = GetComponent<TMP_Text>();
        }

        if(t != null && medium != null && regular != null)
        {
            if(t.font == medium)
            {
                t.font = regular;
            }
        }
    }

   
}

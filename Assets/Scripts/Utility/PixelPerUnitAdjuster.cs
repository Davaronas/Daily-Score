using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelPerUnitAdjuster : MonoBehaviour
{
    private Image image;


    void Start()
    {
        Invoke(nameof(Adjust), Time.deltaTime * 4);
    }

    private void Adjust()
    {
        image = GetComponent<Image>();
        if (image.type == Image.Type.Tiled)
        {
            image.pixelsPerUnitMultiplier = 64 / image.GetComponent<RectTransform>().rect.width;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

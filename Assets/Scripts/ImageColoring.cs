using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColoring : MonoBehaviour
{
    private Image image = null;
    private Texture2D baseTexture = null;
    private Texture2D cloneTexture = null;

    private Color color1 = Color.green;
    private Color color2 = Color.magenta;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image.sprite == null) { Debug.LogError("No sprite on image"); return; }


        baseTexture = image.sprite.texture;
        cloneTexture = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.RGBA32, false);

        UpdateTexture();
    }

    private void Update()
    {
        
    }

    private void UpdateTexture()
    {
       

        if (cloneTexture != null)
        {
            for (int i = 0; i < cloneTexture.width; i++)
            {
                for (int j = 0; j < cloneTexture.height; j++)
                {
                    Color32 _newColor = Color32.Lerp(color2, color1, (float)j / cloneTexture.height);
                    print((float)j / cloneTexture.height);
                    print(_newColor);
                    Color32 _originalColor = baseTexture.GetPixel(i, j);
                    if(_originalColor != Color.black)
                    cloneTexture.SetPixel(i, j, new Color32(_newColor.r, _newColor.g, _newColor.b, _originalColor.a));
                }
            }

            cloneTexture.Apply();
            Rect _rect = new Rect(0, 0, cloneTexture.width, cloneTexture.height);
            image.sprite = Sprite.Create(cloneTexture, _rect, new Vector2(0.5f, 0.5f));
        }

        
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteAlways]
public class ImageColoring : MonoBehaviour
{
    public enum GradientType {Vertical, Horizontal, LeftToRightDiagonal, RightToLeftDiagonal}

    private Image image = null;
    private Texture2D baseTexture = null;
    private Texture2D cloneTexture = null;

    private Sprite originalSprite = null;

    public GradientType gradient = GradientType.Vertical;
    public Color32 color1  = Color.black;
    public Color32 color2 = Color.white;

    [Space]
    public bool overrideAutoBorder = false;
    [Space]
    public float L = 0;
    public float R = 0;
    public float T = 0;
    public float B = 0;


    [Space]
    public float alphaCheck = 250f;


    private float avg_ = 0;

  

    public void SetColor(GoalColor _color1, GoalColor _color2)
    {
        color1 = _color1;
        color2 = _color2;


        UpdateTexture();
    }

    [ContextMenu("Switch colors")]
    public void SwitchColors()
    {
        Color32 _t = color1;
        color1 = color2;
        color2 = _t;
    }

    [ContextMenu("UpdateColor")]
    public void UpdateTexture()
    {
     
        image = GetComponent<Image>();
        if (image.sprite == null) { Debug.LogError("No sprite on image"); return; }

        originalSprite = image.sprite;
        baseTexture = originalSprite.texture;

        cloneTexture = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.RGBA32, false);
       

        if (cloneTexture != null)
        {
            switch(gradient)
            {
                case GradientType.Vertical:
                    VerticalColoring();
                    break;
                case GradientType.Horizontal:
                    HorizontalColoring();
                    break;
                case GradientType.LeftToRightDiagonal:
                    LeftToRightColoring();
                    break;
                case GradientType.RightToLeftDiagonal:
                    RightToLeftColoring();
                    break;
            }

            cloneTexture.Apply(true);
            Rect _rect = new Rect(0, 0, cloneTexture.width, cloneTexture.height);
            Vector4 _newBorder = new Vector4(0, 0, 0, 0);

            if (overrideAutoBorder)
            {
                _newBorder = new Vector4(L, R, T, B);
            }
            else
            {
                _newBorder = new Vector4(cloneTexture.width / 10, cloneTexture.width / 10, cloneTexture.height / 10, cloneTexture.height / 10);
            }


            image.sprite = Sprite.Create(cloneTexture, _rect, new Vector2(cloneTexture.width / 2, cloneTexture.height / 2), 100, 1, SpriteMeshType.FullRect, _newBorder);
        }

    }

    private void VerticalColoring()
    {


        for (int i = 0; i < cloneTexture.width; i++)
        {
            for (int j = 0; j < cloneTexture.height; j++)
            {
                Color32 _newColor = Color32.Lerp(color2, color1, (float)j / cloneTexture.height);
                Color32 _originalColor = baseTexture.GetPixel(i, j);
                if (!isCloseToBlack(_originalColor) && isAlphaOverThreshold(_originalColor))
                    cloneTexture.SetPixel(i, j, new Color32(_newColor.r, _newColor.g, _newColor.b, _originalColor.a));
                else
                    cloneTexture.SetPixel(i, j, _originalColor);
            }
        }
    }

    private void HorizontalColoring()
    {


        for (int i = 0; i < cloneTexture.width; i++)
        {
            for (int j = 0; j < cloneTexture.height; j++)
            {
                Color32 _newColor = Color32.Lerp(color2, color1, (float)i / cloneTexture.height);
                Color32 _originalColor = baseTexture.GetPixel(i, j);
                if (!isCloseToBlack(_originalColor) && isAlphaOverThreshold(_originalColor))
                    cloneTexture.SetPixel(i, j, new Color32(_newColor.r, _newColor.g, _newColor.b, _originalColor.a));
                else
                    cloneTexture.SetPixel(i, j, _originalColor);
            }
        }
    }

    private void RightToLeftColoring()
    {

        for (int i = 0; i < cloneTexture.width; i++)
        {
            for (int j = 0; j < cloneTexture.height; j++)
            {
                Color32 _newColor = Color32.Lerp(color2, color1, (((float)j + (float)i) /2) /  cloneTexture.height);
                Color32 _originalColor = baseTexture.GetPixel(i, j);
                if (!isCloseToBlack(_originalColor) && isAlphaOverThreshold(_originalColor))
                    cloneTexture.SetPixel(i, j, new Color32(_newColor.r, _newColor.g, _newColor.b, _originalColor.a));
                else
                    cloneTexture.SetPixel(i, j, _originalColor);
            }
        }
    }

    private void LeftToRightColoring()
    {


        for (int i = 0; i < cloneTexture.width; i++)
        {
            for (int j = 0; j < cloneTexture.height; j++)
            {
                Color32 _newColor = Color32.Lerp(color2, color1,  (((float)j + (float)i) / 2) / cloneTexture.height);
                Color32 _originalColor = baseTexture.GetPixel(i, j);
                if (!isCloseToBlack(_originalColor) && isAlphaOverThreshold(_originalColor))
                    cloneTexture.SetPixel(i, j, new Color32(_newColor.r, _newColor.g, _newColor.b, _originalColor.a));
                else
                    cloneTexture.SetPixel(i, j, _originalColor);
            }
        }
    }

    private bool isCloseToBlack(Color32 _c)
    {
        if (isLowColorValues(_c) && valuesAreCloseToEachother(_c))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isLowColorValues(Color32 _c)
    {
        if (_c.r <= 80 && _c.g <= 80 && _c.b <= 80)
            return true;
        else
            return false;
    }

    private bool valuesAreCloseToEachother(Color32 _c)
    {
        avg_ = (float)(_c.r + _c.g + _c.b) / 3;
        if (Mathf.Abs(_c.r - avg_) > 15 ||
            Mathf.Abs(_c.g - avg_) > 15 ||
            Mathf.Abs(_c.b - avg_) > 15)
        {

            return false;
        }
        else
        {
            return true;
        }
    }

    private bool isAlphaOverThreshold(Color32 _c)
    {
        if(_c.a >= alphaCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

  
    
}

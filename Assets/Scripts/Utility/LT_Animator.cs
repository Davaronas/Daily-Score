using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class LT_Animator
{


    public static void ColorTransition(RectTransform _what, Color _color, float _speed)
    {

      //  LeanTween.colorText(_what, _color, _speed);
        LeanTween.color(_what, _color, _speed);
   
        
    }

  

    public static void SizeTransition(RectTransform _what, Vector2 _sizeToTransition, float _speed)
    {
        LeanTween.size(_what, _sizeToTransition, _speed);
    }

    public static void RotateAround(RectTransform _what, float _speed, int _degree = 360)
    {
        LeanTween.rotateAround(_what, new Vector3(0, 0, 1), _degree, _speed);
    }
}

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
}

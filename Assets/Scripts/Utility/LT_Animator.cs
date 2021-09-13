using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class LT_Animator
{


    public static LTDescr ColorTransition(RectTransform _what, Color _color, float _speed)
    {

      //  LeanTween.colorText(_what, _color, _speed);
        LTDescr _desc = LeanTween.color(_what, _color, _speed);

        return _desc;
    }

  

    public static LTDescr SizeTransition(RectTransform _what, Vector2 _sizeToTransition, float _speed)
    {
        LTDescr _desc = LeanTween.size(_what, _sizeToTransition, _speed);

        return _desc;
    }

  



    public static LTDescr RotateAround(RectTransform _what, float _speed, int _degree = 360)
    {
        LTDescr _desc = LeanTween.rotateAround(_what, new Vector3(0, 0, 1), _degree, _speed);

        return _desc;

    }

    public static LTDescr Move(RectTransform _what, Vector2 _from, Vector2 _to, float _speed)
    {
        _what.position = _from;
        LTDescr _desc = LeanTween.move(_what.gameObject, _to, _speed);

        return _desc;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenIcon : MonoBehaviour
{

    [SerializeField] private Transform startPos = null;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector2 startSize;
    [SerializeField] private Vector2 endSize;
    


    // Start is called before the first frame update
    void Start()
    {
        RectTransform _rt = GetComponent<RectTransform>();
        LT_Animator.Move(_rt, startPos.position, transform.position, speed);
        LT_Animator.ColorTransition(_rt, Color.white, speed);

        _rt.sizeDelta = startSize;
        LT_Animator.SizeTransition(_rt, endSize, speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

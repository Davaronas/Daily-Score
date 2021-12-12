using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenIcon : MonoBehaviour
{

    [SerializeField] private Transform startPos = null;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector2 startSize;
    [SerializeField] private Vector2 endSize;

    private Loading loading = null;
    private bool started = false;
    


    // Start is called before the first frame update
    void Start()
    {
        loading = FindObjectOfType<Loading>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimation()
    {
        if (started) { return; }
        started = true;

        RectTransform _rt = GetComponent<RectTransform>();
        _rt.sizeDelta = startSize;
        LT_Animator.Move(_rt, startPos.position, transform.position, speed);
        LT_Animator.ColorTransition(_rt, Color.white, speed);

        
        LTDescr _desc = LT_Animator.SizeTransition(_rt, endSize, speed);
       

        Invoke(nameof(AllowSceneActivation), _desc.time);
    }    

    private void AllowSceneActivation()
    {
        loading.AllowSceneActivation();
    }
}

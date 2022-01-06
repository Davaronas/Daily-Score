using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderStar : MonoBehaviour
{


    private RectTransform star = null;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private int rotateDegree = 180;
    [SerializeField] private float sizeIncreaseSpeed = 5f;

    private Vector2 originalSize;

    private void Awake()
    {
        star = transform.GetChild(0).GetComponent<RectTransform>();
        originalSize = star.rect.size;
        star.gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        Invoke(nameof(ResetStar), Time.deltaTime * 3);
    }

    private void OnEnable()
    {
        Invoke(nameof(ResetStar), Time.deltaTime * 3);
    }

    private void ResetStar()
    {
        star.rotation = Quaternion.Euler(Vector3.zero);
        star.sizeDelta = originalSize;
    }


    public float Achieved()
    {
        star.rotation = Quaternion.Euler(Vector3.zero);
        star.sizeDelta = Vector2.zero;
        star.gameObject.SetActive(true);
        LT_Animator.RotateAround(star, rotateSpeed, rotateDegree);
        LTDescr _dscr =  LT_Animator.SizeTransition(star, originalSize, sizeIncreaseSpeed);
        Invoke(nameof(SetRotation), _dscr.time + Time.deltaTime);
        return _dscr.time;
    }

    public float GetTransitionTime()
    {
        return sizeIncreaseSpeed;
    }


    public void Cancel()
    {
        ResetStar();
        star.gameObject.SetActive(false);
    }

    private void SetRotation()
    {
        star.rotation = Quaternion.Euler(Vector3.zero);
    }

    public bool IsStarActivated()
    {
        return star.gameObject.activeInHierarchy;
    }
}

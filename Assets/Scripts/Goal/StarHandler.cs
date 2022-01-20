using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarHandler : MonoBehaviour
{
    [SerializeField] private HeaderStar[] stars = new HeaderStar[5];
    private Coroutine[] starEnableCoroutines = new Coroutine[5];
    private float transitionTime = 0f;


    private int shortenAnimationDelay_ = 0;


    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            stars[i] = transform.GetChild(i).GetComponent<HeaderStar>();
        }

        transitionTime = stars[0].GetTransitionTime();
    }

   

    
    public void AchievedStars(int _amount)
    {
        if(_amount < 0 ||_amount > 5)
        {
            return;
        }

        shortenAnimationDelay_ = 0;

        for (int i = 0; i < _amount; i++)
        {
           // print(i + " " + (i - shortenAnimationDelay_) * transitionTime);

            if (!stars[i].IsStarActivated())
            {
                if (gameObject.activeInHierarchy)
                {
                   starEnableCoroutines[i] = StartCoroutine(EnableStar(stars[i], (i - shortenAnimationDelay_) * transitionTime));
                }
                else
                {
                    stars[i].Achieved();
                }
            }
            else
            {
                shortenAnimationDelay_++;
            }
        }

        for (int i = 0; i < stars.Length; i++)
        {
            if(i + 1 > _amount)
            {
                if(starEnableCoroutines[i] != null)
                {
                    StopCoroutine(starEnableCoroutines[i]);
                }
                stars[i].Cancel();
            }
        }
    }


    IEnumerator EnableStar(HeaderStar _hd, float _t)
    {
        yield return new WaitForSeconds(_t);
        _hd.Achieved();
    }
}

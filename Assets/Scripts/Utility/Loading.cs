using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{

    [SerializeField] private TMP_Text percentText = null;
    [SerializeField] private Image loadingFill = null;

    private AsyncOperation aOp = null;

    private LoadingScreenIcon lsi = null;

    private void Start()
    {
        StartCoroutine(LoadMainScene());
        lsi = FindObjectOfType<LoadingScreenIcon>();
    }

    IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(0.5f);

         aOp = SceneManager.LoadSceneAsync("Main");
        aOp.allowSceneActivation = false;

        yield return new WaitForEndOfFrame();


        while (!aOp.isDone)
        {
            percentText.text =  "Loading...  " + Mathf.Round(aOp.progress * 100).ToString() + " %";
            loadingFill.fillAmount = Mathf.Clamp( aOp.progress / 1f, loadingFill.fillAmount,1f);
            yield return new WaitForEndOfFrame();

            if(aOp.progress >= 0.9f)
            {
                lsi.StartAnimation();
            }
        }

       
        
    }

    public void AllowSceneActivation()
    {
        aOp.allowSceneActivation = true;
    }
}

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

    private void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation aOp = SceneManager.LoadSceneAsync("Main");

        yield return new WaitForEndOfFrame();


        while (!aOp.isDone)
        {
            percentText.text = Mathf.Round(aOp.progress * 100).ToString() + " %";
            loadingFill.fillAmount = aOp.progress / 1f;
            yield return new WaitForEndOfFrame();
        }
    }
}

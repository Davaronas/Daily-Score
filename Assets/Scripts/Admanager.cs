using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class Admanager : MonoBehaviour
{ //Gombos cucc és itt a log mûködik maga az ad nem jön be
	public Button yourButton;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		Debug.Log("You have clicked the button!");
		if (Advertisement.IsReady("video")) //Ad checker
		{
		    Advertisement.Show("video"); //Adot ez fogja elindítani
		}
	}

}
//(Input.GetKeyDown(KeyCode.A))
//if(Advertisement.IsReady("video"))
//{
//    Advertisement.Show("video");
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText = null;
    [SerializeField] private TMP_Text scoreText = null;
    [SerializeField] private Image imageToApplyColorTo = null;

    private const byte defaultTransparency = 255;

    private List<Task> tasks = new List<Task>();

    private string goalName = "Default name";
    private Color32 goalColor = new Color32(0, 0, 0, defaultTransparency);
    private int symbolId = -1;


    public void SetData(string _name, Color32 _color, int _symbolId = -1)
    {
        if(nameText == null || scoreText == null || imageToApplyColorTo == null)
        {
            Debug.LogError("Goal not setup properly, please ensure you set all components from the editor");
            return;
        }

        goalName = _name;
        goalColor = _color;
        symbolId = _symbolId;

        Initialize();
    }

    private void Initialize()
    {
        nameText.text = goalName;
        scoreText.text = "0" + " Points";
        imageToApplyColorTo.color = goalColor;

        // set up sprites
      

    }
}

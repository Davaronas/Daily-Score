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
    private ImageColoring imageColoring = null;

    private ImageColoring gradient = null;

    public bool isPrefab = false;

    private const byte defaultTransparency = 255;


    public string goalName { get; private set; } = "Default name";
  
    public int symbolId { get; private set; } = -1;

   [SerializeField] private GoalData goalData;

    private GoalManager goalManager;


    private void Awake()
    {
        AppManager.OnTaskValueChanged += TaskChanged;
        goalManager = FindObjectOfType<GoalManager>();
       
    }

    private void OnDestroy()
    {
        AppManager.OnTaskValueChanged -= TaskChanged;
    }

    private void TaskChanged(TaskData _td)
    {
        if(goalManager.SearchGoalDataByName(_td.owner) == goalData)
        {
            goalData.current = GetPoints();
            scoreText.text = goalData.current + " p";
        }
    }

    public void SetData(GoalData _goalData)
    {
        if (isPrefab) { return; }

        if (nameText == null || scoreText == null || imageToApplyColorTo == null)
        {
            Debug.LogError("Goal not setup properly, please ensure you set all components from the editor");
            return;
        }

        goalData = _goalData;

        goalName = _goalData.name;
        symbolId = _goalData.spriteId;

        imageColoring = GetComponent<ImageColoring>();

        Initialize();
    }

    private void Initialize()
    {
        nameText.text = goalName;
        scoreText.text = goalData.current + " P";

        Color32 _mix = new Color32((byte)((goalData.color[0].r + goalData.color[1].r) / 2),
            (byte)((goalData.color[0].g + goalData.color[1].g) / 2),
            (byte)((goalData.color[0].b + goalData.color[1].b) / 2),
            (byte)((goalData.color[0].a + goalData.color[1].a) / 2));

        nameText.color = _mix ;
        scoreText.color = _mix;

        /*
        switch(goalData.colorType)
        {
            case ColorType.Simple:
                imageToApplyColorTo.color = goalData.color[0];
                break;
            case ColorType.Gradient:
                imageToApplyColorTo.color = Color.white;

                gradient = (ImageColoring)imageToApplyColorTo.gameObject.AddComponent(typeof(ImageColoring));
                gradient.SetColor(goalData.color[0], goalData.color[1]);
                break;
            case ColorType.FourCornerGradient:
                // this might not be needed
                break;
            default:
                Debug.LogError("ColorType enum doesn't contain this element: " + goalData.colorType);
                break;
        }
        */

        imageColoring.color1 = goalData.color[0];
        imageColoring.color2 = goalData.color[1];
        imageColoring.UpdateTexture();

        // set up sprites
    }

    public void AddTask(TaskData _task)
    {
        

        goalData.tasks.Add(_task);

      
    }

    public void UpdateTask(TaskData _taskToUpdate)
    {

    }


    public TaskData[] GetTasks()
    {
        

        return goalData.tasks.ToArray();
    }

    public bool SearchForTask(string _task)
    {
        for (int i = 0; i < goalData.tasks.Count; i++)
        {
            if(goalData.tasks[i].name == _task)
            {
                return true;
            }
        }

        return false;
    }

    public GoalData GetGoalData()
    {
        return goalData;
    }

    public int GetPoints()
    {
        int _amount = 0;

        for (int i = 0; i < goalData.tasks.Count; i++)
        {
            _amount += TaskPointCalculator.GetPointsFromCurrentValue(goalData.tasks[i]);
        }


        return _amount;
    }
}

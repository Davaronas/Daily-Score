using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : Graphic
{

    private Vector2[] drawPositions;
    [SerializeField] private float lineThickness = 3f;
    [SerializeField] private Color lineColor = Color.red;
    [SerializeField] private float distanceMultiplier = 0.1f;
    private RollingAverage rollingAverageChart = null;

    [SerializeField] private GameObject circlePrefab = null;

    private List<GameObject> circles = new List<GameObject>();

    [SerializeField] private RectTransform mainMenuMask = null;




    protected override void Awake()
    {
        base.Awake();
        rollingAverageChart = FindObjectOfType<RollingAverage>();

        

    }

    public void UpdatePositions(Vector2[] _positions)
    {
        

        drawPositions = _positions;

        for (int i = 0; i < circles.Count; i++)
        {
            Destroy(circles[i]);
        }
        circles.Clear();

        for (int j = 0; j < drawPositions.Length; j++)
        {
           circles.Add(Instantiate(circlePrefab, drawPositions[j], Quaternion.identity, transform));
        }

        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (rollingAverageChart != null)
        {
            drawPositions = rollingAverageChart.GetPositions();
        }

     

        if (vh == null || drawPositions == null) { return; }

        if(drawPositions.Length < 2)
        {
            return;
        }


       
        
        if(drawPositions.Length == 0) { return; }



        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = lineColor;

        Vector2 _lastPos = new Vector2(0,0);
        float _angle = 1;
        int i = 0;
        for (i = 0; i < drawPositions.Length; i++)
        {

            
            if (i != 0)
            {
                _angle = (float)(Mathf.Atan2(drawPositions[i].y - _lastPos.y, drawPositions[i].x - _lastPos.x) * (180 / Mathf.PI)) + 45f;
            }






            if (i != 0)
            {


                Vector2 _convert = transform.worldToLocalMatrix.MultiplyPoint3x4(drawPositions[i]);
                Vector2 _convertLast = transform.worldToLocalMatrix.MultiplyPoint3x4(_lastPos);


 
                    vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(-lineThickness / 2, 0);
                    vertex.position += new Vector3(_convert.x, _convert.y);
                    vh.AddVert(vertex);

                    vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(lineThickness / 2, 0);
                    vertex.position += new Vector3(_convert.x, _convert.y);
                    vh.AddVert(vertex);


                    vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(-lineThickness / 2, 0);
                    vertex.position += new Vector3(_convertLast.x, _convertLast.y);
                    vh.AddVert(vertex);

                    vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(lineThickness / 2, 0);
                    vertex.position += new Vector3(_convertLast.x, _convertLast.y);
                    vh.AddVert(vertex);
                
                


                /*
                vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(-lineThickness / 2, 0);
                vertex.position += new Vector3(drawPositions[i].x, drawPositions[i].y);
                vh.AddVert(vertex);

                vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(lineThickness / 2, 0);
                vertex.position += new Vector3(drawPositions[i].x, drawPositions[i].y);
                vh.AddVert(vertex);


                vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(-lineThickness / 2, 0);
                vertex.position += new Vector3(_lastPos.x, _lastPos.y);
                vh.AddVert(vertex);

                vertex.position = Quaternion.Euler(0, 0, _angle) * new Vector2(lineThickness / 2, 0);
                vertex.position += new Vector3(_lastPos.x, _lastPos.y);
                vh.AddVert(vertex);
                */
                



            }

       
           


            _lastPos = drawPositions[i];
            if (i != 0)
            {
              
                vh.AddTriangle((((i - 1) * 4) + 1), (((i - 1) * 4) + 1) - 1, (((i - 1) * 4) + 1) + 1);
                vh.AddTriangle((((i - 1) * 4) + 1), (((i - 1) * 4) + 1) + 2, (((i - 1) * 4) + 1) + 1);
            }



            for (int j = 0; j < circles.Count; j++)
            {
                    circles[j].transform.position = drawPositions[j];
                if(!RectTransformUtility.RectangleContainsScreenPoint(mainMenuMask, drawPositions[i]))
                {
                    vh.Clear();
                    break;
                }
            }
        }

     


    }


  

}

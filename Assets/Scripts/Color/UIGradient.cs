using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class UIGradient : BaseMeshEffect
{
    public Color m_color1 = Color.white;
    public Color m_color2 = Color.white;
    [Range(-180f, 180f)]
    public float m_angle = 0f;
    public bool m_ignoreRatio = true;
    private Graphic gr = null;

    public override void ModifyMesh(VertexHelper vh)
    {
        if(enabled)
        {
            

            Rect rect = graphic.rectTransform.rect;
            Vector2 dir = UIGradientUtils.RotationDir(m_angle);

            if (!m_ignoreRatio)
                dir = UIGradientUtils.CompensateAspectRatio(rect, dir);

            UIGradientUtils.Matrix2x3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

            UIVertex vertex = default(UIVertex);
            for (int i = 0; i < vh.currentVertCount; i++) {
                vh.PopulateUIVertex (ref vertex, i);
                Vector2 localPosition = localPositionMatrix * vertex.position;
                vertex.color *= Color.Lerp(m_color2, m_color1, localPosition.y);
                vh.SetUIVertex (vertex, i);
            }
        }
    }

    public void SetProperties(Color _color1, Color _color2, float _angle = 0)
    {
        gr = GetComponent<Graphic>();

        m_color1.r = _color1.r;
        m_color1.g = _color1.g;
        m_color1.b = _color1.b;

        m_color2.r = _color2.r;
        m_color2.g = _color2.g;
        m_color2.b = _color2.b;

        print(_color1 + " " + _color2);
        gr.SetVerticesDirty();
        Invoke(nameof(Refresh), 0.2f);
    }

    private void Refresh()
    {
        m_color1.a = 120;
        m_color2.a = 120;
    }

    
}

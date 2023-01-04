using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasRenderer))]
public class GraphRenderer : MaskableGraphic
{
    [SerializeField] private float _length = 0.5f;
    [SerializeField] private float _scale = 1;
    [Range(0.1f, 1f)]
    [SerializeField] private float _step = 0.1f;
    [SerializeField] private float _size = 1f;
    private List<Vector2> _points;
    protected Vector3[] _CornersArray = new Vector3[4];

    private void GeneratePoints()
    {
        _points = new List<Vector2>();
        var pointsCount = _length / _step + 1;
        for (int i = 0; i < pointsCount; i++)
        {
            float x = (float) i * _step - _length / 2;
            _points.Add(new Vector2(x * _scale ,   _scale * x * x));
        }
    }
    protected override void OnValidate()
    {
        base.OnValidate();
        GeneratePoints();
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        rectTransform.GetLocalCorners(_CornersArray);
        if (_points is {Count: > 2})
        {
            for (int i = 0; i < _points.Count - 1; i++)
            {
                var p1 = _points[i];
                var p2 = _points[i + 1];
                RenderLine(vh, p1, p2, _size, color);
            }
        }
    }
    public static void RenderLine(VertexHelper vh, Vector2 start, Vector2 end, float size, Color color)
    {
        var uvVectorLeft = new Vector2(0, size);
        var uvVectorRight= new Vector2(1, size);
        var vertexOffset = vh.currentVertCount;
        var normal = new Vector2(end.y - start.y, start.x - end.x).normalized;
        vh.AddVert(start + normal * size, color, uvVectorRight);
        vh.AddVert(start - normal * size, color, uvVectorLeft);
        vh.AddVert(end - normal * size, color, uvVectorLeft);
        vh.AddVert(end + normal * size, color, uvVectorRight);
        vh.AddTriangle(vertexOffset, vertexOffset + 1, vertexOffset + 2);
        vh.AddTriangle(vertexOffset + 2, vertexOffset + 3, vertexOffset);
    }
}
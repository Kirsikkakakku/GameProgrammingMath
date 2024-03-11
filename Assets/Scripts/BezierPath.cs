using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.Performance.ProfileAnalyzer;

public class BezierPath : MonoBehaviour
{
    [SerializeField]
    Mesh2D shape2D;

    [SerializeField] BezierPoint[] points;
    [SerializeField] GameObject obj;
    [SerializeField] GameObject obj1;



    private Vector3 point;
    private Quaternion rotation;

    private Vector3 anchor1, anchor2, anchor3, anchor4, anchorNext1, anchorNext2, anchorNext3, anchorNext4;

    [Range(3, 255)]
    public int segments = 32;

    public bool closed = false;

    [Range(0, 1)]
    public float t = 0f;

    public float roadScale = 1f;

    private int x = 1;

    private int counter = 0;

    OrientedPoint GetOrientedPoint(float t, Vector3 anc1, Vector3 ctrl1, Vector3 ctrl2, Vector3 anc2)
    {
        Vector3 point1 = Vector3.Lerp(anc1, ctrl1, t);
        Vector3 point2 = Vector3.Lerp(ctrl1, ctrl2, t);
        Vector3 point3 = Vector3.Lerp(ctrl2, anc2, t);

        Vector3 point4 = Vector3.Lerp(point1, point2, t);
        Vector3 point5 = Vector3.Lerp(point2, point3, t);

        Vector3 point6 = Vector3.Lerp(point4, point5, t);

        OrientedPoint op;
        op.Position = point6;
        op.Rotation = Quaternion.LookRotation(point5 - point4); 
        return op;
    }

    private void Start()
    {
        anchor1 = points[0].GetAnchorPoint();
        anchor2 = points[0].GetSecondControlPoint();
        anchor3 = points[1].GetFirstControlPoint();
        anchor4 = points[1].GetAnchorPoint();
    }

    private void Update()
    {
        t += Time.deltaTime;

        GetBezierPointAndRotation();
    }

    /*private void OnValidate()
    {
        anchor1 = points[0].GetAnchorPoint();
        anchor2 = points[0].GetSecondControlPoint();
        anchor3 = points[1].GetFirstControlPoint();
        anchor4 = points[1].GetAnchorPoint();

        OrientedPoint op = GetOrientedPoint(t, anchor1, anchor2, anchor3, anchor4);
        
        obj.transform.position = op.Position;
        obj.transform.rotation = op.Rotation;
    }*/

    private void OnDrawGizmos()
    {
        int n = points.Length;
        int nSegments = n - 1;

        for (int i = 0; i < n - 1; i++)
        {
            Vector3 firstAnchor = points[i].GetAnchorPoint();
            Vector3 secondAnchor = points[i + 1].GetAnchorPoint();

            Vector3 firstControl = points[i].GetSecondControlPoint();
            Vector3 secondControl = points[i + 1].GetFirstControlPoint();

            Handles.DrawBezier(firstAnchor, secondAnchor, firstControl, secondControl, Color.green, null, 3);
        }

        if (closed)
        {
            Vector3 firstAnchor = points[n - 1].GetAnchorPoint();
            Vector3 secondAnchor = points[0].GetAnchorPoint();

            Vector3 firstControl = points[n - 1].GetSecondControlPoint();
            Vector3 secondControl = points[0].GetFirstControlPoint();

            Handles.DrawBezier(firstAnchor, secondAnchor, firstControl, secondControl, Color.green, null, 3);
        }

        for (int segment = 0; segment <= segments-1; segment++)
        {

            float TSegment = (float)segment / segments;
            float TSegmentNext = (float)(segment + 1 )/ segments;

            int segStart = Mathf.FloorToInt(TSegment * n);
            int segStartNext = Mathf.FloorToInt(TSegmentNext * n);

            if (segStart >= n)
            {
                segStart = n - 1;
            }

            if (segStartNext >= n)
            {
                segStartNext = n - 1;
            }

            float tActual = (float)n * TSegment - segStart * 1.0f;
            float tActualNext = n * TSegmentNext - segStartNext * 1.0f;

            anchor1 = points[segStart].GetAnchorPoint();
            anchor2 = points[segStart].GetSecondControlPoint();

            if (segStart + 1 >= n)
            {
                anchor3 = points[0].GetFirstControlPoint();
                anchor4 = points[0].GetAnchorPoint();
            }

            else
            {
                anchor3 = points[segStart + 1].GetFirstControlPoint();
                anchor4 = points[segStart + 1].GetAnchorPoint();
            }

            anchorNext1 = points[segStartNext].GetAnchorPoint();
            anchorNext2 = points[segStartNext].GetSecondControlPoint();

            if (segStartNext + 1 >= n)
            {
                anchorNext3 = points[0].GetFirstControlPoint();
                anchorNext4 = points[0].GetAnchorPoint();
            }

            else
            {
                anchorNext3 = points[segStartNext + 1].GetFirstControlPoint();
                anchorNext4 = points[segStartNext + 1].GetAnchorPoint();
            }

            OrientedPoint op = GetOrientedPoint(tActual, anchor1, anchor2, anchor3, anchor4);
            OrientedPoint opNext = GetOrientedPoint(tActualNext, anchorNext1, anchorNext2, anchorNext3, anchorNext4);

            obj.transform.position = op.Position;
            obj.transform.rotation = op.Rotation;

            obj1.transform.position = opNext.Position;
            obj1.transform.rotation = opNext.Rotation;

            //Handles.PositionHandle(op.Position, op.Rotation;

            Vector3[] verts = shape2D.vertices.Select(v => op.LocalToWorld(v.point * roadScale)).ToArray();
            Vector3[] vertsNext = shape2D.vertices.Select(vNext => opNext.LocalToWorld(vNext.point * roadScale)).ToArray();

            for (int i = 0; i < shape2D.lineIndices.Length; i += 2)
            {
                Vector3 a = verts[shape2D.lineIndices[i]];
                Vector3 b = verts[shape2D.lineIndices[i + 1]];
                Gizmos.color = Color.white;
                Gizmos.DrawLine(a, b);
                Gizmos.color = Color.red;
            }

            List<Vector3> vertices = new List<Vector3>();

            for (int i = 0; i < verts.Length; i += 2)
            {
                vertices.Add(verts[i]);
                vertices.Add(vertsNext[i]);
            }

            for (int i = 0; i < vertices.Count; i += 2)
            {
                Gizmos.DrawLine(vertices[i], vertices[i + 1]);
            }
        }
    }

    private void GetBezierPointAndRotation()
    {
        if (t >= 1)
        {
            t = 0;

            if (x == points.Length - 1)
            {
                anchor1 = points[x].GetAnchorPoint();
                anchor2 = points[x].GetSecondControlPoint();
                anchor3 = points[0].GetFirstControlPoint();
                anchor4 = points[0].GetAnchorPoint();

                x = 0;
            }

            else
            {
                anchor1 = points[x].GetAnchorPoint();
                anchor2 = points[x].GetSecondControlPoint();
                anchor3 = points[x + 1].GetFirstControlPoint();
                anchor4 = points[x + 1].GetAnchorPoint();

                x += 1;
            }
        }

        Vector3 point1 = Vector3.Lerp(anchor1, anchor2, t);
        Vector3 point2 = Vector3.Lerp(anchor2, anchor3, t);
        Vector3 point3 = Vector3.Lerp(anchor3, anchor4, t);

        Vector3 point4 = Vector3.Lerp(point1, point2, t);
        Vector3 point5 = Vector3.Lerp(point2, point3, t);

        Vector3 point6 = Vector3.Lerp(point4, point5, t);

        Quaternion rotation = Quaternion.LookRotation(point5 - point4);

        obj.transform.position = point6;
        obj.transform.rotation = rotation;
    }
}

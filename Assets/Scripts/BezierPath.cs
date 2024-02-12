using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    [SerializeField] BezierPoint[] points;
    [SerializeField] GameObject obj;

    private Vector3 anchor1, anchor2, anchor3, anchor4;

    public bool closed = false;

    private float t = 0f;

    private int x = 1;

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
                anchor2= points[x].GetSecondControlPoint();
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

        obj.transform.position = point6;
    }

    private void OnDrawGizmos()
    {
        int n = points.Length;

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
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [Range(0, 1)]
    public float t = 0f;

    public Transform Anchor1;
    public Transform Anchor2;
    public Transform Anchor3;
    public Transform Anchor4;

    public bool DrawFirstLerp = false;
    public bool DrawSecondLerp = false;
    public bool DrawBezier = false;

    private void OnDrawGizmos()
    {
        Vector3 anchor1 = Anchor1.position;
        Vector3 anchor2 = Anchor2.position;
        Vector3 anchor3 = Anchor3.position;
        Vector3 anchor4 = Anchor4.position;

        Gizmos.color = Color.blue;
        Vector3 point1 = Vector3.Lerp(anchor1, anchor2, t);
        Vector3 point2 = Vector3.Lerp(anchor2, anchor3, t);
        Vector3 point3 = Vector3.Lerp(anchor3, anchor4, t);

        Vector3 point4 = Vector3.Lerp(point1, point2, t);
        Vector3 point5 = Vector3.Lerp(point2, point3, t);

        Vector3 point6 = Vector3.Lerp(point4, point5, t);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(anchor1, 0.3f);
        Gizmos.DrawSphere(anchor2, 0.3f);
        Gizmos.DrawSphere(anchor3, 0.3f);
        Gizmos.DrawSphere(anchor4, 0.3f);

        Handles.color = Color.white;

        if (DrawFirstLerp)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point1, 0.3f);
            Gizmos.DrawSphere(point2, 0.3f);
            Gizmos.DrawSphere(point3, 0.3f);

            Handles.DrawLine(anchor1, anchor2, 0.1f);
            Handles.DrawLine(anchor2, anchor3, 0.1f);
            Handles.DrawLine(anchor3, anchor4, 0.1f);
        }

        if (DrawSecondLerp)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(point4, 0.2f);
            Gizmos.DrawSphere(point5, 0.2f);

            Handles.DrawLine(point2, point3, 0.1f);
            Handles.DrawLine(point1, point2, 0.1f);
        }

        if (DrawBezier)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(point6, 0.3f);

            Handles.DrawLine(point4, point5);
        }

        Handles.DrawBezier(anchor1, anchor4, anchor2, anchor3, Color.green, null, 5f);
    }
}

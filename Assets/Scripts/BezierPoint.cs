using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    //[SerializeField] Transform anchor;
    [SerializeField] Transform[] controls = new Transform[2];

    public bool NeedToUpdateMesh = false;

    public Vector3 GetAnchorPoint() => transform.position;

    public Vector3 GetFirstControlPoint() => controls[0].position;

    public Vector3 GetSecondControlPoint() => controls[1].position; 

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(GetFirstControlPoint(), GetAnchorPoint());
        Gizmos.DrawLine(GetAnchorPoint(), GetSecondControlPoint());
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(GetFirstControlPoint(), 0.1f * HandleUtility.GetHandleSize(GetFirstControlPoint()));
        Gizmos.DrawSphere(GetSecondControlPoint(), 0.1f * HandleUtility.GetHandleSize(GetSecondControlPoint()));
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetAnchorPoint(), 0.1f * HandleUtility.GetHandleSize(GetAnchorPoint()));
    }
}

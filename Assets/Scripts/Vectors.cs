using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vectors : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void DrawVector(Vector3 pos, Vector3 v, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos+v);

        Handles.color = c;

        Vector3 n = v.normalized;
        n = n * 0.35f;

        Handles.ConeHandleCap(1, pos + v - n, Quaternion.LookRotation(v), 0.5f, EventType.Repaint);
    }

    private void OnDrawGizmos()
    {
        DrawVector(Vector3.zero, new Vector3(5, 0, 0), Color.red);

        DrawVector(Vector3.zero, new Vector3(0, 5, 0), Color.green);

        DrawVector(new Vector3(3, 3, 0), new Vector3(4, 3, 0), Color.magenta);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 1);
        DrawVector(transform.position, target.transform.position - transform.position, Color.black);
    }

    
}

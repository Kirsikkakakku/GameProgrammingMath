using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Drawing 
{

    static public void DrawVector(Vector3 pos, Vector3 v, Color c, float thickness=0.0f)
    {
        Handles.color = c;
        Handles.DrawLine(pos, pos + v, thickness);
        // Compute the "rough" endpoint for the cone
        // Normalize the vector (its magnitude becomes 1)
        Vector3 n = v.normalized;
        n = n * 0.35f; // Now the length is 35cm

        Handles.ConeHandleCap(0, pos + v - n, Quaternion.LookRotation(v), 0.5f, EventType.Repaint);

    }

}

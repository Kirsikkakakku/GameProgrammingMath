using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interpolation : MonoBehaviour
{
    [SerializeField] private GameObject goA;
    [SerializeField] private GameObject goB;
    [SerializeField] private GameObject player;
    [SerializeField] private float interpTime = 5.0f;

    private float elapsedTime = 0.0f;

    [Range(0.0f, 1.0f)]
    public float t = 0.0f;

    private void DrawVector(Vector3 pos, Vector3 v, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos + v);

        Handles.color = c;

        Vector3 n = v.normalized;
        n = n * 0.35f;

        Handles.ConeHandleCap(1, pos + v - n, Quaternion.LookRotation(v), 0.5f, EventType.Repaint);
    }

    private void OnDrawGizmos()
    {
        DrawVector(Vector3.zero, goA.transform.position, Color.green);
        DrawVector(Vector3.zero, goB.transform.position, Color.red);
        DrawVector(Vector3.zero, (1 - t) * goA.transform.position, Color.cyan);
        DrawVector((1 - t) * goA.transform.position, t * goB.transform.position, Color.blue);
        SetPlayerPosition(); //Works outside play-mode
    }

    //Just for play-mode
    private void Update()
    {
        //Get elapsed time
        elapsedTime += Time.deltaTime;

        //Interpolate until interpTime
        t = elapsedTime / interpTime;

        //Clamp t to 1
        if (t > 1) t = 1;

        //Interpolation 
        //f(t) = A*(1-t) + B*t

        if (t < 0.5f)
        {
            t = 2 * t * t; // y=2x^2
        }
        else
        {
            t = 1 - 2 * (1 - t) * (1 - t);
        }

        Vector3 pos = (1 - t) * goA.transform.position + t * goB.transform.position;

        //Set player pos
        player.transform.position = pos;
    }

    private void SetPlayerPosition()
    {
        Vector3 pos = (1 - t) * goA.transform.position + t * goB.transform.position;

        //Set player pos
        player.transform.position = pos;
    }
}

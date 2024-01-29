using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject lookTarget;

    [Range(-180, 180)]
    public float AngleThreshold = 30f;
    public float LookDistance = 6f;
    public float Radius = 4f;
    public float Height = 2f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 npcPos = npc.transform.position;
        Vector3 target = lookTarget.transform.position;

        Drawing.DrawVector(Vector3.zero, playerPos - Vector3.zero, Color.white);
        Drawing.DrawVector(Vector3.zero, npcPos - Vector3.zero, Color.white);

        //Radial trigget
        /*if ((playerPos - npcPos).magnitude < Radius)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(npcPos, Vector3.up, Radius);
            Drawing.DrawVector(npcPos, playerPos - npcPos, Color.red);
        }
        else
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(npcPos, Vector3.up, Radius);
            Drawing.DrawVector(npcPos, playerPos - npcPos, Color.green);
        }*/

        Handles.color = Color.green;

        //Handles.DrawWireDisc(npcPos + new Vector3(0, HeightTreshold/2, 0), Vector3.up, Radius);
        //Handles.DrawWireDisc(npcPos - new Vector3(0, HeightTreshold / 2, 0), Vector3.up, Radius);

        //Look-at trigger

        Vector3 lookDir = target - npcPos;

        Vector3 lookVector = Vector3.Normalize(target-npcPos);
        Vector3 playerVector = Vector3.Normalize(playerPos-target);

        float dotProductTreshold = Mathf.Cos(Mathf.Deg2Rad * AngleThreshold);

        float dotProduct = Vector3.Dot(lookVector, playerVector);

        Drawing.DrawVector(npcPos, lookDir, Color.blue);

        if (dotProduct > dotProductTreshold)
        {
            Drawing.DrawVector(npcPos, lookDir, Color.red);
        }
        else
        {
            Drawing.DrawVector(npcPos, lookDir, Color.blue);
        }

        //Wedge trigger

        Vector3 upperMiddle = npcPos + Height / 2 * Vector3.up;
        Vector3 lowerMiddle = npcPos - Height / 2 * Vector3.up;


        Handles.color = Color.white;
        //Handles.DrawWireDisc(upperMiddle, Vector3.up, Radius);
        //Handles.DrawWireDisc(lowerMiddle, Vector3.up, Radius);

        Quaternion posRot = Quaternion.Euler(0f, AngleThreshold, 0f);
        Vector3 rotatedPos = posRot * lookVector;
        Drawing.DrawVector(npcPos, rotatedPos * Radius, Color.magenta);

        Quaternion negRot = Quaternion.Euler(0f, -AngleThreshold, 0f);
        Vector3 rotatedNeg = negRot * lookVector;
        Drawing.DrawVector(npcPos, rotatedNeg * Radius, Color.magenta);

        Gizmos.DrawLine(upperMiddle, upperMiddle + rotatedPos * Radius);
        Gizmos.DrawLine(lowerMiddle, lowerMiddle + rotatedPos * Radius);

        Gizmos.DrawLine(upperMiddle, upperMiddle + rotatedNeg * Radius);
        Gizmos.DrawLine(lowerMiddle, lowerMiddle + rotatedNeg * Radius);

        Gizmos.DrawLine(upperMiddle + rotatedNeg * Radius, lowerMiddle + rotatedNeg * Radius);
        Gizmos.DrawLine(upperMiddle + rotatedPos * Radius, lowerMiddle + rotatedPos * Radius);

        Gizmos.DrawLine(upperMiddle, lowerMiddle);

        Handles.color = Color.white;
        Handles.DrawWireArc(upperMiddle, Vector3.up, lookVector*Radius, AngleThreshold, Radius);
        Handles.DrawWireArc(upperMiddle, Vector3.up, lookVector * Radius, -AngleThreshold, Radius);

        Handles.DrawWireArc(lowerMiddle, Vector3.up, lookVector * Radius, AngleThreshold, Radius);
        Handles.DrawWireArc(lowerMiddle, Vector3.up, lookVector * Radius, -AngleThreshold, Radius);

        if ((playerPos.y < Height / 2 || playerPos.y > Height / 2 * -1) && ((playerPos - npcPos).magnitude < Radius) && dotProduct > dotProductTreshold)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(npcPos, Vector3.up, Radius);
            Drawing.DrawVector(npcPos, playerPos - npcPos, Color.red);
        }
        else
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(npcPos, Vector3.up, Radius);
            Drawing.DrawVector(npcPos, playerPos - npcPos, Color.green);
        }

        //Gizmos.DrawLine(lowerLimit, lookVector * Radius);
    }
}

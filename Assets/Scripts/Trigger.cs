using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject lookAt;
    [SerializeField] private GameObject spotLight;
    [SerializeField] private GameObject lamp;

    [Range(-180, 180)]
    public float AngleThreshold = 30f;
    public float Radius = 4f;
    public float Height = 2f;

    private void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 npcPos = npc.transform.position;
        Vector3 lookingAt = lookAt.transform.position;

        //Radial trigger
        if ((playerPos - npcPos).magnitude < Radius)
        {
            spotLight.SetActive(true);
            lamp.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        }
        else
        {
            spotLight.SetActive(false);
            lamp.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 npcPos = npc.transform.position;
        Vector3 lookingAt = lookAt.transform.position;

        Drawing.DrawVector(Vector3.zero, playerPos - Vector3.zero, Color.white);
        Drawing.DrawVector(Vector3.zero, npcPos - Vector3.zero, Color.white);

        //Radial trigger
        if ((playerPos - npcPos).magnitude < Radius)
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

        //Handles.DrawWireDisc(npcPos + new Vector3(0, HeightTreshold/2, 0), Vector3.up, Radius);
        //Handles.DrawWireDisc(npcPos - new Vector3(0, HeightTreshold / 2, 0), Vector3.up, Radius);

        //Look-at trigger

        /*
        Vector3 lookDir = lookingAt - npcPos;

        Vector3 lookVector = Vector3.Normalize(npcPos-lookingAt);
        Vector3 playerVector = Vector3.Normalize(npcPos-playerPos);

        float dotProductTreshold = Mathf.Cos(Mathf.Deg2Rad * AngleThreshold);

        float dotProduct = Vector3.Dot(lookVector, playerVector);

        bool isLooking = dotProduct > dotProductTreshold;

        Drawing.DrawVector(npcPos, lookDir, Color.blue);

        if (isLooking)
        {
            Drawing.DrawVector(npcPos, lookDir, Color.red);
        }
        else
        {
            Drawing.DrawVector(npcPos, lookDir, Color.blue);
        }
        
        //Wedge trigger

        //Upper and lower height limit center points
        Vector3 upperMiddle = npcPos + Height / 2 * Vector3.up;
        Vector3 lowerMiddle = npcPos - Height / 2 * Vector3.up;

        Quaternion posRot = Quaternion.Euler(0f, AngleThreshold, 0f);
        Vector3 rotatedPos = posRot * lookDir.normalized;
        Drawing.DrawVector(npcPos, rotatedPos * Radius, Color.magenta);

        Quaternion negRot = Quaternion.Euler(0f, -AngleThreshold, 0f);
        Vector3 rotatedNeg = negRot * lookDir.normalized;
        Drawing.DrawVector(npcPos, rotatedNeg * Radius, Color.magenta);

        
        //Check if player height is within npc height-limit
        if (playerPos.y < Height/2 && playerPos.y > -Height / 2)
        {
            //Origin for the radial trigger at player height 
            Vector3 radialOrigin = new Vector3(npcPos.x, playerPos.y, npcPos.z);

            //Radial trigger
            bool withinRadius = (playerPos - radialOrigin).magnitude < Radius;

            //If player is within both the radial trigger and the look-at trigger, player is within the wedge
            if (isLooking && withinRadius)
            {
                Drawing.DrawVector(npcPos, playerPos - npcPos, Color.red);
                //Set wedge color
                Handles.color = Color.red;
                Gizmos.color = Color.red;
            }
            else
            {
                Drawing.DrawVector(npcPos, playerPos - npcPos, Color.green);
                //Set wedge color
                Handles.color = Color.white;
                Handles.color = Color.white;
            }
        }

        else
        {
            Drawing.DrawVector(npcPos, playerPos - npcPos, Color.green);
            //Set wedge color
            Handles.color = Color.white;
            Gizmos.color = Color.white;
        }
        

        //Draw the wedge
        Gizmos.DrawLine(upperMiddle, upperMiddle + rotatedPos * Radius);
        Gizmos.DrawLine(lowerMiddle, lowerMiddle + rotatedPos * Radius);

        Gizmos.DrawLine(upperMiddle, upperMiddle + rotatedNeg * Radius);
        Gizmos.DrawLine(lowerMiddle, lowerMiddle + rotatedNeg * Radius);

        Gizmos.DrawLine(upperMiddle + rotatedNeg * Radius, lowerMiddle + rotatedNeg * Radius);
        Gizmos.DrawLine(upperMiddle + rotatedPos * Radius, lowerMiddle + rotatedPos * Radius);

        Gizmos.DrawLine(upperMiddle, lowerMiddle);

        Handles.DrawWireArc(upperMiddle, Vector3.up, lookDir.normalized * Radius, AngleThreshold, Radius);
        Handles.DrawWireArc(upperMiddle, Vector3.up, lookDir.normalized * Radius, -AngleThreshold, Radius);

        Handles.DrawWireArc(lowerMiddle, Vector3.up, lookDir.normalized * Radius, AngleThreshold, Radius);
        Handles.DrawWireArc(lowerMiddle, Vector3.up, lookDir.normalized * Radius, -AngleThreshold, Radius);*/
    }
}

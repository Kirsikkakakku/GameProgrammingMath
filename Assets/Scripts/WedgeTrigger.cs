using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WedgeTrigger : MonoBehaviour
{
    private EasingFunction.Function rotationEasingFunction;
    public EasingFunction.Ease rotationEaseType;

    [SerializeField] private GameObject car;
    [SerializeField] private GameObject triggerSource;
    [SerializeField] private GameObject lookAt;
    [SerializeField] private GameObject camera;  

    [Range(-180, 180)]
    public float AngleThreshold = 30f;
    public float Radius = 4f;
    public float Height = 2f;
    public float rotationTime = 2f;

    private Vector3 originalRot;
    private Vector3 fromRotation;
    private float elapsedTime = 0f;
    private bool rotating = false;
    private bool carSeen = false;

    private void Awake()
    {
        originalRot = camera.transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (rotating)
        {
            RotateBack();
            return;
        }

        Vector3 carPos = car.transform.position;
        Vector3 triggerSrc = triggerSource.transform.position;
        Vector3 lookingAt = lookAt.transform.position;

        Vector3 lookVector = Vector3.Normalize(triggerSrc - lookingAt);
        Vector3 playerVector = Vector3.Normalize(triggerSrc - carPos);

        float dotProductTreshold = Mathf.Cos(Mathf.Deg2Rad * AngleThreshold);

        float dotProduct = Vector3.Dot(lookVector, playerVector);

        bool isLooking = dotProduct > dotProductTreshold;

        //Check if player height is within npc height-limit
        if (carPos.y < triggerSrc.y + (Height / 2) && carPos.y > triggerSrc.y - (Height / 2))
        {
            //Origin for the radial trigger at player height 
            Vector3 radialOrigin = new Vector3(triggerSrc.x, carPos.y, triggerSrc.z);

            //Radial trigger
            bool withinRadius = (carPos - radialOrigin).magnitude < Radius;

            //If player is within both the radial trigger and the look-at trigger, player is within the wedge
            if (isLooking && withinRadius)
            {
                Quaternion lookRotation = Quaternion.LookRotation(carPos - camera.transform.position, Vector3.up);
                camera.transform.eulerAngles = new Vector3(0, lookRotation.eulerAngles.y, 0);
                carSeen = true;
            }

            else
            {
                if (carSeen)
                {
                    carSeen = false;
                    rotating = true;
                    fromRotation = camera.transform.localRotation.eulerAngles;
                    elapsedTime = 0f;
                    RotateBack();
                }
            }
        }

        else
        {
            if (carSeen)
            {
                carSeen = false;
                rotating = true;
                fromRotation = camera.transform.localRotation.eulerAngles;
                elapsedTime = 0f;
                RotateBack();
            }
        }
    }

    private void RotateBack()
    {
        rotationEasingFunction = EasingFunction.GetEasingFunction(rotationEaseType);

        if (elapsedTime >= rotationTime)
        {
            rotating = false;
        }

        else
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / rotationTime;

            if (fromRotation.y > 180) originalRot.y = 360;

            float rotValue = rotationEasingFunction(fromRotation.y, originalRot.y, t);
            camera.transform.localEulerAngles = new Vector3(0, rotValue, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 carPos = car.transform.position;
        Vector3 triggerSrc = triggerSource.transform.position;
        Vector3 lookingAt = lookAt.transform.position;

        //Look-at trigger
        Vector3 lookDir = lookingAt - triggerSrc;

        Vector3 lookVector = Vector3.Normalize(triggerSrc-lookingAt);
        Vector3 playerVector = Vector3.Normalize(triggerSrc-carPos);

        float dotProductTreshold = Mathf.Cos(Mathf.Deg2Rad * AngleThreshold);

        float dotProduct = Vector3.Dot(lookVector, playerVector);

        bool isLooking = dotProduct > dotProductTreshold;
        
        //Wedge trigger

        //Upper and lower height limit center points
        Vector3 upperMiddle = triggerSrc + Height / 2 * Vector3.up;
        Vector3 lowerMiddle = triggerSrc - Height / 2 * Vector3.up;

        Quaternion posRot = Quaternion.Euler(0f, AngleThreshold, 0f);
        Vector3 rotatedPos = posRot * lookDir.normalized;
        //Drawing.DrawVector(triggerSrc, rotatedPos * Radius, Color.magenta);

        Quaternion negRot = Quaternion.Euler(0f, -AngleThreshold, 0f);
        Vector3 rotatedNeg = negRot * lookDir.normalized;
        //Drawing.DrawVector(triggerSrc, rotatedNeg * Radius, Color.magenta);

        
        //Check if player height is within npc height-limit
        if (carPos.y < triggerSrc.y + (Height / 2) && carPos.y > triggerSrc.y - (Height / 2))
        {
            //Origin for the radial trigger at player height 
            Vector3 radialOrigin = new Vector3(triggerSrc.x, carPos.y, triggerSrc.z);

            //Radial trigger
            bool withinRadius = (carPos - radialOrigin).magnitude < Radius;

            //If player is within both the radial trigger and the look-at trigger, player is within the wedge
            if (isLooking && withinRadius)
            {
                //Drawing.DrawVector(triggerSrc, carPos - triggerSrc, Color.red);
                //Set wedge color
                //Drawing.DrawVector(triggerSrc, lookDir, Color.red);
                Handles.color = Color.red;
                Gizmos.color = Color.red;
            }
            else
            {
                //Drawing.DrawVector(triggerSrc, carPos - triggerSrc, Color.green);
                //Set wedge color
                //Drawing.DrawVector(triggerSrc, lookDir, Color.blue);
                Handles.color = Color.white;
                Handles.color = Color.white;
            }
        }

        else
        {
            //Drawing.DrawVector(triggerSrc, carPos - triggerSrc, Color.green);
            //Set wedge color
            //Drawing.DrawVector(triggerSrc, lookDir, Color.blue);
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
        Handles.DrawWireArc(lowerMiddle, Vector3.up, lookDir.normalized * Radius, -AngleThreshold, Radius);
    }   
}

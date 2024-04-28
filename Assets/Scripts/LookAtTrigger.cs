using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject triggerSource;
    [SerializeField] private GameObject lookAt;
    [SerializeField] private Animator animator;

    [Range(-180, 180)]
    public float AngleThreshold = 30f;
    public float Radius = 8f;
    public float Height = 2f;


    private void Update()
    {
        Vector3 carPos = car.transform.position;
        Vector3 triggerSrc = triggerSource.transform.position;
        Vector3 lookingAt = lookAt.transform.position;

        //Look-at trigger

        
        Vector3 lookDir = lookingAt - triggerSrc;

        Vector3 lookVector = Vector3.Normalize(triggerSrc-lookingAt);
        Vector3 playerVector = Vector3.Normalize(triggerSrc - carPos);

        float dotProductTreshold = Mathf.Cos(Mathf.Deg2Rad * AngleThreshold);

        float dotProduct = Vector3.Dot(lookVector, playerVector);

        bool isLooking = ((dotProduct > dotProductTreshold) && ((carPos - triggerSrc).magnitude < Radius));

        if (isLooking)
        {
            animator.SetBool("Bounce", true);
        }
        else
        {
            animator.SetBool("Bounce", false);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 carPos = car.transform.position;
        Vector3 triggerSrc = triggerSource.transform.position;
        Vector3 lookingAt = lookAt.transform.position;

        //Drawing.DrawVector(Vector3.zero, carPos - Vector3.zero, Color.white);
        //Drawing.DrawVector(Vector3.zero, triggerSrc - Vector3.zero, Color.white);

        //Look-at trigger

        
        Vector3 lookDir = lookingAt - triggerSrc;

        Vector3 lookVector = Vector3.Normalize(triggerSrc-lookingAt);
        Vector3 playerVector = Vector3.Normalize(triggerSrc-carPos);

        float dotProductTreshold = Mathf.Cos(Mathf.Deg2Rad * AngleThreshold);

        float dotProduct = Vector3.Dot(lookVector, playerVector);

        bool isLooking = ((dotProduct > dotProductTreshold) && ((carPos - triggerSrc).magnitude < Radius));

        Drawing.DrawVector(triggerSrc, lookDir, Color.blue);

        if (isLooking)
        {
            Drawing.DrawVector(triggerSrc, lookDir, Color.red);
        }
        else
        {
            Drawing.DrawVector(triggerSrc, lookDir, Color.blue);
        }
    }
}

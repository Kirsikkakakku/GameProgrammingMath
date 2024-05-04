using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject triggerSource;
    [SerializeField] private GameObject spotLight;
    [SerializeField] private GameObject lamp;

    public float Radius = 4f;

    private void Update()
    {
        Vector3 carPos = car.transform.position;
        Vector3 npcPos = triggerSource.transform.position;

        if ((carPos - npcPos).magnitude < Radius)
        {
            spotLight.SetActive(true);
            lamp.GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", new Color(192, 137, 46));
        }
        else
        {
            spotLight.SetActive(false);
            lamp.GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", Color.black);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 carPos = car.transform.position;
        Vector3 triggerSourcePos = triggerSource.transform.position;

        //Drawing.DrawVector(Vector3.zero, carPos - Vector3.zero, Color.white);
        //Drawing.DrawVector(Vector3.zero, triggerSourcePos - Vector3.zero, Color.white);

        if ((carPos - triggerSourcePos).magnitude < Radius)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(triggerSourcePos, Vector3.up, Radius);
            //Drawing.DrawVector(triggerSourcePos, carPos - triggerSourcePos, Color.red);
        }
        else
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(triggerSourcePos, Vector3.up, Radius);
            //Drawing.DrawVector(triggerSourcePos, carPos - triggerSourcePos, Color.green);
        }
    }
}

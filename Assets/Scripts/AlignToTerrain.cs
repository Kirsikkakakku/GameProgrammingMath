using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlignToTerrain : MonoBehaviour
{
    [SerializeField] private GameObject car;

    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        bool hitSomething = Physics.Raycast(ray, out hit);

        if (!hitSomething) return;

        Handles.color = Color.magenta;
        Handles.DrawLine(transform.position, hit.point, 3f);

        Handles.color = Color.green;
        Handles.DrawLine(hit.point, hit.point + hit.normal * 0.5f, 3f);

        Vector3 right = Vector3.Cross(hit.normal, transform.forward);
        Handles.color = Color.red;
        Handles.DrawLine(hit.point, hit.point + right * 0.5f, 3f);
        //transform.up = hit.normal;

        Vector3 forward = Vector3.Cross(hit.normal, -right);
        Handles.color = Color.blue;
        Handles.DrawLine(hit.point, hit.point + forward * 0.5f , 3f);

        if (car != null)
        {
            car.transform.position = hit.point;
            Quaternion rotation = Quaternion.LookRotation(forward, hit.normal);
            car.transform.rotation = rotation;
        }
    }
}

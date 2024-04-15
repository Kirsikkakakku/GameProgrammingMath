using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BouncingRay : MonoBehaviour
{
    [Range(1, 1000)]
    public int Bounces = 100;

    [SerializeField] private GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        /*
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, hit.point);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hit.point, hit.normal + hit.point);

        Vector3 reflect = transform.forward - 2 * Vector3.Dot(transform.forward, hit.normal) * hit.normal;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hit.point, (reflect + hit.point) * 10);
        */

        CalculateBounces();
        //AlignCar();
    }

    private void CalculateBounces()
    {
        RaycastHit hit;

        Vector3 rayDirection = transform.forward;
        Vector3 rayStart = transform.position;

        for (int i = 0; i < Bounces; i++)
        {
            Ray ray = new Ray(rayStart, rayDirection);
            Physics.Raycast(ray, out hit);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(rayStart, hit.point);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(hit.point, hit.normal * 0.5f + hit.point);

            rayDirection = rayDirection - 2 * Vector3.Dot(rayDirection, hit.normal) * hit.normal;
            rayStart = hit.point;
        }
    }

    private void AlignCar()
    {
        RaycastHit hit;
        Ray ray = new Ray(car.transform.position, -car.transform.up);
        Physics.Raycast(ray, out hit);
        car.transform.up = hit.normal;
    }
}

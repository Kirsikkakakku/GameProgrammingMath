using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EasingFunction;

public class EasingAnimation : MonoBehaviour
{

    private EasingFunction.Function easingFunction;
    public EasingFunction.Ease easeType;

    Vector3 target = new Vector3(0, 0, 2);

    private float cooldown = 0.5f;
    private float timeSinceLastShot = 0f;

    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        EasingFunction.Function func = GetEasingFunction(easeType);

        if (Vector3.Distance(transform.position, target) <= 0.1f)
        {
            transform.position = new Vector3(0, 0, 0);
            time = 0f;
        }

        
        float value = func(0, target.z, time/3);

        time += Time.deltaTime;
        
        //EasingFunction.Function derivativeFunc = GetEasingFunctionDerivative(ease);
        
        //float derivativeValue = derivativeFunc(0, 0.01f, 0.01f);


        transform.position = new Vector3(0, 0, value);

        //easingFunction = 

        /*if (Input.GetKeyDown(KeyCode.Mouse0) && timeSinceLastShot >= cooldown)
        {
            Shoot();
        }*/
    }

    private void Shoot()
    {
        timeSinceLastShot = 0f;

        //transform.position = new Vector
    }

    private void Recoil()
    {

    }
}

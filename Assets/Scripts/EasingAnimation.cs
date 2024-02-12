using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingAnimation : MonoBehaviour
{
    private EasingFunction.Function moveEasingFunction;
    private EasingFunction.Function rotationEasingFunction;
    public EasingFunction.Ease moveEaseType;
    public EasingFunction.Ease rotationEaseType;
    //Recoil duration
    public float recoilTime = 0.2f;
    //How much gun moves backwards with each shot
    public float recoilMoveAmount = 0.2f;
    //How much gun rotates with each shot
    public float recoilRotationAmount = 10f;

    private Vector3 originalPos;
    private Vector3 originalRot;
    private Vector3 moveTarget;
    private Vector3 rotTarget;
    private float cooldown = 0.5f;
    private float timeSinceLastShot = 0f;

    private float elapsedTime = 0f;
    private bool recoiling = false;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation.eulerAngles;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (recoiling)
        {
            Recoil();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && timeSinceLastShot >= cooldown && recoiling == false)
        {
            recoiling = true;
            //Set new origin and rotation
            originalPos = transform.position;
            originalRot = transform.rotation.eulerAngles;
            Debug.Log(originalRot);
            moveTarget = new Vector3(0, 0, transform.position.z - recoilMoveAmount);
            rotTarget = new Vector3(transform.rotation.eulerAngles.x - recoilRotationAmount, 0, 0);
            Debug.Log(rotTarget);
            elapsedTime = 0f;
            Recoil();
        }

    }

    private void Recoil()
    {
        moveEasingFunction = EasingFunction.GetEasingFunction(moveEaseType);
        rotationEasingFunction = EasingFunction.GetEasingFunction(rotationEaseType);

        if (elapsedTime >= recoilTime)
        {
            recoiling = false;
        }

        else
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / recoilTime;

            float moveValue = moveEasingFunction(originalPos.z, moveTarget.z, t);
            float rotValue = rotationEasingFunction(originalRot.x, rotTarget.x, t);

            transform.position = new Vector3(0, 0, moveValue);
            transform.eulerAngles = new Vector3(rotValue, 0, 0);
        }
    }
}

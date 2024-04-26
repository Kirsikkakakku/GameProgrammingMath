using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    [SerializeField] private GameObject spotLight;

    public void SetLight(bool lightOn)
    {
        spotLight.SetActive(lightOn);
    }
}

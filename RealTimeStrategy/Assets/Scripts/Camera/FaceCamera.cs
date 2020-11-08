using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCamTf;

    private void Start()
    {
        mainCamTf = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamTf.rotation * Vector3.forward,
            mainCamTf.rotation * Vector3.up);
    }
}

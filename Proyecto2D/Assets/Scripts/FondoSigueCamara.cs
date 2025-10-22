using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoSigeCamara : MonoBehaviour
{
    public Transform cameraTransform;

    void LateUpdate()
    {
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
    }
}

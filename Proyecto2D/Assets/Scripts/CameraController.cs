using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;

    void Start()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 5, transform.position.z);
    }

    void Update()
    {
        
    }
}

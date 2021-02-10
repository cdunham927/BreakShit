using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform[] cameraPositions;
    public int curPos = 0;
    public float lerpSpd;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraPositions[curPos].position, Time.deltaTime * lerpSpd);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -5);
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Follow the target position with an offset
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
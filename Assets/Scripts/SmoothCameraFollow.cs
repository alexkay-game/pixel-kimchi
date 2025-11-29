using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] float damping;
    [SerializeField] Transform target;

    Vector3 vel = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
    }
}

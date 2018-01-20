using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow: MonoBehaviour { 
    public Transform followee;
    private Vector2 m_Offset;
    public float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        // get relative position of camera from player/sphere
        m_Offset = new Vector3(transform.position.x - followee.position.x, transform.position.y - followee.position.y, transform.position.z - followee.position.z);
    }

    // Use lateupdate to ensure positioning of sphere is complete
    void LateUpdate () {
        // Move to new poition
        Vector3 targetPosition = new Vector3(followee.position.x - m_Offset.x, followee.position.y - m_Offset.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}
}

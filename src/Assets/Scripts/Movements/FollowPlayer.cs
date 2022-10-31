using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Vector3 m_offset;

    private void Start()
    {
        // transform.rotation = new Quaternion(m_rotationOffset.x, m_rotationOffset.y, m_rotationOffset.z, m_rotationOffset.w);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = m_playerTransform.position + m_offset;
        transform.position = targetPosition;
    }

    public void SetCameraPosition(Vector3 newOffset, Quaternion newRotation)
    {
        m_offset = newOffset;
        transform.rotation = newRotation;
    }
}

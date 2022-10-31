using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitching : MonoBehaviour
{
    [SerializeField] private GameObject[] m_posCamera;
    private int m_idCamera;

    private FollowPlayer m_cameraFollow;

    private void Start()
    {
        m_cameraFollow = GetComponent<FollowPlayer>();
        m_idCamera = 0;
    }

    public void SwitchCamera()
    {
        if (m_idCamera == 0)
        {
            m_idCamera = 1;
        }
        else
        {
            m_idCamera = 0;
        }

        m_cameraFollow.SetCameraPosition(m_posCamera[m_idCamera].transform.localPosition, m_posCamera[m_idCamera].transform.localRotation);
    }
}

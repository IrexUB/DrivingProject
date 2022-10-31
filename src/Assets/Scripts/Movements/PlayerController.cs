using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerControllerEvent(Vector3 leadingPlayerPosition, Vector3 lastPlayerPosition);
    public static event PlayerControllerEvent PlayerMove;

    [SerializeField] private PlayerConfigSO m_playerConfig;
    private float m_horizontalInput;
    private bool m_areControlsDisabled;

    private Rigidbody m_rb;
    [SerializeField] private float m_minSpeed;
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_currentSpeed;
    [SerializeField] private float m_turnSpeed;

    [SerializeField] private AnimationCurve m_accelerationCurve;
    private bool m_isAccelerating;
    private float m_timeElapsed;


    [SerializeField] private GameObject m_camera;

    public float MinSpeed
    {
        get { return m_minSpeed; }
        set
        {
            if (value > 0)
            {
                m_minSpeed = value;
            }
        }
    }

    public float MaxSpeed
    {
        get { return m_maxSpeed; }
        set
        {
            if (value > 0)
            {
                m_maxSpeed = value;
            }
        }
    }

    public uint PlayerId
    {
        get { return m_playerConfig.m_playerId;  }
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_horizontalInput = Input.GetAxis(m_playerConfig.m_playerAxisName);

        if (Input.GetKeyDown(m_playerConfig.m_playerAccelerationKey))
            m_isAccelerating = true;
        else if (Input.GetKeyUp(m_playerConfig.m_playerAccelerationKey))
            m_isAccelerating = false;

        if (m_isAccelerating)
            m_timeElapsed += Time.deltaTime;
        else
            m_timeElapsed -= Time.deltaTime;

        if (m_timeElapsed < 0)
            m_timeElapsed = 0f;
        m_timeElapsed = Mathf.Min(m_timeElapsed, 1.0f);

        m_currentSpeed = Mathf.Max(m_maxSpeed * m_accelerationCurve.Evaluate(m_timeElapsed), m_minSpeed);

        var leadingPlayer = GameManager.instance.m_leadingPlayer;
        var lastPlayer = GameManager.instance.m_lastPlayer;

        if (leadingPlayer != null && lastPlayer != null) 
            PlayerMove?.Invoke(leadingPlayer.transform.position, lastPlayer.transform.position);

        if (Input.GetKeyDown(m_playerConfig.m_playerCameraSwitchKey))
        {
            m_camera.GetComponent<CameraSwitching>().SwitchCamera();
        }
    }

    private void FixedUpdate()
    {
        if (!m_areControlsDisabled)
        {
            if (GetComponent<PlayerCollisionManager>().IsPlayerGrounded())
            {
                Quaternion rotation = Quaternion.Euler(Vector3.up * m_turnSpeed * m_horizontalInput * Time.fixedDeltaTime);
                m_rb.MoveRotation(m_rb.rotation * rotation);
            }
            m_rb.MovePosition(transform.position + transform.forward * m_currentSpeed * Time.fixedDeltaTime);
        } else
        {
            m_currentSpeed = 0f;
        }
    }

    public void DisableControl(bool state)
    {
        m_areControlsDisabled = state;
    }
}

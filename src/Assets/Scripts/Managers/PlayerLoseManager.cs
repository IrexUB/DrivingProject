using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoseManager : MonoBehaviour
{   
    public delegate void PlayerLoseManagerEvent(uint playerId);
    public static event PlayerLoseManagerEvent PlayerLose;

    private Rigidbody m_rb;
    private bool m_isFalling;
    private uint m_playerId;

    // Start is called before the first frame update
    void Start()
    {
        m_playerId = GetComponent<PlayerController>().PlayerId;
        m_rb = GetComponent<Rigidbody>();
        m_isFalling = false;
    }

    // Update is called once per frame
    void Update()
    {
        // On vérifie si le joueur est renversé sur le côté ou non.
        if (Mathf.Abs(transform.localRotation.w) <= 0.85)
        {
            PlayerLose?.Invoke(m_playerId);
        }

        if (m_rb.position.y < -5.0f)
        {
            m_isFalling = true;

            if (m_isFalling)
            {
                PlayerLose?.Invoke(m_playerId);
            }
        }
        else
        {
            m_isFalling = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    public delegate void CollidingEvent(GameObject chunk);
    public static event CollidingEvent CollidingWithObstacleEvent;

    [SerializeField] private bool m_isPlayerGrounded;

    private void Start()
    {
        m_isPlayerGrounded = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            CollidingWithObstacleEvent?.Invoke(collision.gameObject);
        }
    }


    private void FixedUpdate()
    {
        // On utilise un overlap sphere au lieu d'un raycast pour gérer le cas où si le joueur est au bord de la route, il puisse malgré tout continuer à contrôler sa voiture.
        var colliders = Physics.OverlapSphere(transform.position, 1.5f, LayerMask.GetMask("Ground"));
        if (colliders.Length > 0)
        {
            m_isPlayerGrounded = true;
        } else
        {
            m_isPlayerGrounded = false;
        }
    }

    public bool IsPlayerGrounded()
    {
        return m_isPlayerGrounded;
    }
}

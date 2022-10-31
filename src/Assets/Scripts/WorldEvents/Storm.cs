using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    private float m_stormProgressionSpeed;
    void Start()
    {
        m_stormProgressionSpeed = GameManager.instance.m_lastPlayer.GetComponent<PlayerController>().MinSpeed / 2;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var collidingPlayer = collision.gameObject;

            var healthSystem = collidingPlayer.GetComponent<PlayerHealthSystem>();
            if (healthSystem)
            {
                healthSystem.KillPlayer();
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, m_stormProgressionSpeed * Time.fixedDeltaTime);
    }

    public void IncreaseStormSpeed(float percentage)
    {
        m_stormProgressionSpeed *= (1 + (percentage / 100));
    }
}

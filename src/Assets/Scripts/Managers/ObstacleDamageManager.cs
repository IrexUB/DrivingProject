using System.Collections;
using UnityEngine;


public class ObstacleDamageManager : MonoBehaviour
{
    [SerializeField] private float m_damageToDeal;
    [SerializeField] private ScriptableEffect m_speedMalus;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var collidingPlayer = collision.gameObject;

            var healthSystem = collidingPlayer.GetComponent<PlayerHealthSystem>();
            if (healthSystem)
            {
                healthSystem.TakeDamage(m_damageToDeal);
            }

            collidingPlayer.GetComponent<EffectableEntity>().AddEffect(m_speedMalus.InitializeBuff(collidingPlayer));
        }
    }
}
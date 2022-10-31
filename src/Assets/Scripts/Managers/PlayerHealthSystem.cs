using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private float m_maxHealth;
    [SerializeField] private float m_currentHealth;

    private uint m_playerId;

    public delegate void PlayerHealthEvent(uint playerId, float currentHealth = 0);
    public static event PlayerHealthEvent PlayerTakeDamage;
    public static event PlayerHealthEvent PlayerDeath;

    public void Start()
    {
        m_currentHealth = m_maxHealth;
        m_playerId = GetComponent<PlayerController>().PlayerId;
    }

    public void KillPlayer()
    {
        PlayerDeath?.Invoke(m_playerId);
    }

    public float GetRemainingLifeRatio()
    {
        return (m_currentHealth / m_maxHealth);
    }

    public void TakeDamage(float damage)
    {
        m_currentHealth -= damage;
        PlayerTakeDamage?.Invoke(m_playerId, GetRemainingLifeRatio());

        if (m_currentHealth <= 0)
        {
            PlayerDeath?.Invoke(m_playerId);
        }
    }
        
}

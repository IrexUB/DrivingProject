using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip m_crashSound;
    private AudioSource m_source;

    // Start is called before the first frame update
    void Start()
    {
        m_source = GetComponent<AudioSource>();
        PlayerCollisionManager.CollidingWithObstacleEvent += OnPlayerCollideWithObstacle;
    }

    private void OnDestroy()
    {
        PlayerCollisionManager.CollidingWithObstacleEvent -= OnPlayerCollideWithObstacle;
    }

    void OnPlayerCollideWithObstacle(GameObject obstacle)
    {
        m_source.clip = m_crashSound;
        if (!m_source.isPlaying)
        {
            m_source.Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    [SerializeField] private GameObject m_stormPrefab;
    [SerializeField] private float m_stormSpawnPositionOffset;

    private GameObject m_storm;
    [SerializeField] private float m_stormSpeedIncreasePercentage;
    private bool m_isStormHere;

    public delegate void WorldEventManagerEvent();
    public static event WorldEventManagerEvent EnableEventsSpawning;

    private void Start()
    {
        m_isStormHere = false;

        GameManager.WorldEventDistanceReached += SpawnStorm;
        GameManager.WorldEventDistanceReached += EmitEventsSpawningEvent;
        GameManager.IncreaseDifficulty += IncreaseStormSpeed;
    }

    private void OnDestroy()
    {
        GameManager.WorldEventDistanceReached -= SpawnStorm;
        GameManager.WorldEventDistanceReached -= EmitEventsSpawningEvent;
        GameManager.IncreaseDifficulty -= IncreaseStormSpeed;
    }

    private void SpawnStorm()
    {
        var lastPlayerZPosition = GameManager.instance.m_lastPlayer.transform.position.z;

        m_storm = Instantiate(m_stormPrefab, new Vector3(0, 0, lastPlayerZPosition - m_stormSpawnPositionOffset), Quaternion.identity);
        m_isStormHere = true;
    }

    private void EmitEventsSpawningEvent()
    {
        EnableEventsSpawning?.Invoke();
    }

    private void IncreaseStormSpeed()
    {
        if (m_isStormHere)
        {
            m_storm.GetComponent<Storm>().IncreaseStormSpeed(m_stormSpeedIncreasePercentage);
        }
    }
}

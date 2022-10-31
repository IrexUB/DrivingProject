using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public GamemodeSO m_gameModeConf;
    public ChronoRecordsMap m_loadedRecords = new ChronoRecordsMap();

    public float m_recordToBeat;

    public GameObject m_firstPlayer;
    private uint m_firstPlayerId;
    private Vector3 m_firstPlayerPosition;

    public GameObject m_secondPlayer;
    private uint m_secondPlayerId;
    private Vector3 m_secondPlayerPosition;

    public GameObject m_leadingPlayer;
    public GameObject m_lastPlayer;

    // On a besoin de connaître la distance maximale parcourue pour augmenter la difficulté de façon dynamique.
    [SerializeField] private float m_maxTravelledDistance;

    private float m_maxTravelledDistanceFirstPlayer;
    private float m_maxTravelledDistanceSecondPlayer;

    public delegate void GameEventChrono(float distance);
    public static event GameEventChrono OnDistanceIncreaseChrono;

    public delegate void GameEventVersus(uint playerId, float distance);
    public static event GameEventVersus OnDistanceIncreaseVersus;

    public float MaxTravelledDistance
    {
        get { return instance.m_maxTravelledDistance; }
    }

    private bool m_isEvtDistanceReached;

    public delegate void GameManagerEvent();
    public static event GameManagerEvent WorldEventDistanceReached;
    public static event GameManagerEvent IncreaseDifficulty;

    private void Start()
    {
        PlayerLoseManager.PlayerLose += LoadLoserScene;
        TimeManager.OnTimerStop += OnChronoModeLose;
        PlayerHealthSystem.PlayerDeath += LoadLoserScene;
    }

    public void Awake()
    {
        base.Awake();

        instance.m_firstPlayer = null;
        instance.m_secondPlayer = null;
        instance.m_leadingPlayer = null;
        instance.m_lastPlayer = null;
        instance.m_maxTravelledDistance = 0;
        instance.m_maxTravelledDistanceFirstPlayer = 0;
        instance.m_maxTravelledDistanceSecondPlayer = 0;

        instance.m_loadedRecords = ChronoSaveManager.instance.LoadRecords();

        var playerOccurences = GameObject.FindGameObjectsWithTag("Player");

        if (instance.m_gameModeConf != null && instance.m_gameModeConf.m_isVersus && playerOccurences.Length == 2)
        {
            instance.m_firstPlayer = playerOccurences[0];
            instance.m_secondPlayer = playerOccurences[1];

            instance.m_firstPlayerId = playerOccurences[0].GetComponent<PlayerController>().PlayerId;
            instance.m_secondPlayerId = playerOccurences[1].GetComponent<PlayerController>().PlayerId;

        } else if (playerOccurences.Length == 1)
        {
            // Le gameplay est à l'origine adapté, à deux joueurs.
            // Pour le réadapter à 1 joueur, il suffit d'assigner le joueur 1 et le joueur 2 au même joueur.
            instance.m_firstPlayer = playerOccurences[0];
            instance.m_secondPlayer = playerOccurences[0];
        }
    }
    private void OnDestroy()
    {
        base.OnDestroy();

        PlayerLoseManager.PlayerLose -= LoadLoserScene;
        TimeManager.OnTimerStop -= OnChronoModeLose;
    }


    private void Update()
    {
        if (m_firstPlayer != null && m_secondPlayer != null)
        {
            m_firstPlayerPosition = m_firstPlayer.transform.position;
            m_secondPlayerPosition = m_secondPlayer.transform.position;

            if (m_firstPlayerPosition.z > m_secondPlayerPosition.z)
            {
                m_leadingPlayer = m_firstPlayer;
                m_lastPlayer = m_secondPlayer;
            }
            else
            {
                m_leadingPlayer = m_secondPlayer;
                m_lastPlayer = m_firstPlayer;
            } 
        }

        if (m_gameModeConf != null) {
            if (m_leadingPlayer != null)
            {
                m_maxTravelledDistance = Mathf.Max(m_maxTravelledDistance, m_leadingPlayer.transform.position.z);
            }

            if (!m_gameModeConf.m_isVersus)
                OnDistanceIncreaseChrono?.Invoke(GetTravelledDistanceInKm());

            if (m_firstPlayer != null && m_secondPlayer != null)
            {
                m_maxTravelledDistanceFirstPlayer = Mathf.Max(m_maxTravelledDistanceFirstPlayer, m_firstPlayer.transform.position.z);
                m_maxTravelledDistanceSecondPlayer = Mathf.Max(m_maxTravelledDistanceSecondPlayer , m_secondPlayer.transform.position.z);

                OnDistanceIncreaseVersus?.Invoke(m_firstPlayerId, ConvertDistanceInKm(m_maxTravelledDistanceFirstPlayer));
                OnDistanceIncreaseVersus?.Invoke(m_secondPlayerId, ConvertDistanceInKm(m_maxTravelledDistanceSecondPlayer));
            }

            if (m_gameModeConf.m_areEventsOn)
            {
                if (m_maxTravelledDistance >= m_gameModeConf.m_distRqdToStartSpawningEvt && !m_isEvtDistanceReached)
                {
                    // On émet un événement lorsqu'on a atteint la distance requise pour faire apparaître les événements aléatoires
                    // afin que les autres components abonnés puissent être au courant
                    WorldEventDistanceReached?.Invoke();
                    m_isEvtDistanceReached = true;
                }
            }

            if(Mathf.Round(m_maxTravelledDistance) % m_gameModeConf.m_difficultyIncreaseDistanceOffset == 0)
                IncreaseDifficulty?.Invoke();
        }
    }

    public GameObject GetOpponentOf(GameObject player)
    {
        if (player == m_firstPlayer)
        {
            return m_secondPlayer;
        }
        else
        {
            return m_firstPlayer;
        }
    }

    public float GetTravelledDistanceInKm()
    {
        return ConvertDistanceInKm(instance.MaxTravelledDistance);
    }

    private float ConvertDistanceInKm(float distance)
    {
        return (float)Math.Round(distance / 1000, 2);
    }

    public void SetGameMode(GamemodeSO gameMode)
    {
        instance.m_gameModeConf = gameMode;
    }

    public void LoadLoserScene(uint loserId)
    {
        SceneManager.LoadScene("SceneLoser");
        UIManager.instance.DisplayLoserMenu(loserId);
    }

    public void LoadLoserScene(uint loserId, float healthRatio)
    {
        SceneManager.LoadScene("SceneLoser");
        UIManager.instance.DisplayLoserMenu(loserId);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnChronoModeLose()
    {
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            LoadLoserScene(playerController.PlayerId);

            ChronoSaveManager.instance.SaveRecord(new ChronoRecordData(TimeManager.instance.ChosenTimer, GetTravelledDistanceInKm()));
        }
    }
}
 
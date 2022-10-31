using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class UIManager : MonoSingleton<UIManager>
{

    [Header("Main Menu UI")]
    [SerializeField] private GameObject m_gameModeSelectionMenuUI;
    [SerializeField] private GameObject m_versusModeMenuUI;
    [SerializeField] private GameObject m_mainMenuUI;

    [Header("Loser Menu UI")]
    [SerializeField] private GameObject m_loserMenuUI;
    [SerializeField] private Text m_loserText;
    [SerializeField] private Text m_distanceText;

    [Header("Chrono Config UI")]
    [SerializeField] private GameObject m_chronoModeMenuUI;
    [SerializeField] private TMP_Dropdown m_chronoDropdown;

    [Header("Chrono Mode HUD")]
    [SerializeField] private GameObject m_chronoModeHUD;
    [SerializeField] private Text m_recordText;
    [SerializeField] private Text m_remainingTimeText;
    [SerializeField] private Text m_travelledDistanceChrono;

    [Header("Versus Mode HUD")]
    [SerializeField] private GameObject m_versusModeHUD;
    [SerializeField] private Slider m_firstPlayerHealthBar;
    [SerializeField] private Slider m_secondPlayerHealthBar;
    [SerializeField] private Text m_travelledDistanceFirstPlayer;
    [SerializeField] private Text m_travelledDistanceSecondPlayer;

    public void Start()
    {
        TimeManager.OnTimerDecrease += OnChronoRemainingTimeUpdate;

        GameManager.OnDistanceIncreaseChrono += OnTravelledDistanceUpdateChrono;
        GameManager.OnDistanceIncreaseVersus += OnTravelledDistanceUpdateVersus;

        PlayerHealthSystem.PlayerTakeDamage += UpdatePlayerHealthBar;

        DisplayMainMenu();
    }

    private void Awake()
    {
        base.Awake();

        // On reset les valeurs des barres de vies. 
        instance.m_firstPlayerHealthBar.value = 1;
        instance.m_secondPlayerHealthBar.value = 1;
    }


    private void OnDestroy()
    {
        TimeManager.OnTimerDecrease -= OnChronoRemainingTimeUpdate;

        GameManager.OnDistanceIncreaseChrono -= OnTravelledDistanceUpdateChrono;
        GameManager.OnDistanceIncreaseVersus -= OnTravelledDistanceUpdateVersus;

        PlayerHealthSystem.PlayerTakeDamage -= UpdatePlayerHealthBar;
    }

    public void HideUI()
    {
        m_loserMenuUI.SetActive(false);
        m_mainMenuUI.SetActive(false);
        m_chronoModeHUD.SetActive(false);
        m_chronoModeMenuUI.SetActive(false);
        m_versusModeHUD.SetActive(false);
    }

    public void DisplayGameModeSelectionMenu()
    {
        m_gameModeSelectionMenuUI.SetActive(true);
        m_versusModeMenuUI.SetActive(false);
        m_chronoModeMenuUI.SetActive(false);
    }

    public void DisplayVersusModeMenu()
    {
        m_gameModeSelectionMenuUI.SetActive(false);
        m_versusModeMenuUI.SetActive(true);
    }

    public void DisplayChronoModeMenu()
    {
        m_chronoModeMenuUI.SetActive(true);
        m_gameModeSelectionMenuUI.SetActive(false);
        m_versusModeMenuUI.SetActive(false);
    }

    public void DisplayMainMenu()
    {
        HideUI();

        m_mainMenuUI.SetActive(true);
        DisplayGameModeSelectionMenu();
    }

    public void DisplayLoserMenu(uint loserId)
    {
        HideUI();

        m_loserMenuUI.SetActive(true);
        m_loserText.text = "Player " + loserId + " lose !";
        m_distanceText.text = "Maximum distance reached : " + GameManager.instance.GetTravelledDistanceInKm() + " km";
    }

    public void DisplayChronoModeHUD()
    {
        HideUI();
        m_chronoModeHUD.SetActive(true);    
    }

    public void DisplayVersusModeHUD()
    {
        HideUI();
        m_versusModeHUD.SetActive(true);
    }

    private void UpdatePlayerHealthBar(uint playerId, float healthRatio)
    {
        if (playerId == 1)
        {
            m_firstPlayerHealthBar.value = healthRatio;
        } else
        {
            m_secondPlayerHealthBar.value = healthRatio;
        }
    }

    private void OnChronoRemainingTimeUpdate(int remainingTime)
    {
        m_remainingTimeText.text = "Remaining time : " + remainingTime + " seconds";
    }

    private int ConvertChronoDropdownValueToInt()
    {
        var chronoDropdownValues = m_chronoDropdown.GetComponent<TMP_Dropdown>().options;
        var value = int.Parse(chronoDropdownValues[m_chronoDropdown.value].text);

        return value;
    }

    public void OnChronoDropdownValueChanged()
    {
        var chosenChronoTime = ConvertChronoDropdownValueToInt();
        TimeManager.instance.SetTimer(chosenChronoTime);

        var recordsMap = GameManager.instance.m_loadedRecords;
        if (recordsMap != null)
        {
            if (recordsMap.m_records.Exists(record => record.timer == chosenChronoTime))
            {
                ChronoRecordData? recordForSpecifiedTimer = recordsMap.m_records.Single(record => record.timer == chosenChronoTime);
                if (recordForSpecifiedTimer.HasValue)
                {
                    var recordForSpecifiedTimerValue = recordForSpecifiedTimer.Value.max_distance;
                    GameManager.instance.m_recordToBeat = recordForSpecifiedTimerValue;
                    m_recordText.text = "Record : " + recordForSpecifiedTimerValue + " km";
                }
            }
            else
            {
                GameManager.instance.m_recordToBeat = 0;
                m_recordText.text = "Record : " + 0 + " km";
            }
        }
    }

    public void OnTravelledDistanceUpdateChrono(float distance)
    {
        var travelledDistance = GameManager.instance.GetTravelledDistanceInKm();

        m_travelledDistanceChrono.text = "Distance: " + distance + " km";

        if (travelledDistance > GameManager.instance.m_recordToBeat)
        {
            m_recordText.text = "Record : " + distance + " km";
        }
    }

    public void OnTravelledDistanceUpdateVersus(uint playerId, float distance)
    {
        if (playerId == 1)
        {
            m_travelledDistanceFirstPlayer.text = distance + " km";
        } else
        {
            m_travelledDistanceSecondPlayer.text = distance + " km";
        }
    }
}

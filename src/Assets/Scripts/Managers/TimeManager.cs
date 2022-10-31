using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    [SerializeField] private int m_timer;
    private int m_remainingTime;

    public delegate void TimeEvent(int time);
    public static event TimeEvent OnTimerDecrease;

    public delegate void TimeManagerEvent();
    public static event TimeManagerEvent OnTimerStop;

    private void Start()
    {
        PlayerLoseManager.PlayerLose += StopTimer;
    }

    private void OnDestroy()
    {
        PlayerLoseManager.PlayerLose -= StopTimer;
    }

    public int ChosenTimer
    {
        get { return m_timer;  }
    }

    public void SetTimer(int seconds)
    {
        m_timer = seconds;
    }

    public void StartTimer()
    {
        StartCoroutine(DecreaseTimer());
    }

    public void StopTimer(uint optionalPlayerId)
    {
        StopAllCoroutines();
        OnTimerStop?.Invoke();
    }

    private IEnumerator DecreaseTimer()
    {
        for (m_remainingTime = m_timer; m_remainingTime >= 0; m_remainingTime--)
        {
            OnTimerDecrease?.Invoke(m_remainingTime);
            yield return new WaitForSeconds(1);
        }

        OnTimerStop?.Invoke();
    }
}

using System.Collections;
using UnityEngine;

public class TimedSpeedMalus : TimedEffect
{
    private PlayerController m_controller;
    private SpeedMalus m_speedMalus;

    public TimedSpeedMalus(ScriptableEffect buff, GameObject obj) : base(buff, obj)
    {
        m_controller = obj.GetComponent<PlayerController>();
        m_speedMalus = (SpeedMalus)m_effect;
    }

    protected override void ApplyEffect()
    {
        m_controller.MaxSpeed = m_controller.MaxSpeed / (1 + (m_speedMalus.m_debuffPercentage / 100));
    }

    public override void End()
    {
        m_controller.MaxSpeed = m_controller.MaxSpeed * (1 + (m_speedMalus.m_debuffPercentage / 100));
    }
}
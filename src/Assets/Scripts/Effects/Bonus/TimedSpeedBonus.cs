using System.Collections;
using UnityEngine;

public class TimedSpeedBonus : TimedEffect
{
    private PlayerController m_controller;
    private SpeedBonus m_speedBonus;

    public TimedSpeedBonus(ScriptableEffect buff, GameObject obj) : base(buff, obj)
    {
        m_controller = obj.GetComponent<PlayerController>();
        m_speedBonus = (SpeedBonus)m_effect;
    }

    protected override void ApplyEffect()
    {
        m_controller.MaxSpeed = m_controller.MaxSpeed * (1 + (m_speedBonus.m_buffPercentage / 100));
    }

    public override void End()
    {
        m_controller.MaxSpeed = m_controller.MaxSpeed / (1 + (m_speedBonus.m_buffPercentage / 100));

        m_effectStacks = 0;
    }
}
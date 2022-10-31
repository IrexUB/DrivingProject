using System.Collections;
using UnityEngine;

public class TimedSabotageBonus : TimedEffect
{
    private PlayerController m_opponentController;
    private float m_maxSpeedTmp;

    public TimedSabotageBonus(ScriptableEffect buff, GameObject obj) : base(buff, obj)
    {
        m_opponentController = GameManager.instance.GetOpponentOf(obj).GetComponent<PlayerController>();

        m_maxSpeedTmp = m_opponentController.MaxSpeed;
    }

    protected override void ApplyEffect()
    {
        m_opponentController.MaxSpeed = m_opponentController.MinSpeed;
    }

    public override void End()
    {
        m_opponentController.MaxSpeed = m_maxSpeedTmp;
    }
}
using System.Collections;
using UnityEngine;

public class TimedDisableControlBonus : TimedEffect
{
    private PlayerController m_opponentController;

    public TimedDisableControlBonus(ScriptableEffect buff, GameObject obj) : base(buff, obj)
    {
        m_opponentController = GameManager.instance.GetOpponentOf(obj).GetComponent<PlayerController>();
    }

    protected override void ApplyEffect()
    {
        m_opponentController.DisableControl(true);
    }

    public override void End()
    {
        m_opponentController.DisableControl(false);
    }
}
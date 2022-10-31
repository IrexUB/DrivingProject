using System.Collections;
using UnityEngine;

public class TimedPositionSwitchSpecial : TimedEffect
{
    public TimedPositionSwitchSpecial(ScriptableEffect buff, GameObject obj) : base(buff, obj) { ; }

    protected override void ApplyEffect()
    {
        var opponent = GameManager.instance.GetOpponentOf(m_obj);
        var currentPlayerPositionBeforeSwitch = m_obj.transform.position;

        m_obj.transform.position = opponent.transform.position;
        opponent.transform.position = currentPlayerPositionBeforeSwitch;
    }

    public override void End() { ; }
}
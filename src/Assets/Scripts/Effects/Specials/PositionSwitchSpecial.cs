using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Specials/PositionSwitchSpecial", fileName = "PositionSwitchSpecial")]
public class PositionSwitchSpecial : ScriptableEffect
{
    public float m_effectPercentage;

    public override TimedEffect InitializeBuff(GameObject obj)
    {
        return new TimedPositionSwitchSpecial(this, obj);
    }
}

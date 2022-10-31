using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Malus/SpeedMalus", fileName = "SpeedMalus")]
public class SpeedMalus : ScriptableEffect
{
    public float m_debuffPercentage;

    public override TimedEffect InitializeBuff(GameObject obj)
    {
        return new TimedSpeedMalus(this, obj);
    }
}

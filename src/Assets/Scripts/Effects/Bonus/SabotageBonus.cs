using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Bonus/SabotageBonus", fileName = "SabotageBonus")]
public class SabotageBonus : ScriptableEffect
{
    public float m_effectPercentage;

    public override TimedEffect InitializeBuff(GameObject obj)
    {
        return new TimedSabotageBonus(this, obj);
    }
}

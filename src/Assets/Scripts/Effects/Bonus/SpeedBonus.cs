using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Bonus/SpeedBonus", fileName = "SpeedBonus")]
public class SpeedBonus : ScriptableEffect
{
    public float m_buffPercentage;

    public override TimedEffect InitializeBuff(GameObject obj)
    {
        return new TimedSpeedBonus(this, obj);
    }
}

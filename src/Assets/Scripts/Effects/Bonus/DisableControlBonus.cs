using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Bonus/DisableControlBonus", fileName = "DisableControlBonus")]
public class DisableControlBonus : ScriptableEffect
{
    public float m_effectPercentage;

    public override TimedEffect InitializeBuff(GameObject obj)
    {
        return new TimedDisableControlBonus(this, obj);
    }
}

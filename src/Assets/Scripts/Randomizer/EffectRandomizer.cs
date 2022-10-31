using UnityEditor;
using UnityEngine;

public class EffectRandomizer : Randomizer<ScriptableEffect>
{
    public ScriptableEffect PullRandomEffect()
    {
        return m_objectsTypes[ChooseRandomObstacle()].m_objectPrefab;
    }
}
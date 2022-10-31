using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct ObjectProbability<T>
{
    [Range(0.0f, 100f)]
    public float m_probability;

    public T m_objectPrefab;
};

public abstract class Randomizer<T> : MonoBehaviour
{
    [SerializeField] protected ObjectProbability<T>[] m_objectsTypes;

    protected int ChooseRandomObstacle()
    {
        float probabilitySum = 0;
        float numForAdding = 0;
        float r = UnityEngine.Random.value;

        for (var i = 0; i < m_objectsTypes.Length; ++i)
            probabilitySum += m_objectsTypes[i].m_probability;

        for (var i = 0; i < m_objectsTypes.Length; ++i)
        {
            if (m_objectsTypes[i].m_probability / probabilitySum + numForAdding >= r)
            {
                return i;
            }
            else
            {
                numForAdding += m_objectsTypes[i].m_probability / probabilitySum;
            }
        }

        return 0;
    }

}
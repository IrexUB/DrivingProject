using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectRandomizer : Randomizer<GameObject>
{
    [SerializeField] private GameObject m_objectsParent;
    protected List<GameObject> m_objects = new List<GameObject>();

    public void SpawnObject(Vector3 position)
    {
        var randomObstacleType = m_objectsTypes[ChooseRandomObstacle()].m_objectPrefab;

        var newObstacle = Instantiate(randomObstacleType, position, Quaternion.identity);
        newObstacle.transform.parent = m_objectsParent.transform;

        m_objects.Add(newObstacle);
    }

    public void SpawnObject(Vector3 position, Quaternion rotation)
    {
        var randomObstacleType = m_objectsTypes[ChooseRandomObstacle()].m_objectPrefab;

        var newObstacle = Instantiate(randomObstacleType, position, rotation);
        newObstacle.transform.parent = m_objectsParent.transform;

        m_objects.Add(newObstacle);
    }

    public void RemoveObject(GameObject gameObject)
    {
        var objectToRemove = m_objects.Find(currentObject => currentObject == gameObject);
        if (objectToRemove != null)
        {
            Destroy(objectToRemove);
            m_objects.Remove(objectToRemove);
        }
    }

    public void ObjectsCulling(Vector3 playerPosition)
    {
        var objectsToRemove = m_objects.FindAll(currentObject => currentObject.transform.position.z < playerPosition.z);

        if (objectsToRemove.Count > 0)
        {
            foreach (var currentObject in objectsToRemove)
            {
                if (currentObject != null)
                {
                    Destroy(currentObject);
                }
            }
            m_objects.RemoveAll(currentObject => currentObject.transform.position.z < playerPosition.z);
        }

    }

    public int Count()
    {
        return m_objects.Count;
    }

    public GameObject At(int index)
    {
        if (index >= 0 && index < m_objects.Count)
            return m_objects[index];

        Debug.LogError("Attempt to segfault");
        return null;
    }
}
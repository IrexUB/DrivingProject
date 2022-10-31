using UnityEngine;

public struct EffectProbability
{
    [Range(0.0f, 100f)]
    public float m_probability;

    public ScriptableEffect m_objectPrefab;
};

public class EffectBox : MonoBehaviour
{
    public delegate void CullingEvent(GameObject objectToDelete);
    public static event CullingEvent BoxEffectCulling;

    [SerializeField] private ScriptableEffect m_effectSet;

    private void Start()
    {
        m_effectSet = FindObjectOfType<EffectRandomizer>().PullRandomEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject playerGO = other.gameObject;
            playerGO.GetComponent<EffectableEntity>().AddEffect(m_effectSet.InitializeBuff(playerGO));
            BoxEffectCulling?.Invoke(gameObject);
        }
    }
}
using UnityEditor;
using UnityEngine;

public class PositionRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject m_roadChunk;
    [SerializeField] private GameObject m_borderBarrier;

    private float m_borderBarrierWidth;
    private float m_roadChunkWidth;

    private void Start()
    {
        m_roadChunkWidth = m_roadChunk.GetComponent<Renderer>().bounds.size.x;
        m_borderBarrierWidth = m_borderBarrier.GetComponent<Renderer>().bounds.size.z;

        var randomX = Random.Range((-m_roadChunkWidth / 2) + (m_borderBarrierWidth * 2), (m_roadChunkWidth / 2) - (m_borderBarrierWidth * 2));

        transform.position = new Vector3(randomX, transform.position.y, transform.position.z);
    }
}
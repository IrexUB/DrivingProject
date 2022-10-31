using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField] private GameObject m_roadChunk;
    [SerializeField] private GameObject m_borderBarrier;

    private float m_borderBarrierWidth;
    private float m_roadChunkWidth;
    
    private bool m_isTargetReached;
    private Vector3 m_targetPosition;

    private float m_currentCrateSpeed;

    void Start()
    {
        m_roadChunkWidth = m_roadChunk.GetComponent<Renderer>().bounds.size.x;
        m_borderBarrierWidth = m_borderBarrier.GetComponent<Renderer>().bounds.size.z;

        m_isTargetReached = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isTargetReached)
        {
            // On génère une position horizontale aléatoire que l'objet doit atteindre de façon à regénérer une nouvelle position aléatoire, et cela à l'infini.
            var randomX = Random.Range((-m_roadChunkWidth / 2) + (m_borderBarrierWidth * 2), (m_roadChunkWidth / 2) - (m_borderBarrierWidth * 2));

            m_targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);

            m_currentCrateSpeed = Random.Range(5, 10);

            m_isTargetReached = false;
        }

        var newPosition = Vector3.MoveTowards(transform.position, m_targetPosition, m_currentCrateSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void LateUpdate()
    {
        // Si la position de l'objet à atteint la position horizontale aléatoire, on peut en généré une nouvelle.
        if (transform.position == m_targetPosition)
        {
            m_isTargetReached = true;
        }
    }
}

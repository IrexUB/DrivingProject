using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct ObstacleProbability
{
    [Range(0.0f, 100f)]
    public float m_probability;

    public GameObject m_obstaclePrefab;
};

public class CreateRealTimeRoad : MonoBehaviour
{
    [SerializeField] GameObject m_roadChunkPrefab;
    [SerializeField] private int m_obstacleGenerationOffset;

    [SerializeField] private ObjectRandomizer m_obstaclesSpawner;
    [SerializeField] private ObjectRandomizer m_roadChunksSpawner;
    [SerializeField] private ObjectRandomizer m_effectBoxSpawner;

    [Range(0.0f, 10.0f)]
    [SerializeField] private float m_effectBoxSpawnRateInSeconds;
    [SerializeField] private bool m_canSpawnEffectBox;

    private float m_roadChunkHeight;
    private Quaternion m_roadChunkDefaultRot = Quaternion.Euler(0, 90, 0);

    // On utilise cette donnée membre pour faire apparaître à la bonne position dans le monde les différents objets.
    private int m_nChunkGenerated = 0;


    void Start()
    {
        PlayerController.PlayerMove += OnPlayerMove;
        EffectBox.BoxEffectCulling += ManuallyDeleteEffectBox;

        // L'apparition d'événements aléatoires est possible uniquement dans le mode de jeu "Difficile en versus"
        if (GameManager.instance.m_gameModeConf.m_areEventsOn)
        {
            // WorldEventManager.EnableEventsSpawning += EnableRandomEventsSpawing;
        }

        GameManager.IncreaseDifficulty += DecreaseObstacleGenerationOffset;

        m_roadChunkHeight = m_roadChunkPrefab.GetComponent<Renderer>().bounds.size.z;

        GenerateRoadChunks(64);

        if (m_canSpawnEffectBox)
        {
            InvokeRepeating("GenerateEffectBox", 0f, m_effectBoxSpawnRateInSeconds);
        }
    }

    private void OnDestroy()
    {
        PlayerController.PlayerMove -= OnPlayerMove;
        EffectBox.BoxEffectCulling -= ManuallyDeleteEffectBox;
        GameManager.IncreaseDifficulty -= DecreaseObstacleGenerationOffset;
    }

   /* private void EnableRandomEventsSpawing()
    {
        m_canSpawnRandomEventsOnRoad = true;
    }*/

    private void DecreaseObstacleGenerationOffset()
    {
        m_obstacleGenerationOffset--;
    }

    private void OnPlayerMove(Vector3 leadingPlayerPosition, Vector3 lastPlayerPosition)
    {
        // On génére des nouveaux morceaux de route si le joueur en tête se trouve au milieu de la route.
        if (IsPlayerOnCenterOfTheRoad(leadingPlayerPosition))
        {
            GenerateRoadChunks(m_obstacleGenerationOffset * m_obstacleGenerationOffset);
        }

        // Si le joueur à la dernière position se trouve sur le morceau suivant le morceau n°"m_crateGenerationOffset" de la route, on supprime les m_crateGenerationOffset précédents morceaux
        // tout en prenant en compte la suppression de l'obstacle
        if (IsPlayerOnChunk(m_obstacleGenerationOffset, lastPlayerPosition))
        {
            RoadCulling(lastPlayerPosition);
        }
    }

    private bool IsPlayerOnChunk(int chunkNo, Vector3 playerPosition)
    {
        if (chunkNo >= 0 && chunkNo < m_roadChunksSpawner.Count())
        {
            // On récupére la position du joueur sur la route, et on vérifie si il est présent sur le morceau de route spécifié par l'index "chunkNo"
            if (playerPosition.z > m_roadChunksSpawner.At(chunkNo).transform.position.z && playerPosition.z <= (m_roadChunksSpawner.At(chunkNo).transform.position.z + m_roadChunkHeight))
                return true;
        }

        return false;
    }

    private bool IsPlayerOnCenterOfTheRoad(Vector3 playerPosition)
    {
        // On récupére la position du joueur sur la route, et on vérifie si il est présent sur le morceau de route spécifié par l'index "chunkNo"
        if (playerPosition.z > ((m_nChunkGenerated / 2) * m_roadChunkHeight) && playerPosition.z <= ((m_nChunkGenerated / 2) * m_roadChunkHeight + m_roadChunkHeight))
            return true;
        

        return false;
    }

    private void GenerateEffectBox()
    {
        var leadingPlayer = GameManager.instance.m_leadingPlayer;
        var lastPlayer = GameManager.instance.m_lastPlayer;

        if (leadingPlayer != null && lastPlayer != null)
        {
            m_effectBoxSpawner.SpawnObject(new Vector3(0, 0, lastPlayer.transform.position.z + (m_obstacleGenerationOffset * 2) * m_roadChunkHeight));
            m_effectBoxSpawner.SpawnObject(new Vector3(0, 0, leadingPlayer.transform.position.z + (m_obstacleGenerationOffset * 2) * m_roadChunkHeight));
        }
    }

    private void GenerateRoadChunk()
    {
        m_roadChunksSpawner.SpawnObject(new Vector3(0, 0, m_nChunkGenerated * m_roadChunkHeight), m_roadChunkDefaultRot);
        m_nChunkGenerated++;

        // On crée un nouvel obstacle tout les m_crateGenerationOffset morceaux de route.
        if (m_nChunkGenerated % m_obstacleGenerationOffset == 0 && m_nChunkGenerated > 0)
        {
            m_obstaclesSpawner.SpawnObject(new Vector3(0, 0, m_nChunkGenerated * m_roadChunkHeight + (m_roadChunkHeight / 2)));
        }
    }

    private void GenerateRoadChunks(int nChunks)
    {
        for (var i = 0; i < nChunks; ++i)
            GenerateRoadChunk();
    }

    private void ManuallyDeleteEffectBox(GameObject gameObject)
    {
        m_effectBoxSpawner.RemoveObject(gameObject);
    }

    private void RoadCulling(Vector3 lastPlayerPosition)
    {
        m_roadChunksSpawner.ObjectsCulling(lastPlayerPosition - new Vector3(0, 0, m_roadChunkHeight));
        m_obstaclesSpawner.ObjectsCulling(lastPlayerPosition);
        m_effectBoxSpawner.ObjectsCulling(lastPlayerPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldManager : MonoBehaviour
{
    [SerializeField]
    int level = 0;

    public GameObject worldSpace;
    public MapChunk chunkPrefab;
    public GameObject buildingPrefab;
    public CubeMovement player;

    public MapChunk[] chunks;

    private void Awake()
    {
        InitializeChunks();

        player.transform.position = new Vector3(
            GameMetrics.chunkSizeX * 0.5f,
            20f,
            0f);
    }

    private void Update()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            if (chunks[i] == null)
            {
                chunks[i] = InitializeChunk(i);
            }
        }
    }

    void InitializeChunks()
    {
        chunks = new MapChunk[GameMetrics.chunkCount];

        for (int i = 0; i < GameMetrics.chunkCount; i++)
        {
            MapChunk chunk = InitializeChunk(i);

            Vector3 newPos = new Vector3(
            GameMetrics.chunkSizeX * 0.5f,
            0f,
            GameMetrics.chunkSizeZ * 0.5f);

            newPos.z += (i / (float)GameMetrics.chunkCount) *
                (GameMetrics.chunkCount * GameMetrics.chunkSizeZ);
            chunk.transform.position = newPos;

            chunks[i] = chunk;
        }
    }

    MapChunk InitializeChunk(int val)
    {
        MapChunk chunk = Instantiate(chunkPrefab);
        chunk.transform.parent = worldSpace.transform;

        chunk.transform.position = new Vector3(
            GameMetrics.chunkSizeX * 0.5f,
            0f,
            GameMetrics.chunkSpawnPosZ);

        chunk.Populate(level);
        level++;

        return chunk;
    }
}

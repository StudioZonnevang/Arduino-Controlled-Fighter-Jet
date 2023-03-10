using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldManager : MonoBehaviour
{
    [SerializeField]
    int level = 1;

    public GameObject worldSpace;
    public MapChunk chunkPrefab;
    public GameObject cellPrefab;
    public CubeMovement player;

    public MapChunk[] chunks;

    public float worldSpeed;

    private void Awake()
    {
        InitializeChunks();
        player.transform.position = new Vector3(
            GameMetrics.chunkSizeX * 0.5f,
            20f,
            15f);
        player.overWorldManager = this;
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

        updateChunkPos();
    }
    
    void updateChunkPos()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            Vector3 val = chunks[i].transform.position;
            val.z -= Time.deltaTime * player.MapSpeedMultiplier() * GameMetrics.mapSpeed;
            chunks[i].transform.position = val;
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
        chunk.cells = new Cell[GameMetrics.cellLength];
        //Debug.Log(GameMetrics.cellLength);

        chunk.transform.position = new Vector3(
            GameMetrics.chunkSizeX * 0.5f,
            0f,
            GameMetrics.chunkSpawnPosZ);

        int zCount = GameMetrics.chunkSizeZ / GameMetrics.cellSizeZ;
        int xCount = GameMetrics.chunkSizeX / GameMetrics.cellSizeX;

        Vector3 chunkOrigin;
        chunkOrigin.x = chunk.transform.position.x -
                    0.5f * GameMetrics.chunkSizeX;
        chunkOrigin.y = 0;
        chunkOrigin.z = chunk.transform.position.z -
                    0.5f * GameMetrics.chunkSizeZ;

        for (int z = 0, i = 0; z < zCount; z++)
        {
            for (int x = 0; x < xCount; x++, i++)
            {
                Vector3 pos;
                pos.x = x * GameMetrics.cellSizeX + (0.5f * GameMetrics.cellSizeX);
                pos.y = 0;
                pos.z = z * GameMetrics.cellSizeZ + (0.5f * GameMetrics.cellSizeZ);

                //Debug.Log(i);
                Cell newCell =
                    Instantiate(cellPrefab).GetComponent<Cell>();
                chunk.cells[i] = newCell;

                newCell.transform.SetParent(chunk.transform);
                newCell.transform.position = chunkOrigin + pos;
            }
        }

        chunk.level = level;
        chunk.LayInfrastructure();
        chunk.Populate(level);
        chunk.SpawnCollectables(level);
        level++;

        return chunk;
    }
}

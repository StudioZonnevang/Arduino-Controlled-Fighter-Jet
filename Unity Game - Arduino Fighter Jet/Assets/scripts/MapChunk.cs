using UnityEngine;

public class MapChunk : MonoBehaviour
{
    float moveSpeed = 30f;

    public GameObject[] buildingPrefabs;
    public GameObject[] collectables;

    GameObject[] population;
    public Cell[] cells;

    private void Awake()
    {
        cells =
            new Cell[(int)(0.5f * GameMetrics.chunkSizeX
            + 0.5f * GameMetrics.chunkSizeZ)];
    }

    private void Update()
    {
        UpdatePosition();

        if (transform.position.z < (-GameMetrics.chunkSizeZ * 0.5))
        {
            Disintegrate();
        }
    }

    void UpdatePosition()
    {
        Vector3 val = transform.position;
        val.z -= Time.deltaTime * moveSpeed;
        transform.position = val;
    }
    void spawnCollectables(int level)
    {

    }
    public void Populate(int level)
    {
        population = new GameObject[5 + level];

        for (int i = 0; i < population.Length; i++)
        {
            int index = Random.Range(0, cells.Length);

            if (cells[index] != null)
            {
                level %= 4;
                GameObject building = Instantiate(buildingPrefabs[level]);

                building.transform.position = cells[index].transform.position;
                building.transform.SetParent(cells[index].transform);
                population[i] = building;
            }
        }
    }
    void Disintegrate()
    {
        Debug.Log("destroying chunk");
        for (int i = 0; i < population.Length; i++)
        {
            Destroy(population[i]);
        }

        Destroy(gameObject);
    }

    public Cell GetCell(int index)
    {
        return cells[index];
    }
    public Cell[] GetNeighbors(int index)
    {
        Cell[] neighbors = new Cell[4];

        //north neighbor
        neighbors[0] = this.GetCell(index + GameMetrics.chunkSizeX);
        neighbors[1] = this.GetCell(index + 1);
        neighbors[2] = this.GetCell(index - GameMetrics.chunkSizeX);
        neighbors[3] = this.GetCell(index - 1);

        return neighbors;
    }
    public Cell GetNeighbor(int index, int dir)
    {
        if (dir == 0)
        {
            return this.GetCell(index + GameMetrics.chunkSizeX);
        }
        else if (dir == 1)
        {
            return this.GetCell(index + 1);
        }
        else if (dir == 2)
        {
            return this.GetCell(index - GameMetrics.chunkSizeX);
        }
        else if (dir == 3)
        {
            return this.GetCell(index - 1);
        }
        else
        {
            return null;
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapChunk : MonoBehaviour
{
    float moveSpeed = 30f;
    public int level;

    public GameObject[] buildingPrefabs;
    public GameObject[] collectables;

    GameObject[] population;
    public Cell[] cells;

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
    public void LayInfrastructure()
    {

    }
    public IEnumerator Populate()
    {
        yield return new WaitForSeconds(2);
    }
    public void Populate(int level)
    {
        //need to be moved to Coroutine
        population = new GameObject[5 + level];

        int i = 0;
        while (i < population.Length)
        {
            if (i >= GameMetrics.cellLength)
            {
                i = population.Length;
            }
            int index = GameMetrics.ReturnRandomValue(cells.Length);
            Debug.Log(index + " " + level);

            if (cells[index] != null)
            {
                if (!cells[index].IsOccupied)
                {
                    GameObject building =
                    Instantiate(buildingPrefabs
                    [GameMetrics.ReturnRandomValue(buildingPrefabs.Length)]);

                    building.transform.position = cells[index].transform.position;
                    building.transform.SetParent(cells[index].transform);
                    cells[index].IsOccupied = true;
                    population[i] = building;
                    i++;
                }
            }
        }
        /*
        for (int i = 0; i < population.Length;)
        {
            int index = Random.Range(0, cells.Length);

            if (cells[index] != null)
            {
                if (cells[index].IsOccupied)
                {
                    Debug.Log("is occupied");
                }

                GameObject building =
                    Instantiate(buildingPrefabs
                    [GameMetrics.ReturnRandomValue(buildingPrefabs.Length)]);

                building.transform.position = cells[index].transform.position;
                building.transform.SetParent(cells[index].transform);
                cells[index].IsOccupied = true;
                population[i] = building;
                i++;
            }
        }*/
    }
    public void SpawnCollectables(int level)
    {

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

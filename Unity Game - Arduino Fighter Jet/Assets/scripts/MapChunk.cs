using UnityEngine;

public class MapChunk : MonoBehaviour
{
    float moveSpeed = 30f;

    public GameObject buildingPrefab;

    GameObject[] population;

    private void Awake()
    {
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
    public void Populate(int level)
    {
        population = new GameObject[5 + level];

        for (int i = 0; i < population.Length; i++)
        {
            Vector3 Pos = new Vector3(
                Random.Range(0f, GameMetrics.chunkSizeX),
                0f,
                Random.Range(-0.5f * GameMetrics.chunkSizeZ, 0.5f
                * GameMetrics.chunkSizeZ));

            Pos.z += gameObject.transform.position.z;

            GameObject building = Instantiate(buildingPrefab);
            building.transform.position = Pos;
            building.transform.SetParent(this.transform);
            population[i] = building;
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
}

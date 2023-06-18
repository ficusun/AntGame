using UnityEngine;

[System.Serializable]
public class PheromoneSpawner
{
    private Transform myTransform;

    [SerializeField] private Pheromone pheromonePrefab;
    [SerializeField] private float pheromoneDistance;

    private Vector3 lastPheromonePos;

    public void Init(Ant ant)
    {
        myTransform = ant.transform;
    }

    public void TrySpawn()
    {
        if (Vector3.Distance(myTransform.position, lastPheromonePos) > pheromoneDistance)
        {
            SpawnPheromone();
        }
    }

    private void SpawnPheromone()
    {
        var currPos = myTransform.position;
        Object.Instantiate(pheromonePrefab, currPos, Quaternion.identity);
        lastPheromonePos = currPos;
    }
}

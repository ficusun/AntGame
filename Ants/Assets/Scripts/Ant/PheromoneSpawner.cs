using UnityEngine;

[System.Serializable]
public class PheromoneSpawner
{
    private Transform myTransform;

    [SerializeField] private Pheromone pheromonePrefab;
    [SerializeField] private float pheromoneDistance;

    public bool isSpawn = true;
    
    private Vector3 lastPheromonePos;

    public void Init(Ant ant)
    {
        myTransform = ant.transform;
    }

    public void TrySpawn()
    {
        var distance = Vector3.Distance(myTransform.position, lastPheromonePos);
        if (distance > pheromoneDistance && isSpawn)
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

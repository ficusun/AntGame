using System;

using UnityEditor;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField] public World world;
    [SerializeField] private Pheromone pheromonePrefab;
    [SerializeField] private VisionCollider visionCollider;

    [SerializeField] public AntView antView;
    [SerializeField] public AntMovement movement;
    [SerializeField] private PheromoneSpawner pheromoneSpawner;

    public TaskManager taskManager;
    [field: SerializeField] public AntStats Stats { get; private set; }

    public VisibleObject memorizedObject;
    [field: SerializeField] public float Energy { get; set; }
    [field: SerializeField] public float CurrentLoad { get; set; }
    
    public bool debug;

    private void Awake()
    {
        antView = new AntView();
        visionCollider.Init(antView);
        movement.Init(this);
        pheromoneSpawner.Init(this);
        
        taskManager.AddTask(new Searching(5, this));
    }

    private void Update()
    {
        taskManager.Manager();
        pheromoneSpawner.TrySpawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Pheromone>(out var pheromone))
        {
            pheromone.AddPower(50);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.TryGetComponent<Pheromone>(out var pheromone))
        {
            pheromoneSpawner.isSpawn = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Pheromone>(out var pheromone))
        {
            pheromoneSpawner.isSpawn = true;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     if (!debug) return;
    //
    //     var steeringAgent = movement.SteeringAgent;
    //
    //     Handles.color = Color.red;
    //     var point = transform.position + transform.right;
    //     Handles.DrawWireDisc(point, Vector3.back, steeringAgent.WanderRadius);
    //
    //     var wanderPoint = steeringAgent.CalculateWanderPoint(point);
    //     Handles.DrawLine(transform.position, wanderPoint);
    //
    //     Handles.DrawSolidDisc(wanderPoint, Vector3.back, 0.1f);
    // }
}

[System.Serializable]
public class AntStats
{
    public float MoveCost;
    public float MiningSpeed;
    public float MiningCost;
    public float CapacityLoad;
    public float MinTaskTime;
    public float LowEnergyAlarm;
}
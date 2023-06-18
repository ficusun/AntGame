using UnityEditor;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField] private World world;
    [SerializeField] private Pheromone pheromonePrefab;

    [SerializeField] private VisionCollider visionCollider;

    [SerializeField] private AntView antView;
    [SerializeField] private AntMovement movement;
    [SerializeField] private PheromoneSpawner pheromoneSpawner;

    [field: SerializeField] public AntStats Stats { get; private set; }
    [field: SerializeField] public float Energy { get; set; }
    [field: SerializeField] public float CurrentLoad { get; set; }

    public bool debug;

    public float testAngle;

    private void Awake()
    {
        antView = new AntView();
        visionCollider.Init(antView);
        movement.Init(this);
        pheromoneSpawner.Init(this);
    }

    private void Update()
    {
        movement.Move(ref world.bounds);
        pheromoneSpawner.TrySpawn();

        var direction = world.bounds.center - transform.position;
        testAngle = Vector3.Angle(direction, transform.right);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Pheromone>(out var pheromone))
        {
            pheromone.AddPower(50);
        }
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;

        var steeringAgent = movement.SteeringAgent;

        Handles.color = Color.red;
        var point = transform.position + transform.right;
        Handles.DrawLine(transform.position, point);
        Handles.DrawSolidDisc(point, Vector3.back, 0.1f);
        Handles.DrawWireDisc(point, Vector3.back, steeringAgent.WanderRadius);

        var wanderPoint = steeringAgent.CalculateWanderPoint(point);

        Handles.DrawWireDisc(wanderPoint, Vector3.back, 0.1f);
    }
}

[System.Serializable]
public class AntStats
{
    public float MoveCost;
    public float MiningSpeed;
    public float MiningCost;
    public float CapacityLoad;
}
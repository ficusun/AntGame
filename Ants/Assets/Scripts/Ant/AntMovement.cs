using UnityEngine;

[System.Serializable]
public class AntMovement
{
    private Transform myTransform;
    private Ant ant;

    [SerializeField] private float moveCost;
    [field: SerializeField] public SteeringAgent SteeringAgent { get; private set; }

    public void Init(Ant ant)
    {
        this.ant = ant;
        myTransform = ant.transform;

        var rb = ant.GetComponent<Rigidbody2D>();
        SteeringAgent.Init(rb);
    }

    public void Move()
    {
        if (ant.Energy < moveCost) return;
        
        if (ant.world.bounds.Contains(myTransform.position))
        {
            ant.Energy -= moveCost * Time.deltaTime;

            SteeringAgent.Wander();
        }
        else
        {
            SteeringAgent.Seek(ant.world.bounds.center);
            SteeringAgent.ReverseDirection();
        }
    }
    
    public void MoveToTarget(Vector3 target)
    {
        if (ant.Energy < moveCost) return;
        SteeringAgent.Seek(target);
    }
}
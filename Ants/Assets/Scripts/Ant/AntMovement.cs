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

    public void Move(ref Bounds bounds)
    {
        if (ant.Energy < moveCost) return;

        if (bounds.Contains(myTransform.position))
        {
            ant.Energy -= moveCost * Time.deltaTime;

            SteeringAgent.Wander();
        }
        else
        {
            SteeringAgent.Seek(bounds.center);
            SteeringAgent.ReverseDirection();
        }
    }
}
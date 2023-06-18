using UnityEngine;

[System.Serializable]
public class SteeringAgent
{
    private Rigidbody2D rb;

    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float MaxForce { get; private set; }
    [field: SerializeField] public float AngleChange { get; private set; }
    [field: SerializeField] public float AngleChangeRate { get; private set; }
    [field: SerializeField] public float WanderRadius { get; private set; }
    public Vector2 WanderPoint { get; private set; }

    private float angleChangeTimer;
    public float theta;

    public void Init(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void Seek(Vector2 targetPos)
    {
        var desired = targetPos - rb.position;
        desired = SetMagnitude(desired, MaxSpeed);

        var steeringForce = desired - rb.velocity;
        steeringForce = Vector2.ClampMagnitude(steeringForce, MaxForce);
        rb.AddForce(steeringForce);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
        rb.transform.right = rb.velocity;

    }

    public void Wander()
    {
        angleChangeTimer += Time.deltaTime;

        if (angleChangeTimer >= AngleChangeRate)
        {
            theta += Random.Range(-AngleChange, AngleChange);
            angleChangeTimer = 0;
        }

        WanderPoint = SetMagnitude(rb.velocity, 1);
        WanderPoint += rb.position;
        WanderPoint = CalculateWanderPoint(WanderPoint);

        Seek(WanderPoint);
    }

    public Vector2 CalculateWanderPoint(Vector2 center)
    {
        var wanderPoint = center;

        var angle = theta * Mathf.Deg2Rad;

        var x = WanderRadius * Mathf.Cos(angle);
        var y = WanderRadius * Mathf.Sin(angle);
        wanderPoint += new Vector2(x, y);

        return wanderPoint;
    }

    public void ReverseDirection()
    {
        theta = rb.transform.eulerAngles.z;
    }

    public static Vector3 SetMagnitude(Vector2 vector, float length)
    {
        return vector.normalized * length;
    }
}
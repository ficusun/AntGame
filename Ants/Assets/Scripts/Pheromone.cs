using UnityEngine;

public class Pheromone : VisibleObject
{
    [SerializeField]
    private float power;

    private void Update()
    {
        power -= Time.deltaTime;
        if (power <= 0f) Destroy(gameObject);
    }

    public void AddPower(float power)
    {
        this.power += power;
    }
}

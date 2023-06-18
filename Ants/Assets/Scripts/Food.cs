using System;
using UnityEngine;

public class Food : VisibleObject
{
    [SerializeField] private float energy;

    public event Action<Food> OnDestroyEvent;

    public const float SCALE_FACTOR = 0.10f;

    private void Start()
    {
        SetScale();
    }

    public void Init(float energy)
    {
        this.energy = energy;
    }

    public float DecreaseEnergy(float amount)
    {
        var res = Mathf.Min(energy, amount);
        energy -= amount;

        if (energy <= 0)
        {
            OnDestroyEvent?.Invoke(this);
            Destroy(gameObject);
        }

        SetScale();

        return res;
    }

    private void SetScale()
    {
        transform.localScale = Vector3.one * energy * SCALE_FACTOR;
    }

    private void OnValidate()
    {
        SetScale();
    }
}
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class FoodManager : MonoBehaviour
{
    public World world;

    public Food foodPrefab;

    public Dictionary<Food, Food> foods;

    public int foodCapacity;

    public int foodMinEnergy;
    public int foodMaxEnergy;

    private void Start()
    {
        foods = new Dictionary<Food, Food>();
    }

    private void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (foods.Count >= foodCapacity) return;
        var pos = new Vector3
        {
            x = Random.Range(-world.bounds.extents.x, world.bounds.extents.x),
            y = Random.Range(-world.bounds.extents.y, world.bounds.extents.y)
        };
        var food = Instantiate(foodPrefab, pos, Quaternion.identity);

        food.OnDestroyEvent += Remove;
        food.Init(Random.Range(foodMinEnergy, foodMaxEnergy));

        foods.Add(food, food);
    }

    private void Remove(Food food)
    {
        food.OnDestroyEvent -= Remove;
        foods.Remove(food);
    }
}

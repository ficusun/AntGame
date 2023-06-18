using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Search;

[Serializable]
public class Searching : AntAction
{
    public Ant ant;

    private Vector3? currentTarget;

    public Searching(int priority, Ant ant) : base(priority)
    {
        this.ant = ant;
    }

    public override void Action()
    {
        float closesDistance = float.MaxValue;

        if (currentTarget != null)
        {   
            // Debug.Log("currentTarget != null");
            // Debug.Log(currentTarget.Value);
            ant.movement.MoveToTarget(currentTarget.Value);
            if (Vector3.Distance(ant.transform.position, currentTarget.Value) < 1.05f)
            {
                currentTarget = null;
            }

            return;
        }

        foreach (var obj in ant.antView.objectInView)
        {
            if (obj.objectType == ObjectType.Pheromone)
            {
                var distance = Vector3.Distance(ant.transform.position, obj.transform.position);
                if (distance < closesDistance && distance > 1.1f)
                {
                    closesDistance = distance;
                    currentTarget = obj.transform.position;
                    // Debug.Log(ant.transform.localScale.x);
                    ant.memorizedObject = obj;
                }
            }
        }

        if (currentTarget == null)
        {   
            // Debug.Log("currentTarget == null");
            // Debug.Log(currentTarget);
            ant.movement.Move();
        }
    }
}
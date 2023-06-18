using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class TaskManager
{
    public int maxTask = 3;
    
    public List<AntAction> tasks = new List<AntAction>();

    public Ant ant;

    private float taskTimer;
    public AntAction CurrentTask;


    public void Manager()
    {
        taskTimer -= Time.deltaTime;
        CurrentTask?.Action();
        if (taskTimer > 0) return;

        if (tasks.Count == 0)
        {
            taskTimer = ant.Stats.MinTaskTime;
            return;
        }

        int maxPriority = 0;
        int id = 0;
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].Priority > maxPriority)
            {
                maxPriority = tasks[i].Priority;
                id = i;
            }
        }

        CurrentTask = tasks[id];
        taskTimer = ant.Stats.MinTaskTime;
        
        tasks.RemoveAt(id);
    }

    public void AddTask(AntAction newTask)
    {
        if (tasks.Count < maxTask)
        {
            tasks.Add(newTask);
            return;
        }
        
        int minPriority = 0;
        int id = 0;
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].Priority > minPriority)
            {
                minPriority = tasks[i].Priority;
                id = i;
            }
        }

        if (minPriority < newTask.Priority)
        {
            tasks.RemoveAt(id);
            tasks.Add(newTask);
        }
    }
}

// public struct AntTask
// {
//     public AntTaskType taskType;
//
//     public int Priority;
// }
//
// public enum AntTaskType
// {
//     Search,
//     Resting,
//     Eat,
//     Mining,
//     Mover, // move an object from A to B
//     TargetMove,
// }
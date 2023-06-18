using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public abstract class AntAction
{
    public int Priority;

    protected AntAction(int priority)
    {
        Priority = priority;
    }

    public abstract void Action();
}
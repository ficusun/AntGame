using UnityEngine;

public class World : MonoBehaviour
{
    public Bounds bounds;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
using Unity.Mathematics;

public class MathUtility
{
    public static float3 SetMagnitude(float3 vector, float length)
    {
        return math.normalizesafe(vector) * length;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "WeightedSpawnSO", menuName = "Scriptable Objects/WeightedSpawnSO")]
public class WeightedSpawnSO : ScriptableObject
{
    public GameObject enemy;
    [Range(0f, 1f)] public float MinWeight;
    [Range(0f, 1f)] public float MaxWeight;

    public float GetWeight()
    {
        return Random.Range(MinWeight,MaxWeight);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainDecoratorAsset", menuName = "Terrain Decorator Asset")]
public class TerrainDecoratorAsset : ScriptableObject
{
    public string decoratorName;
    public GameObject decoratorPrefab;
    public float radius;
    public float weight;

    public bool IsNormalAcceptable(Vector3 normal){
        // return Vector3.Angle(normal, Vector3.up) < 45f;
        return true;
    }
}

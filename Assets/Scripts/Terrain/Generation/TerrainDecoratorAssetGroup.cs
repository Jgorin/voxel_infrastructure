using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class creates a scriptable object that stores the noise options
[CreateAssetMenu(fileName = "TerrainDecoratorAssetGroup", menuName = "Terrain Decorator Asset Group")]
public class TerrainDecoratorAssetGroup : ScriptableObject
{
    public string groupName;
    public List<TerrainDecoratorAsset> decoratorPrefabs;
    public Vector2Int minMaxDecorators;

    public TerrainDecoratorAsset GetRandomDecoratorAsset(Vector3 position, Vector3 normal){
        // for each TerrainDecoratorAsset in the list, add the weight to the total weight if the normal is acceptable
        float totalWeight = 0;
        List<TerrainDecoratorAsset> acceptableAssets = new List<TerrainDecoratorAsset>();
        foreach(TerrainDecoratorAsset terrainDecoratorAsset in this.decoratorPrefabs){
            if(terrainDecoratorAsset.IsNormalAcceptable(normal)){
                totalWeight += terrainDecoratorAsset.weight;
                acceptableAssets.Add(terrainDecoratorAsset);
            }
        }

        // get a random number between 0 and the total weight
        float randomWeight = Random.Range(0.0f, totalWeight);
        while(randomWeight > 0){
            // subtract the weight of the first acceptable asset from the random weight
            randomWeight -= acceptableAssets[0].weight;
            // if the random weight is less than 0, return the first acceptable asset
            if(randomWeight < 0){
                return acceptableAssets[0];
            }
            // otherwise, remove the first acceptable asset from the list
            acceptableAssets.RemoveAt(0);
        }
        return null;
    }
}

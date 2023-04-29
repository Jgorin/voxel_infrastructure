using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunkDecorator : MonoBehaviour
{
    Dictionary<string, TerrainDecoratorAssetGroup> decoratorPrefabs;
    TerrainManager manager;

    public static TerrainChunkDecorator Init(TerrainManager manager){
        GameObject terrainChunkDecoratorGameObject = new GameObject("TerrainChunkDecorator");
        TerrainChunkDecorator terrainChunkDecorator = terrainChunkDecoratorGameObject.AddComponent<TerrainChunkDecorator>();
        terrainChunkDecorator.manager = manager;
        terrainChunkDecorator.decoratorPrefabs = new Dictionary<string, TerrainDecoratorAssetGroup>();
        terrainChunkDecorator.LoadDecoratorDictionary();
        return terrainChunkDecorator;
    }


    public List<TerrainDecoratorAsset> DecorateChunk(TerrainChunk chunk){
        // for now, use assets in the forest group
        TerrainDecoratorAssetGroup forestGroup = this.decoratorPrefabs["Forest"];
        List<TerrainDecoratorAsset> instantiatedDecorators = new List<TerrainDecoratorAsset>();

        // get a random number of decorators to instantiate
        int numDecorators = Random.Range(forestGroup.minMaxDecorators[0], forestGroup.minMaxDecorators[1]);

        int trials = 0;
        // get a random point on the chunk
        while(instantiatedDecorators.Count < numDecorators || trials < 10){
            Vector3 randomPosition;
            Vector3 randomNormal;
            (randomPosition, randomNormal) = chunk.GetRandomPointOnMesh();
            TerrainDecoratorAsset newDecoratorAsset = forestGroup.GetRandomDecoratorAsset(randomPosition, randomNormal);
            float squareRadius = newDecoratorAsset.radius * newDecoratorAsset.radius;
            
            // get all chunks in the decorator radius
            List<Vector3Int> chunksInRadius = this.manager.GetTerrainChunksInRadius(randomPosition, newDecoratorAsset.radius);
            foreach(Vector3Int chunkCoord in chunksInRadius){
                // get the chunk
                TerrainChunk chunkInRadius = this.manager.generatedChunks[chunkCoord];
                // get the decorators in the chunk
                List<TerrainDecoratorAsset> decorators = chunkInRadius.decorators;
                // get all decorators that are the same type as the new decorator
                List<TerrainDecoratorAsset> sameTypeDecorators = decorators.FindAll(decorator => decorator.decoratorName == newDecoratorAsset.decoratorName);
                
                // if no decorators inside the radius are the same type, instantiate the new decorator
                bool canInstantiate = true;
                for(int i = 0; i < sameTypeDecorators.Count; i++){
                    TerrainDecoratorAsset sameTypeDecorator = sameTypeDecorators[i];
                    // if the same type decorator is within the radius, skip
                    if (Mathematics.SquareDistance(randomPosition, sameTypeDecorator.decoratorPrefab.transform.position) < squareRadius){
                        canInstantiate = false;
                        break;
                    }
                }
                if (canInstantiate){
                    // instantiate the new decorator
                    GameObject newDecorator = Instantiate(newDecoratorAsset.decoratorPrefab, randomPosition, Quaternion.identity);
                    newDecorator.transform.up = randomNormal;
                    newDecoratorAsset.decoratorPrefab = newDecorator;
                    instantiatedDecorators.Add(newDecoratorAsset);
                    chunk.decorators.Add(newDecoratorAsset);
                    break;
                }else{
                    trials++;
                }
            }
        }

        return instantiatedDecorators;
    }


    public void LoadDecoratorDictionary(){
        Debug.Log("Test");
        // get the decorator asset groups from the resources folder at AssetGroups
        TerrainDecoratorAssetGroup[] terrainDecoratorAssetGroups = Resources.LoadAll<TerrainDecoratorAssetGroup>("AssetGroups");
        
        foreach(TerrainDecoratorAssetGroup terrainDecoratorAssetGroup in terrainDecoratorAssetGroups){
            Debug.Log(terrainDecoratorAssetGroup.groupName);
            this.decoratorPrefabs.Add(terrainDecoratorAssetGroup.groupName, terrainDecoratorAssetGroup);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Transform playerTransform;
    public TerrainGeneratorOptions terrainGeneratorOptions;
    public TerrainRendererOptions terrainRendererOptions;
    public Dictionary<Vector3Int, TerrainChunk> generatedChunks;
    public Dictionary<Vector3Int, TerrainChunk> renderedChunks;
    public TerrainGenerator terrainGenerator;
    public Material terrainMaterial;


    void Start(){
        this.renderedChunks = new Dictionary<Vector3Int, TerrainChunk>();
        this.generatedChunks = new Dictionary<Vector3Int, TerrainChunk>();
        this.terrainGenerator = this.gameObject.AddComponent<TerrainGenerator>();
    }


    public List<Vector3Int> GetTerrainChunksInRadius(Vector3 position, float radius){
        List<Vector3Int> chunks = new List<Vector3Int>();
        // convert position to chunk coords
        Vector3Int chunkCoords = Transformation.WorldToChunkCoords(position, this.terrainGeneratorOptions);
        // get the chunk radius
        int chunkRadius = Transformation.WorldToChunkCoords(Vector3.one * radius, this.terrainGeneratorOptions)[0];
        float squareRadius = Mathf.Pow(chunkRadius, 2);
        for (int x = -chunkRadius; x < chunkRadius; x++){
            for (int y = -chunkRadius; y < chunkRadius; y++){
                for (int z = -chunkRadius; z < chunkRadius; z++){
                    Vector3Int coords = new Vector3Int(x, y, z) + chunkCoords;
                    
                    // if outside of radius, skip
                    if (Mathematics.SquareDistance(coords, chunkCoords) > squareRadius){
                        continue;
                    }

                    chunks.Add(coords);
                }
            }
        }
        return chunks;
    }


    public int getRenderRadius(){
        return Mathf.FloorToInt(this.terrainRendererOptions.renderDistance / this.terrainGeneratorOptions.numPointsPerAxis);
    }

    
    public Vector3Int getPlayerChunkCoords(){
        return Transformation.WorldToChunkCoords(this.playerTransform.position, this.terrainGeneratorOptions);
    }
}

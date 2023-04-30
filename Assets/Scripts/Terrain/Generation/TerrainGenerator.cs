using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator: MonoBehaviour
{
    private Queue<TerrainChunk> chunksToGenerate;
    private TerrainManager manager;
    private ComputeBuffer densityMapBuffer;
    private ComputeBuffer densityBuffer;
    private ComputeBuffer voxelBuffer;
    private ComputeBuffer voxelCountBuffer;


    public void Start(){
        this.chunksToGenerate = new Queue<TerrainChunk>();
        TerrainManager manager = this.gameObject.GetComponent<TerrainManager>();
        this.manager = manager;
        int numVoxelsPerAxis = (int)(manager.terrainGeneratorOptions.numPointsPerAxis - manager.terrainGeneratorOptions.spacing);
        int numVoxels = numVoxelsPerAxis * numVoxelsPerAxis * numVoxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

        // create compute buffers for the chunk
        this.densityBuffer = new ComputeBuffer(manager.terrainGeneratorOptions.numPointsPerAxis * manager.terrainGeneratorOptions.numPointsPerAxis * manager.terrainGeneratorOptions.numPointsPerAxis, sizeof(float) * 4);
        this.voxelBuffer = new ComputeBuffer (maxTriangleCount, sizeof (float) * 3 * 3, ComputeBufferType.Append);
        this.voxelCountBuffer = new ComputeBuffer (1, sizeof (int), ComputeBufferType.Raw);
    }


    public void Update(){
        float timestamp = Time.realtimeSinceStartup;
        this.GenerateChunks(timestamp);
        if(Time.realtimeSinceStartup - timestamp < 0.016f){
            this.UpdateGenerationQueue();
        }
    }

    
    public void UpdateGenerationQueue(){
        Vector3Int playerCoords = this.manager.getPlayerChunkCoords();
        int radius = this.manager.getRenderRadius();
        float squareRadius = Mathf.Pow(radius, 2);
        
        List<Vector3Int> chunksInRenderDistance = this.manager.GetTerrainChunksInRadius(
            this.manager.playerTransform.position, 
            this.manager.terrainRendererOptions.renderDistance
        );

        foreach(Vector3Int chunk in chunksInRenderDistance){
            if(!this.manager.generatedChunks.ContainsKey(chunk)){
                this.AddChunkToGenerationQueue(chunk);
            }
            else{
                // check if terrain chunk is already rendered in the dictionary
                if (!this.manager.renderedChunks.ContainsKey(chunk)){
                    this.manager.renderedChunks.Add(chunk, this.manager.generatedChunks[chunk]);
                    this.manager.generatedChunks[chunk].gameObject.SetActive(true);
                }
            }
        }

        // unrender chunks that are too far away
        List<Vector3Int> coordsToRemove = new List<Vector3Int>();
        foreach(Vector3Int coords in this.manager.renderedChunks.Keys){
            if (Mathematics.SquareDistance(coords, playerCoords) > squareRadius){
                coordsToRemove.Add(coords);
            }
        }

        foreach(Vector3Int coords in coordsToRemove){
            this.manager.renderedChunks[coords].gameObject.SetActive(false);
            this.manager.renderedChunks.Remove(coords);
        }
    }


    public void AddChunkToGenerationQueue(Vector3Int coords){
        if (!this.manager.generatedChunks.ContainsKey(coords)){
            TerrainChunk chunk = TerrainChunk.Instantiate(coords, this.manager);
            this.chunksToGenerate.Enqueue(chunk);
            this.manager.generatedChunks.Add(coords, chunk);
            this.manager.renderedChunks.Add(coords, chunk);
        }
    }


    public void GenerateChunks(float timestamp){
        while (this.chunksToGenerate.Count > 0 && Time.realtimeSinceStartup - timestamp < 0.016f){
            TerrainChunk chunk = this.chunksToGenerate.Dequeue();
            voxelBuffer.SetCounterValue(0);
            chunk.Generate(this.densityBuffer, this.voxelBuffer, this.voxelCountBuffer);
        }
    }


    public void OnDestroy(){
        this.densityBuffer.Release();
        this.voxelBuffer.Release();
        this.voxelCountBuffer.Release();
    }
}

struct Triangle {
    #pragma warning disable 649 // disable unassigned variable warning
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;

    public Vector3 this [int i] {
        get {
            switch (i) {
                case 0:
                    return a;
                case 1:
                    return b;
                default:
                    return c;
            }
        }
    }
}

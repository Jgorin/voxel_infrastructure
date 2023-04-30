using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class TerrainChunk : MonoBehaviour
{
    // COMPUTE SHADER PARAMS
    private static ComputeShader densityShader = Resources.Load<ComputeShader>("NoiseMap");
    private static int densityKernel = densityShader.FindKernel("NoiseMap");
    private static ComputeShader voxelShader = Resources.Load<ComputeShader>("MarchingCubes");
    private static int voxelKernel = voxelShader.FindKernel("March");
    
    public Vector3Int coords;
    public bool isGenerated = false;
    private TerrainManager manager;
    private MeshFilter mf;
    private MeshRenderer mr;
    private MeshCollider mc;

    public static TerrainChunk Instantiate(Vector3Int coords, TerrainManager manager){
        GameObject chunk = new GameObject("chunk: " + coords);
        TerrainChunk terrainChunk = chunk.AddComponent<TerrainChunk>();
        terrainChunk.mf = terrainChunk.gameObject.AddComponent<MeshFilter>();
        terrainChunk.mr = terrainChunk.gameObject.AddComponent<MeshRenderer>();
        terrainChunk.mc = terrainChunk.gameObject.AddComponent<MeshCollider>();

        terrainChunk.manager = manager;
        terrainChunk.coords = coords;

        chunk.transform.position = Transformation.ChunkToWorldCoords(coords, terrainChunk.manager.terrainGeneratorOptions);
        return terrainChunk;
    }

    
    public void Generate(ComputeBuffer densityBuffer, ComputeBuffer voxelBuffer, ComputeBuffer voxelCountBuffer){
        this.GenerateDensityMap(densityBuffer);
        this.GenerateVoxelBuffer(densityBuffer, voxelBuffer);
        this.mf.mesh = this.GenerateMesh(voxelBuffer, voxelCountBuffer);
        this.mc.sharedMesh = this.mf.mesh;
        this.isGenerated = true;
        // add default material to meshrenderer
        this.mr.material = manager.terrainMaterial;
    }


    public void GenerateDensityMap(ComputeBuffer densityBuffer){
        // set compute shader parameters
        densityShader.SetBuffer(densityKernel, "points", densityBuffer);
        densityShader.SetVector("chunkWorldPos", this.gameObject.transform.position);
        densityShader.SetInt("numPointsPerAxis", this.manager.terrainGeneratorOptions.numPointsPerAxis);
        densityShader.SetInt("octaves", this.manager.terrainGeneratorOptions.octaves);
        densityShader.SetFloat("spacing", this.manager.terrainGeneratorOptions.spacing);
        densityShader.SetFloat("lacunarity", this.manager.terrainGeneratorOptions.lacunarity);
        densityShader.SetFloat("persistence", this.manager.terrainGeneratorOptions.persistence);
        densityShader.SetFloat("noiseScale", this.manager.terrainGeneratorOptions.noiseScale);
        densityShader.SetFloat("noiseWeight", this.manager.terrainGeneratorOptions.noiseWeight);
        densityShader.Dispatch(densityKernel, this.manager.terrainGeneratorOptions.numPointsPerAxis, this.manager.terrainGeneratorOptions.numPointsPerAxis, this.manager.terrainGeneratorOptions.numPointsPerAxis);
    }

    public void GenerateVoxelBuffer(ComputeBuffer densityBuffer, ComputeBuffer voxelBuffer){
        // set compute shader parameters
        voxelShader.SetBuffer(voxelKernel, "points", densityBuffer);
        voxelShader.SetBuffer(voxelKernel, "triangles", voxelBuffer);
        voxelShader.SetFloat("isoLevel", this.manager.terrainGeneratorOptions.isoLevel);
        voxelShader.SetInt("numPointsPerAxis", this.manager.terrainGeneratorOptions.numPointsPerAxis);
        voxelShader.Dispatch(voxelKernel, this.manager.terrainGeneratorOptions.numPointsPerAxis, this.manager.terrainGeneratorOptions.numPointsPerAxis, this.manager.terrainGeneratorOptions.numPointsPerAxis);
    }

    
    public Mesh GenerateMesh(ComputeBuffer voxelBuffer, ComputeBuffer voxelCountBuffer){
        // get the voxels from the compute buffer
        ComputeBuffer.CopyCount (voxelBuffer, voxelCountBuffer, 0);
        int[] voxelCountArray = { 0 };
        voxelCountBuffer.GetData (voxelCountArray);
        int numTris = voxelCountArray[0];
        Triangle[] tris = new Triangle[numTris];
        voxelBuffer.GetData(tris, 0, 0, numTris);
        
        Vector3[] vertices = new Vector3[numTris * 3];
        int[] triangles = new int[numTris * 3];

        for(int i = 0; i < numTris; i++){
            for(int j = 0; j < 3; j++){
                vertices[i * 3 + j] = tris[i][j];
                triangles[i * 3 + j] = i * 3 + j;
            }
        }

        // remove duplicate vertices
        Dictionary<Vector3, int> vertexMap = new Dictionary<Vector3, int>();
        List<Vector3> uniqueVertices = new List<Vector3>();
        for(int i = 0; i < vertices.Length; i++){
            if(!vertexMap.ContainsKey(vertices[i])){
                vertexMap.Add(vertices[i], uniqueVertices.Count);
                uniqueVertices.Add(vertices[i]);
            }
            triangles[i] = vertexMap[vertices[i]];
        }
        vertices = uniqueVertices.ToArray();

        // create mesh with the name as the chunk coords
        Mesh mesh = new Mesh();
        mesh.name = "chunk: " + this.coords;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }


    public (Vector3, Vector3) GetRandomPointOnMesh(){
        // get random point on mesh
        Vector3 randomPoint = this.mf.mesh.vertices[Random.Range(0, this.mf.mesh.vertices.Length)];
        // convert to world coords
        Vector3 worldPoint = this.transform.TransformPoint(randomPoint);
        // get the normal at that point
        Vector3 normal = this.mf.mesh.normals[Random.Range(0, this.mf.mesh.normals.Length)];
        return (worldPoint, normal);
    }


    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        float size = (this.manager.terrainGeneratorOptions.numPointsPerAxis - 1) * this.manager.terrainGeneratorOptions.spacing;
        // draw wirecube around chunk
        Gizmos.DrawWireCube(this.transform.position, new Vector3(size, size, size));
    }
}

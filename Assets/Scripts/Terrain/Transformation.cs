using UnityEngine;

public static class Transformation{
    public static Vector3Int WorldToChunkCoords(Vector3 worldCoords, TerrainGeneratorOptions options){
        float boundSize = (options.numPointsPerAxis - 1) * options.spacing;
        return new Vector3Int(
            Mathf.FloorToInt(worldCoords.x / boundSize),
            Mathf.FloorToInt(worldCoords.y / boundSize),
            Mathf.FloorToInt(worldCoords.z / boundSize)
        );
    }

    public static Vector3 ChunkToWorldCoords(Vector3Int chunkCoords, TerrainGeneratorOptions options){
        float boundSize = (options.numPointsPerAxis - 1) * options.spacing;
        return new Vector3(
            chunkCoords.x * boundSize,
            chunkCoords.y * boundSize,
            chunkCoords.z * boundSize
        );
    }
}
